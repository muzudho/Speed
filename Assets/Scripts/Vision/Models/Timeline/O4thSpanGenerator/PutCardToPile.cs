namespace Assets.Scripts.Vision.Models.Timeline.O4thSpanGenerator
{
    using Assets.Scripts.ThinkingEngine;
    using Assets.Scripts.ThinkingEngine.Models;
    using Assets.Scripts.Vision.Models.World;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// 指定のカード（台札を想定）を手札へ移動
    /// </summary>
    internal static class PutCardToPile
    {
        /// <summary>
        /// ムーブメント生成
        /// </summary>
        /// <param name="startSeconds">ゲーム内時間（秒）</param>
        /// <param name="duration">持続時間（秒）</param>
        /// <param name="target">ゲーム・オブジェクトId</param>
        /// <returns></returns>
        internal static IFinalLevelSpan Generate(
            float startSeconds,
            float duration,
            Player playerObj,
            List<IdOfPlayingCards> idOfPlayerPileCards,
            IdOfPlayingCards idOfPlayingCard)
        {
            // 台札から手札へ移動するカードについて
            var target = IdMapping.GetIdOfGameObject(idOfPlayingCard);

            var lengthOfPile = idOfPlayerPileCards.Count;
            var idOfTopOfPile = idOfPlayerPileCards[lengthOfPile - 1]; // 手札の天辺

            Vector3? startPosition = null;
            Quaternion? startRotation = null;
            Vector3? endPosition = null;
            Quaternion? endRotation = null;

            return Helper.Generate(
                startSeconds: startSeconds,
                duration: duration,
                target: target,
                getBegin: () => new PositionAndRotationLazy(
                        getPosition: () =>
                        {
                            // 初回アクセス時に、値固定
                            if (startPosition == null)
                            {
                                startPosition = GameObjectStorage.Items[target].transform.position;
                            }
                            return startPosition ?? throw new Exception();
                        },
                        getRotation: () =>
                        {
                            // 初回アクセス時に、値固定
                            if (startRotation == null)
                            {
                                startRotation = GameObjectStorage.Items[target].transform.rotation;
                            }
                            return startRotation ?? throw new Exception();
                        }),
                getEnd: () => new PositionAndRotationLazy(
                        getPosition: () =>
                        {
                            // 初回アクセス時に、値固定
                            if (endPosition == null)
                            {
                                // 現在の天辺の手札のポジションより１枚分上、または、一番下
                                // 手札が１枚も無ければ
                                if (lengthOfPile < 1)
                                {
                                    // 一番下
                                    endPosition = Vision.Commons.positionOfPileCardsOrigin[playerObj.AsInt].ToMutable();
                                }
                                // 既存の手札があれば
                                else
                                {
                                    var goCardOfTop = GameObjectStorage.Items[IdMapping.GetIdOfGameObject(idOfTopOfPile)];
                                    // より、１枚分上
                                    endPosition = goCardOfTop.transform.position;
                                }
                            }
                            return endPosition ?? throw new Exception();
                        },
                        getRotation: () =>
                        {
                            // 初回アクセス時に、値固定
                            if (endRotation == null)
                            {
                                // １プレイヤーのカードは１８０°回転
                                float angleY;
                                if (playerObj==Commons.Player1)
                                {
                                    angleY = 180.0f;
                                }
                                else if (playerObj==Commons.Player2)
                                {
                                    angleY = 0.0f;
                                }
                                else
                                {
                                    throw new Exception();
                                }

                                endRotation = Quaternion.Euler(0, angleY, 180.0f);
                            }
                            return endRotation ?? throw new Exception();
                        })
                    );
        }
    }
}
