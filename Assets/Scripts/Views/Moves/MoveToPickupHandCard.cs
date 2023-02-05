namespace Assets.Scripts.Views.Moves
{
    using Assets.Scripts.Models;
    using Assets.Scripts.Simulators.Timeline;
    using Assets.Scripts.Views.Timeline;
    using System;
    using UnityEngine;

    internal static class MoveToPickupHandCard
    {
        /// <summary>
        /// 場札を持ち上げる
        /// </summary>
        /// <param name="startSeconds">ゲーム内時間（秒）</param>
        /// <param name="duration">持続時間（秒）</param>
        /// <param name="getBegin"></param>
        /// <param name="idOfCard">カードId</param>
        internal static ViewMovement Generate(
            float startSeconds,
            float duration,
            LazyArgs.GetValue<PositionAndRotationLazy> getBegin,
            IdOfPlayingCards idOfCard)
        {
            // 持ち上げる（パースペクティブがかかっていて、持ち上げすぎると北へ移動したように見える）
            Vector3 lift = new Vector3(0.0f, 5.0f, 0.0f);

            Vector3? startPosition = null;
            Quaternion? startRotation = null;
            Vector3? endStartPosition = null;
            Quaternion? endStartRotation = null;

            return new ViewMovement(
                startSeconds: startSeconds,
                duration: duration,
                target: Specification.GetIdOfGameObject(idOfCard),
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
                            if (endStartPosition == null)
                            {
                                endStartPosition = getBegin().GetPosition();
                            }
                            return (endStartPosition ?? throw new Exception()) + lift;
                        },
                        getRotation: () =>
                        {
                            // 初回アクセス時に、値固定
                            if (endStartRotation == null)
                            {
                                endStartRotation = getBegin().GetRotation();
                            }
                            var rot = endStartRotation ?? throw new Exception();
                            var rotateY = -5; // -5°傾ける
                            var rotateZ = -5; // -5°傾ける
                            return Quaternion.Euler(
                                rot.eulerAngles.x,
                                rot.eulerAngles.y + rotateY,
                                rot.eulerAngles.z + rotateZ);
                        });
                });
        }
    }
}
