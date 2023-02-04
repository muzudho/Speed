namespace Assets.Scripts.Views.Timeline.Spans
{
    using Assets.Scripts.Models;
    using Assets.Scripts.Models.Timeline.Spans;
    using Assets.Scripts.Simulators.Timeline;
    using Assets.Scripts.Views;
    using System;
    using UnityEngine;
    using SimulatorsOfTimeline = Assets.Scripts.Simulators.Timeline;

    /// <summary>
    /// 右（または左）側の台札１枚を、手札へ移動する
    /// </summary>
    class MoveCardsToPileFromCenterStacksView : AbstractSpanView
    {
        // - 生成

        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        public override ISpanView Spawn()
        {
            return new MoveCardsToPileFromCenterStacksView();
        }

        // - プロパティ

        MoveCardsToPileFromCenterStacksModel GetModel(SimulatorsOfTimeline.TimeSpan timeSpan)
        {
            return (MoveCardsToPileFromCenterStacksModel)timeSpan.SpanModel;
        }

        // - メソッド

        /// <summary>
        /// 台札を、手札へ移動する
        /// 
        /// - ゲーム開始時に使う
        /// </summary>
        /// <param name="place">右:0, 左:1</param>
        public override void OnEnter(
            SimulatorsOfTimeline.TimeSpan timeSpan,
            GameModelBuffer gameModelBuffer,
            LazyArgs.SetValue<ViewMovement> setViewMovement)
        {
            // 台札の一番上（一番後ろ）のカードを１枚抜く
            var numberOfCards = 1;
            var length = gameModelBuffer.IdOfCardsOfCenterStacks[GetModel(timeSpan).Place].Count; // 台札の枚数
            if (1 <= length)
            {
                var startIndex = length - numberOfCards;
                var idOfCardOfCenterStack = gameModelBuffer.IdOfCardsOfCenterStacks[GetModel(timeSpan).Place][startIndex]; // 台札の１番上のカード
                gameModelBuffer.RemoveCardAtOfCenterStack(GetModel(timeSpan).Place, startIndex);

                // 黒いカードは１プレイヤー、赤いカードは２プレイヤー
                int player;
                float angleY;
                var suit = idOfCardOfCenterStack.Suit();
                switch (suit)
                {
                    case IdOfCardSuits.Clubs:
                    case IdOfCardSuits.Spades:
                        player = 0;
                        angleY = 180.0f;
                        break;

                    case IdOfCardSuits.Diamonds:
                    case IdOfCardSuits.Hearts:
                        player = 1;
                        angleY = 0.0f;
                        break;

                    default:
                        throw new Exception();
                }

                // プレイヤーの手札を積み上げる
                gameModelBuffer.AddCardOfPlayersPile(player, idOfCardOfCenterStack);

                // 台札から手札へ移動するカードについて
                var idOfGameObjectOfCard = Specification.GetIdOfGameObject(idOfCardOfCenterStack);
                var goCard = GameObjectStorage.Items[idOfGameObjectOfCard];
                setViewMovement(new ViewMovement(
                    startSeconds: timeSpan.StartSeconds,
                    duration: timeSpan.Duration,
                    target: idOfGameObjectOfCard,
                    getBegin: ()=> new PositionAndRotationLazy(
                        getPosition: ()=>goCard.transform.position,
                        getRotation: () => goCard.transform.rotation),
                    getEnd: ()=> new PositionAndRotationLazy(
                        getPosition: ()=>
                            {
                                // 現在の天辺の手札のポジションより１枚分上、または、一番下
                                Vector3 positionOfTop;
                                {
                                    var length = gameModelBuffer.IdOfCardsOfPlayersPile[player].Count;

                                    // 手札が１枚も無ければ
                                    if (length < 1)
                                    {
                                        // 一番下
                                        positionOfTop = GameView.positionOfPileCardsOrigin[player].ToMutable();
                                    }
                                    // 既存の手札があれば
                                    else
                                    {
                                        var idOfTop = gameModelBuffer.IdOfCardsOfPlayersPile[player][length - 1];
                                        var goCardOfTop = GameObjectStorage.Items[Specification.GetIdOfGameObject(idOfTop)];
                                        // より、１枚分上
                                        positionOfTop = goCardOfTop.transform.position;
                                    }
                                }

                                return positionOfTop;
                            },
                        getRotation: ()=> Quaternion.Euler(0, angleY, 180.0f))));
            }
        }
    }
}
