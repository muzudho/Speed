namespace Assets.Scripts.Gui.SpanOfLerp.Generator
{
    using Assets.Scripts.ThinkingEngine;
    using Assets.Scripts.Views;
    using Assets.Scripts.Views.Timeline;
    using System;
    using UnityEngine;

    /// <summary>
    /// 場札を持ち上げる
    /// </summary>
    internal static class PickupHandCard
    {
        /// <summary>
        /// ムーブメント生成
        /// </summary>
        /// <param name="startSeconds">ゲーム内時間（秒）</param>
        /// <param name="duration">持続時間（秒）</param>
        /// <param name="getBegin"></param>
        /// <param name="idOfCard">カードId</param>
        internal static SpanToLerp Generate(
            float startSeconds,
            float duration,
            LazyArgs.GetValue<PositionAndRotationLazy> getBegin,
            IdOfPlayingCards idOfCard)
        {
            // 持ち上がっている状態は、初回アクセス時に確定
            Vector3? startPosition = null;
            Quaternion? startRotation = null;
            Vector3? endPosition = null;
            Quaternion? endRotation = null;

            return new SpanToLerp(
                startSeconds: startSeconds,
                duration: duration,
                target: Definition.GetIdOfGameObject(idOfCard),
                getBegin: () =>
                {
                    return new PositionAndRotationLazy(
                        getPosition: () =>
                        {
                            // 初回アクセス時に、値固定
                            if (startPosition == null)
                            {
                                startPosition = getBegin().GetPosition();
                            }
                            return startPosition ?? throw new Exception();
                        },
                        getRotation: () =>
                        {
                            // 初回アクセス時に、値固定
                            if (startRotation == null)
                            {
                                startRotation = getBegin().GetRotation();
                            }
                            return startRotation ?? throw new Exception();
                        });
                },
                getEnd: () =>
                {
                    return new PositionAndRotationLazy(
                        getPosition: () =>
                        {
                            // 初回アクセス時に、値固定
                            if (endPosition == null)
                            {
                                endPosition = getBegin().GetPosition() + GameView.yOfPickup.ToMutable();
                            }
                            return endPosition ?? throw new Exception();
                        },
                        getRotation: () =>
                        {
                            // 初回アクセス時に、値固定
                            if (endRotation == null)
                            {
                                var rot = getBegin().GetRotation();

                                endRotation = Quaternion.Euler(
                                    rot.eulerAngles.x,
                                    rot.eulerAngles.y + GameView.rotationOfPickup.EulerAnglesY,
                                    rot.eulerAngles.z + GameView.rotationOfPickup.EulerAnglesZ);
                            }

                            return endRotation ?? throw new Exception();
                        });
                });
        }
    }
}
