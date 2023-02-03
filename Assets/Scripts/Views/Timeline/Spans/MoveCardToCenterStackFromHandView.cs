namespace Assets.Scripts.Views.Timeline.Spans
{
    using Assets.Scripts.Models;
    using Assets.Scripts.Models.Timeline.Spans;
    using Assets.Scripts.Views;
    using UnityEngine;
    using ViewsOfTimeline = Assets.Scripts.Views.Timeline;

    /// <summary>
    /// ｎプレイヤーがピックアップしている場札を、右（または左）の台札へ移動する
    /// </summary>
    class MoveCardToCenterStackFromHandView : AbstractSpanModel
    {
        // - 生成

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeSpan">タイム・スパン</param>
        /// <param name="model">モデル</param>
        internal MoveCardToCenterStackFromHandView(ViewsOfTimeline.TimeSpan timeSpan, MoveCardToCenterStackFromHandModel model)
            : base(timeSpan)
        {
            this.Model = model;
        }

        // - プロパティ

        MoveCardToCenterStackFromHandModel Model { get; set; }

        // - メソッド

        /// <summary>
        /// ｎプレイヤーがピックアップしている場札を、右（または左）の台札へ移動する
        /// </summary>
        /// <param name="player">何番目のプレイヤー</param>
        /// <param name="place">右なら0、左なら1</param>
        public override void OnEnter(
            GameModelBuffer gameModelBuffer,
            GameViewModel gameViewModel,
            LazyArgs.SetValue<CardMovementViewModel> setCardMovementModel)
        {
            var gameModel = new GameModel(gameModelBuffer);

            // ピックアップしているカードがあるか？
            GetIndexOfFocusedHandCard(
                gameModelBuffer: gameModelBuffer,
                player: this.Model.Player,
                (indexOfFocusedHandCard) =>
                {
                    RemoveAtOfHandCard(
                        gameModelBuffer: gameModelBuffer,
                        gameViewModel: gameViewModel,
                        player: this.Model.Player,
                        place: this.Model.Place,
                        indexOfHandCardToRemove: indexOfFocusedHandCard,
                        setIndexOfNextFocusedHandCard: (indexOfNextFocusedHandCard) =>
                        {
                            gameModelBuffer.IndexOfFocusedCardOfPlayers[this.Model.Player] = indexOfNextFocusedHandCard; // 更新：何枚目の場札をピックアップしているか

                            //// もし、場札が空っぽのところへ、手札を配ったのなら、先頭の場札をピックアップする
                            //if (gameModelBuffer.IndexOfFocusedCardOfPlayers[Player] == -1)
                            //{
                            //    gameModelBuffer.IndexOfFocusedCardOfPlayers[Player] = 0;
                            //}

                            // 場札の位置調整（をしないと歯抜けになる）
                            gameViewModel.ArrangeHandCards(
                                startSeconds: this.TimeSpan.StartSeconds,
                                duration1: this.TimeSpan.Duration / 4.0f,
                                duration2: this.TimeSpan.Duration / 4.0f,
                                gameModel: gameModel,
                                player: this.Model.Player,
                                setCardMovementModel: setCardMovementModel); // 場札
                        },
                        setCardMovementModel: (movementModel) =>
                        {
                            setCardMovementModel(movementModel); // 台札

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
        private void RemoveAtOfHandCard(
            GameModelBuffer gameModelBuffer,
            GameViewModel gameViewModel,
            int player,
            int place,
            int indexOfHandCardToRemove,
            LazyArgs.SetValue<int> setIndexOfNextFocusedHandCard,
            LazyArgs.SetValue<CardMovementViewModel> setCardMovementModel)
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

            AddCardOfCenterStack2(gameModelBuffer, gameViewModel, goCard, place,
                setCardMovementModel: setCardMovementModel); // 台札
            setIndexOfNextFocusedHandCard(indexOfNextFocusedHandCard);
        }

        private void AddCardOfCenterStack2(
            GameModelBuffer gameModelBuffer,
            GameViewModel gameViewModel,
            IdOfPlayingCards idOfCard,
            int place,
            LazyArgs.SetValue<CardMovementViewModel> setCardMovementModel)
        {
            var gameModel = new GameModel(gameModelBuffer);

            // 手ぶれ
            var (shakeX, shakeZ, shakeAngleY) = gameViewModel.MakeShakeForCenterStack(place);

            // 台札の次の天辺（一番後ろ）のカードの中心座標 X, Z
            var (nextTopX, nextTopZ) = gameViewModel.GetXZOfNextCenterStackCard(gameModel, place);

            // 台札の捻り
            var goCard = GameObjectStorage.PlayingCards[idOfCard]; // TODO ビューが必要？
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
            setCardMovementModel(new CardMovementViewModel(
                startSeconds: this.TimeSpan.StartSeconds + this.TimeSpan.Duration / 2.0f,
                duration: this.TimeSpan.Duration / 2.0f,
                beginPosition: goCard.transform.position,
                endPosition: new Vector3(nextTopX + shakeX, gameViewModel.centerStacksY[place], nextTopZ + shakeZ),
                beginRotation: goCard.transform.rotation,
                endRotation: Quaternion.Euler(0, nextAngleY, 0.0f),
                idOfCard: idOfCard));

            // 次に台札に積むカードの高さ
            gameViewModel.centerStacksY[place] += 0.2f;
        }
    }
}
