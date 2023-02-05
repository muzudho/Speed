﻿namespace Assets.Scripts.Views.Timeline.Spans
{
    using Assets.Scripts.Engine.CommandArgs;
    using Assets.Scripts.Gui.Models;
    using Assets.Scripts.Views.Movements;
    using Assets.Scripts.Views.Moves;
    using SimulatorsOfTimeline = Assets.Scripts.Simulators;

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
        public override void CreateSpanToLerp(
            SimulatorsOfTimeline.TimeSpan timeSpan,
            GameModelBuffer gameModelBuffer,
            LazyArgs.SetValue<SpanToLerp> setViewMovement)
        {
            var gameModel = new GameModel(gameModelBuffer);
            var player = GetModel(timeSpan).Player;

            // ピックアップしているカードがあるか？
            GetIndexOfFocusedHandCard(
                gameModelBuffer: gameModelBuffer,
                player: player,
                (indexToRemove) =>  // 確定：場札から抜くのは何枚目
                {
                    var place = GetModel(timeSpan).Place;

                    if (CanRemoveHandCardAt(
                        gameModelBuffer: gameModelBuffer,
                        player: player,
                        indexToRemove: indexToRemove))
                    {
                        // 抜いた後の場札の数
                        int lengthAfterRemove;
                        {
                            // 抜く前の場札の数
                            var lengthBeforeRemove = gameModelBuffer.IdOfCardsOfPlayersHand[player].Count;
                            lengthAfterRemove = lengthBeforeRemove - 1;
                        }
                       
                        // （抜いた後に）次にピックアップするカード（が先頭から何枚目か）
                        int indexOfNextPick;
                        if (lengthAfterRemove <= indexToRemove) // 範囲外アクセス防止対応
                        {
                            // 一旦、最後尾へ
                            indexOfNextPick = lengthAfterRemove - 1;
                        }
                        else
                        {
                            // そのまま
                            indexOfNextPick = indexToRemove;
                        }

                        var target = gameModelBuffer.IdOfCardsOfPlayersHand[player][indexToRemove];

                        // モデル更新：場札を１枚抜く
                        gameModelBuffer.RemoveCardAtOfPlayerHand(player, indexToRemove);

                        // 確定：場札の枚数
                        var lengthOfHandCards = gameModel.GetLengthOfPlayerHandCards(player);
                        // 確定：抜いたあとの場札リスト
                        var idOfHandCardsAfterRemove = gameModel.GetCardsOfPlayerHand(player);

                        // モデル更新：何枚目の場札をピックアップしているか
                        gameModelBuffer.IndexOfFocusedCardOfPlayers[player] = indexOfNextPick;

                        // 場札からカードを抜く
                        {

                            // 場札の位置調整（をしないと歯抜けになる）
                            MoveToArrangeHandCards.Generate(
                                startSeconds: timeSpan.StartSeconds,
                                duration: timeSpan.Duration / 2.0f,
                                player: player,
                                indexOfPickup: indexOfNextPick, // 抜いたカードではなく、次にピックアップするカードを指定。 × indexToRemove
                                idOfHandCards: idOfHandCardsAfterRemove,
                                keepPickup: true,
                                setViewMovement: setViewMovement); // 場札

                            // TODO ★ ピックアップしている場札を持ち上げる
                            {

                            }
                        }

                        // 前の台札の天辺のカード
                        IdOfPlayingCards idOfPreviousTop = gameModel.GetTopOfCenterStack(place);

                        // 次に、台札として置く
                        gameModelBuffer.AddCardOfCenterStack(place, target);

                        // 台札へ置く
                        setViewMovement(MoveToPutCardToCenterStack.Generate(
                            startSeconds: timeSpan.StartSeconds + timeSpan.Duration / 2.0f,
                            duration: timeSpan.Duration / 2.0f,
                            player: player,
                            place: place,
                            target: target,
                            idOfPreviousTop));
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
            int indexToRemove)
        {
            // 抜く前の場札の数
            var lengthBeforeRemove = gameModelBuffer.IdOfCardsOfPlayersHand[player].Count;
            if (indexToRemove < 0 || lengthBeforeRemove <= indexToRemove)
            {
                // 抜くのに失敗
                return false;
            }


            return true;
        }
    }
}