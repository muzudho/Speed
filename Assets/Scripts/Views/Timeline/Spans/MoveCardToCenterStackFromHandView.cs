namespace Assets.Scripts.Views.Timeline.Spans
{
    using Assets.Scripts.Models;
    using Assets.Scripts.Models.Timeline.Spans;
    using Assets.Scripts.Simulators.Timeline;
    using Assets.Scripts.Views;
    using UnityEngine;
    using SimulatorsOfTimeline = Assets.Scripts.Simulators.Timeline;

    /// <summary>
    /// ｎプレイヤーがピックアップしている場札を、右（または左）の台札へ移動する
    /// </summary>
    class MoveCardToCenterStackFromHandView : AbstractSpanView
    {
        // - 生成

        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        public override ISpanView Spawn()
        {
            return new MoveCardToCenterStackFromHandView();
        }

        // - プロパティ

        MoveCardToCenterStackFromHandModel GetModel(SimulatorsOfTimeline.TimeSpan timeSpan)
        {
            return (MoveCardToCenterStackFromHandModel)timeSpan.SpanModel;
        }

        // - メソッド

        /// <summary>
        /// ｎプレイヤーがピックアップしている場札を、右（または左）の台札へ移動する
        /// </summary>
        /// <param name="player">何番目のプレイヤー</param>
        /// <param name="place">右なら0、左なら1</param>
        public override void OnEnter(
            TimeSpan timeSpan,
            GameModelBuffer gameModelBuffer,
            GameViewModel gameViewModel,
            LazyArgs.SetValue<MovementViewModel> setMovementViewModel)
        {
            var gameModel = new GameModel(gameModelBuffer);

            // ピックアップしているカードがあるか？
            GetIndexOfFocusedHandCard(
                gameModelBuffer: gameModelBuffer,
                player: GetModel(timeSpan).Player,
                (indexOfFocusedHandCard) =>
                {
                    var place = GetModel(timeSpan).Place;
                    RemoveAtOfHandCard(
                        timeSpan: timeSpan,
                        gameModelBuffer: gameModelBuffer,
                        gameViewModel: gameViewModel,
                        player: GetModel(timeSpan).Player,
                        place: place,
                        getIndexOfHandCardToRemove: () => indexOfFocusedHandCard,
                        getNumberOfCenterStackCards: () => gameModel.GetLengthOfCenterStackCards(place),
                        getNextTopOfCenterStackCard: () =>
                        {
                            return gameViewModel.GetPositionOfNextCenterStackCard(
                                place: place,
                                getCenterStack: ()=>gameModel.GetCenterStack(place));
                        },
                        setIndexOfNextFocusedHandCard: (indexOfNextFocusedHandCard) =>
                        {
                            gameModelBuffer.IndexOfFocusedCardOfPlayers[GetModel(timeSpan).Player] = indexOfNextFocusedHandCard; // 更新：何枚目の場札をピックアップしているか

                            //// もし、場札が空っぽのところへ、手札を配ったのなら、先頭の場札をピックアップする
                            //if (gameModelBuffer.IndexOfFocusedCardOfPlayers[Player] == -1)
                            //{
                            //    gameModelBuffer.IndexOfFocusedCardOfPlayers[Player] = 0;
                            //}

                            var player = GetModel(timeSpan).Player;
                            int numberOfCards = gameModel.GetLengthOfPlayerHandCards(player); // 場札の枚数
                            if (0 < numberOfCards)
                            {
                                // 場札の位置調整（をしないと歯抜けになる）
                                MovementGenerator.ArrangeHandCards(
                                    startSeconds: timeSpan.StartSeconds,
                                    duration: timeSpan.Duration / 2.0f,
                                    player: GetModel(timeSpan).Player,
                                    getNumberOfHandCards: () => gameModel.GetLengthOfPlayerHandCards(player),// 場札の枚数
                                    getIndexOfPickup: () => gameModel.GetIndexOfFocusedCardOfPlayer(player),
                                    getIdOfHands: () => gameModel.GetCardsOfPlayerHand(player),
                                    getZOfHandCardsOrigin: gameViewModel.GetZOfHandCardsOrgin(),
                                    setCardMovementModel: setMovementViewModel); // 場札
                            }

                        },
                        setCardMovementModel: (movementModel) =>
                        {
                            setMovementViewModel(movementModel); // 台札

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
            TimeSpan timeSpan,
            GameModelBuffer gameModelBuffer,
            GameViewModel gameViewModel,
            int player,
            int place,
            LazyArgs.GetValue<int> getIndexOfHandCardToRemove,
            LazyArgs.GetValue<int> getNumberOfCenterStackCards,
            LazyArgs.GetValue<Vector3> getNextTopOfCenterStackCard,
            LazyArgs.SetValue<int> setIndexOfNextFocusedHandCard,
            LazyArgs.SetValue<MovementViewModel> setCardMovementModel)
        {
            var indexOfHandCardToRemove = getIndexOfHandCardToRemove();

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

            AddCardOfCenterStack2(
                timeSpan: timeSpan,
                idOfCard: goCard,
                place: place,
                getNumberOfCenterStackCards: getNumberOfCenterStackCards,
                getNextTopOfCenterStackCard: getNextTopOfCenterStackCard,
                addCardOfCenterStack: (results) => gameModelBuffer.AddCardOfCenterStack(results.Item1, results.Item2),
                setCardMovementModel: setCardMovementModel); // 台札

            setIndexOfNextFocusedHandCard(indexOfNextFocusedHandCard);
        }

        private void AddCardOfCenterStack2(
            TimeSpan timeSpan,
            IdOfPlayingCards idOfCard,
            int place,
            LazyArgs.GetValue<int> getNumberOfCenterStackCards,
            LazyArgs.GetValue<Vector3> getNextTopOfCenterStackCard,
            LazyArgs.SetValue<(int, IdOfPlayingCards)> addCardOfCenterStack,
            LazyArgs.SetValue<MovementViewModel> setCardMovementModel)
        {
            // 手ぶれ
            var (shakeX, shakeZ, shakeAngleY) = ViewHelper.MakeShakeForCenterStack(place);

            // 台札の次の天辺（一番後ろ）のカードの中心座標 X, Z
            Vector3 nextTop = getNextTopOfCenterStackCard();

            // 台札の捻り
            var goCard = GameObjectStorage.Items[Specification.GetIdOfGameObject(idOfCard)]; // TODO ビューが必要？
            float nextAngleY = goCard.transform.rotation.eulerAngles.y;
            var numberOfCenterStackCards = getNumberOfCenterStackCards();
            if (numberOfCenterStackCards < 1)
            {
            }
            else
            {
                nextAngleY += shakeAngleY;
            }

            addCardOfCenterStack((place, idOfCard));// 台札として置く

            // 台札の位置をセット
            var idOfGo = Specification.GetIdOfGameObject(idOfCard);
            setCardMovementModel(new MovementViewModel(
                startSeconds: timeSpan.StartSeconds + timeSpan.Duration / 2.0f,
                duration: timeSpan.Duration / 2.0f,
                getBeginPosition: () => goCard.transform.position,
                getEndPosition: () => new Vector3(nextTop.x + shakeX, nextTop.y, nextTop.z + shakeZ),
                getBeginRotation: () => goCard.transform.rotation,
                getEndRotation: () => Quaternion.Euler(0, nextAngleY, 0.0f),
                idOfGameObject: idOfGo));
        }
    }
}
