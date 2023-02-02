namespace Assets.Scripts.Models.Timeline.Spans
{
    using Assets.Scripts.Models;
    using Assets.Scripts.Views;
    using UnityEngine;

    /// <summary>
    /// ｎプレイヤーがピックアップしている場札を、右（または左）の台札へ移動する
    /// </summary>
    class MoveCardToCenterStackFromHand : AbstractSpan
    {
        // - 生成

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startSeconds">ゲーム内時間（秒）</param>
        /// <param name="duration">持続時間（秒）</param>
        /// <param name="player"></param>
        /// <param name="place"></param>
        internal MoveCardToCenterStackFromHand(float startSeconds, float duration, int player, int place)
            : base(startSeconds, duration)
        {
            this.Player = player;
            this.Place = place;
        }

        // - プロパティ

        int Player { get; set; }
        int Place { get; set; }

        // - メソッド

        /// <summary>
        /// ｎプレイヤーがピックアップしている場札を、右（または左）の台札へ移動する
        /// </summary>
        /// <param name="player">何番目のプレイヤー</param>
        /// <param name="place">右なら0、左なら1</param>
        public override void OnEnter(GameModelBuffer gameModelBuffer, GameViewModel gameViewModel)
        {
            var gameModel = new GameModel(gameModelBuffer);

            // ピックアップしているカードがあるか？
            GetIndexOfFocusedHandCard(
                gameModelBuffer: gameModelBuffer,
                player: Player,
                (indexOfFocusedHandCard) =>
                {
                    RemoveAtOfHandCard(
                        gameModelBuffer: gameModelBuffer,
                        gameViewModel: gameViewModel,
                        player: Player,
                        place: Place,
                        indexOfHandCardToRemove: indexOfFocusedHandCard,
                        setIndexOfNextFocusedHandCard: (indexOfNextFocusedHandCard) =>
                        {
                            gameModelBuffer.IndexOfFocusedCardOfPlayers[Player] = indexOfNextFocusedHandCard; // 更新：何枚目の場札をピックアップしているか

                            // 場札の位置調整（をしないと歯抜けになる）
                            gameViewModel.ArrangeHandCards(
                                gameModel: gameModel,
                                player: Player);
                        });
                });
        }

        private void GetIndexOfFocusedHandCard(GameModelBuffer gameModelBuffer, int player, LazyArgs.SetValue<int> setIndex)
        {
            int handIndex = gameModelBuffer.IndexOfFocusedCardOfPlayers[player]; // 何枚目の場札をピックアップしているか
            if (handIndex < 0 || gameModelBuffer.IdOfCardsOfPlayersHand[player].Count <= handIndex) // 範囲外は無視
            {
                return;
            }

            setIndex(handIndex);
        }


        /// <summary>
        /// 台札を抜く
        /// </summary>
        /// <param name="player"></param>
        /// <param name="indexOfHandCardToRemove"></param>
        /// <param name="setIndexOfNextFocusedHandCard"></param>
        private void RemoveAtOfHandCard(GameModelBuffer gameModelBuffer, GameViewModel gameViewModel, int player, int place, int indexOfHandCardToRemove, LazyArgs.SetValue<int> setIndexOfNextFocusedHandCard)
        {
            // 抜く前の場札の数
            var lengthBeforeRemove = gameModelBuffer.IdOfCardsOfPlayersHand[player].Count;
            if (indexOfHandCardToRemove < 0 || lengthBeforeRemove <= indexOfHandCardToRemove)
            {
                // 抜くのに失敗
                return;
            }

            // 抜いた後の場札の数
            var lengthAfterRemove = lengthBeforeRemove - 1;

            // 抜いた後の次のピックアップするカードが先頭から何枚目か、先に算出
            int indexOfNextFocusedHandCard;
            if (lengthAfterRemove <= indexOfHandCardToRemove) // 範囲外アクセス防止対応
            {
                // 一旦、最後尾へ
                indexOfNextFocusedHandCard = lengthAfterRemove - 1;
            }
            else
            {
                // そのまま
                indexOfNextFocusedHandCard = indexOfHandCardToRemove;
            }

            var goCard = gameModelBuffer.IdOfCardsOfPlayersHand[player][indexOfHandCardToRemove]; // 場札を１枚抜いて
            gameModelBuffer.RemoveCardAtOfPlayerHand(player, indexOfHandCardToRemove);

            AddCardOfCenterStack2(gameModelBuffer, gameViewModel, goCard, place); // 台札
            setIndexOfNextFocusedHandCard(indexOfNextFocusedHandCard);
        }

        private void AddCardOfCenterStack2(GameModelBuffer gameModelBuffer, GameViewModel gameViewModel, IdOfPlayingCards idOfCard, int place)
        {
            var gameModel = new GameModel(gameModelBuffer);

            // 手ぶれ
            var (shakeX, shakeZ, shakeAngleY) = gameViewModel.MakeShakeForCenterStack(place);

            // 台札の次の天辺（一番後ろ）のカードの中心座標 X, Z
            var (nextTopX, nextTopZ) = gameViewModel.GetXZOfNextCenterStackCard(gameModel, place);

            // 台札の捻り
            var goCard = GameObjectStorage.PlayingCards[idOfCard];
            float nextAngleY = goCard.transform.rotation.eulerAngles.y;
            var length = gameModel.GetLengthOfCenterStackCards(place);
            if (length < 1)
            {
            }
            else
            {
                nextAngleY += shakeAngleY;
            }

            gameModelBuffer.AddCardOfCenterStack(place, idOfCard); // 台札として置く

            // 台札の位置をセット
            var movement = new Movement(
                startSeconds: this.StartSeconds,
                duration: this.Duration,
                beginPosition: goCard.transform.position,
                endPosition: new Vector3(nextTopX + shakeX, gameViewModel.centerStacksY[place], nextTopZ + shakeZ),
                beginRotation: goCard.transform.rotation,
                endRotation: Quaternion.Euler(0, nextAngleY, 0.0f),
                gameObject: goCard);
            movement.Lerp(progress: 1.0f);

            // 次に台札に積むカードの高さ
            gameViewModel.centerStacksY[place] += 0.2f;
        }
    }
}
