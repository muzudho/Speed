﻿namespace Assets.Scripts.Views.Timeline.Spans
{
    using Assets.Scripts.Gui.Models;
    using Assets.Scripts.Gui.Models.Timeline.Spans;
    using Assets.Scripts.Views.Movements;
    using UnityEngine;
    using SimulatorsOfTimeline = Assets.Scripts.Simulators;

    /// <summary>
    /// ｎプレイヤーの手札から場札へ、ｍ枚のカードを移動
    /// </summary>
    class MoveCardsToHandFromPileView : AbstractSpanView
    {
        // - その他（生成）

        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        public override ISpanView Spawn()
        {
            return new MoveCardsToHandFromPileView();
        }

        // - プロパティ

        MoveCardsToHandFromPileModel GetModel(SimulatorsOfTimeline.TimeSpan timeSpan)
        {
            return (MoveCardsToHandFromPileModel)timeSpan.SpanModel;
        }

        // - メソッド

        /// <summary>
        /// ゲーム画面の同期を始めます
        /// 
        /// - 手札の上の方からｎ枚抜いて、場札の後ろへ追加する
        /// - 画面上の場札は位置調整される
        /// </summary>
        public override void OnEnter(
            SimulatorsOfTimeline.TimeSpan timeSpan,
            GameModelBuffer gameModelBuffer,
            LazyArgs.SetValue<SpanToLerp> setViewMovement)
        {
            var length = gameModelBuffer.IdOfCardsOfPlayersPile[GetModel(timeSpan).Player].Count; // 手札の枚数

            if (length < GetModel(timeSpan).NumberOfCards)
            {
                // できない指示は無視
                Debug.Log("[MoveCardsToHandFromPileView OnEnter] できない指示は無視");
                return;
            }

            var player = GetModel(timeSpan).Player;

            // TODO ★ 状態変更をして、ビューが再生する感じ？
            // TODO ★ ビューは、状態にアクセスせず再生できる必要がある
            // 天辺から取っていく
            gameModelBuffer.MoveCardsToHandFromPile(
                player: player,
                startIndex: length - GetModel(timeSpan).NumberOfCards,
                numberOfCards: GetModel(timeSpan).NumberOfCards);

            // もし、場札が空っぽのところへ、手札を配ったのなら、先頭の場札をピックアップする
            if (gameModelBuffer.IndexOfFocusedCardOfPlayers[player] == -1)
            {
                gameModelBuffer.IndexOfFocusedCardOfPlayers[player] = 0;
            }


            // ビュー：場札の位置の再調整（をしないと、手札から移動しない）
            GameModel gameModel = new GameModel(gameModelBuffer);
            int numberOfCards = gameModel.GetLengthOfPlayerHandCards(player);
            if (0 < numberOfCards)
            {
                MoveToArrangeHandCards.Generate(
                    startSeconds: timeSpan.StartSeconds,
                    duration: timeSpan.Duration,
                    player: player,
                    indexOfPickup: gameModel.GetIndexOfFocusedCardOfPlayer(player),
                    idOfHandCards: gameModel.GetCardsOfPlayerHand(player),
                    keepPickup: true,
                    setViewMovement: setViewMovement);
            }

            // TODO ★ ピックアップしている場札を持ち上げる
            {

            }
        }
    }
}
