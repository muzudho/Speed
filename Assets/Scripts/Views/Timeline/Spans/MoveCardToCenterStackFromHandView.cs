namespace Assets.Scripts.Views.Timeline.Spans
{
    using Assets.Scripts.Models;
    using Assets.Scripts.Models.Timeline.Spans;
    using Assets.Scripts.Simulators.Timeline;
    using Assets.Scripts.Views;
    using Assets.Scripts.Views.Movements;
    using Assets.Scripts.Views.Moves;
    using System;
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
        /// ゲーム画面の同期を始めます
        /// 
        /// - ｎプレイヤーがピックアップしている場札を、右（または左）の台札へ移動する
        /// </summary>
        /// <param name="player">何番目のプレイヤー</param>
        /// <param name="place">右なら0、左なら1</param>
        public override void OnEnter(
            SimulatorsOfTimeline.TimeSpan timeSpan,
            GameModelBuffer gameModelBuffer,
            LazyArgs.SetValue<ViewMovement> setViewMovement)
        {
            var gameModel = new GameModel(gameModelBuffer);
            var player = GetModel(timeSpan).Player;

            // ピックアップしているカードがあるか？
            GetIndexOfFocusedHandCard(
                gameModelBuffer: gameModelBuffer,
                player: player,
                (indexOfFocusedHandCard) =>
                {
                    var place = GetModel(timeSpan).Place;

                    if (CanRemoveHandCardAt(
                        gameModelBuffer: gameModelBuffer,
                        player: player,
                        getIndexOfHandCardToRemove: () => indexOfFocusedHandCard))
                    {
                        // 抜いた後の場札の数
                        int lengthAfterRemove;
                        {
                            // 抜く前の場札の数
                            var lengthBeforeRemove = gameModelBuffer.IdOfCardsOfPlayersHand[player].Count;
                            lengthAfterRemove = lengthBeforeRemove - 1;
                        }

                        // 抜いた後の次のピックアップするカードが先頭から何枚目か、先に算出
                        int indexOfNextFocusedHandCard;
                        if (lengthAfterRemove <= indexOfFocusedHandCard) // 範囲外アクセス防止対応
                        {
                            // 一旦、最後尾へ
                            indexOfNextFocusedHandCard = lengthAfterRemove - 1;
                        }
                        else
                        {
                            // そのまま
                            indexOfNextFocusedHandCard = indexOfFocusedHandCard;
                        }

                        var target = gameModelBuffer.IdOfCardsOfPlayersHand[player][indexOfFocusedHandCard];

                        // 確定：場札から抜くのは何枚目
                        var indexOfHandToRemove = gameModel.GetIndexOfFocusedCardOfPlayer(player);

                        // モデル更新：場札を１枚抜く
                        gameModelBuffer.RemoveCardAtOfPlayerHand(player, indexOfFocusedHandCard);

                        // 確定：場札の枚数
                        var lengthOfHandCards = gameModel.GetLengthOfPlayerHandCards(player);
                        // 確定：抜いたあとの場札リスト
                        var idOfHandCardsAfterRemove = gameModel.GetCardsOfPlayerHand(player);

                        // 場札からカードを抜く
                        {
                            // モデル更新：何枚目の場札をピックアップしているか
                            gameModelBuffer.IndexOfFocusedCardOfPlayers[GetModel(timeSpan).Player] = indexOfNextFocusedHandCard;

                            // 場札の位置調整（をしないと歯抜けになる）
                            MoveToArrangeHandCards.Generate(
                                startSeconds: timeSpan.StartSeconds,
                                duration: timeSpan.Duration / 2.0f,
                                player: GetModel(timeSpan).Player,
                                indexOfPickup: indexOfHandToRemove,// 場札から抜くのは何枚目
                                idOfHandCards: idOfHandCardsAfterRemove,
                                setViewMovement: setViewMovement); // 場札
                        }

                        // 台札の次の天辺（一番後ろ）のカードの中心座標 X, Z
                        Vector3 nextTop = GameView.GetPositionOfNextCenterStackCard(
                                    place: place,
                                    getCenterStack: () => gameModel.GetCenterStack(place));

                        // 台札へ置く
                        AddCardOfCenterStack(
                            startSeconds: timeSpan.StartSeconds + timeSpan.Duration / 2.0f,
                            duration: timeSpan.Duration / 2.0f,
                            target: target,
                            player: GetModel(timeSpan).Player,
                            place: place,
                            nextTop: nextTop,
                            addCardOfCenterStack: (results) => gameModelBuffer.AddCardOfCenterStack(results.Item1, results.Item2),
                            setViewMovement: (movementModel) =>
                            {
                                setViewMovement(movementModel); // 台札
                            });
                    }

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

        private bool CanRemoveHandCardAt(
            GameModelBuffer gameModelBuffer,
            int player,
            LazyArgs.GetValue<int> getIndexOfHandCardToRemove)
        {
            var indexOfHandCardToRemove = getIndexOfHandCardToRemove();

            // 抜く前の場札の数
            var lengthBeforeRemove = gameModelBuffer.IdOfCardsOfPlayersHand[player].Count;
            if (indexOfHandCardToRemove < 0 || lengthBeforeRemove <= indexOfHandCardToRemove)
            {
                // 抜くのに失敗
                return false;
            }


            return true;
        }

        /// <summary>
        /// 台札へ置く
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <param name="target"></param>
        /// <param name="player"></param>
        /// <param name="place"></param>
        /// <param name="addCardOfCenterStack"></param>
        /// <param name="setViewMovement"></param>
        private void AddCardOfCenterStack(
            float startSeconds,
            float duration,
            IdOfPlayingCards target,
            int player,
            int place,
            Vector3 nextTop,
            LazyArgs.SetValue<(int, IdOfPlayingCards)> addCardOfCenterStack,
            LazyArgs.SetValue<ViewMovement> setViewMovement)
        {
            addCardOfCenterStack((place, target));// 台札として置く

            setViewMovement(MoveCardToPutToCenterStack.Generate(
                startSeconds: startSeconds,
                duration: duration,
                player: player,
                place: place,
                nextTop: nextTop,
                target: target));
        }
    }
}
