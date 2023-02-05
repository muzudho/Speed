namespace Assets.Scripts.Views
{
    using Assets.Scripts.Models;
    using Assets.Scripts.Simulators.Timeline;
    using Assets.Scripts.Views.Timeline;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    internal static class MovementGenerator
    {
        /// <summary>
        /// 場札を持ち上げる
        /// </summary>
        /// <param name="startSeconds">ゲーム内時間（秒）</param>
        /// <param name="duration">持続時間（秒）</param>
        /// <param name="getBegin"></param>
        /// <param name="idOfCard">カードId</param>
        internal static ViewMovement PickupCardOfHand(
            float startSeconds,
            float duration,
            LazyArgs.GetValue<PositionAndRotationLazy> getBegin,
            IdOfPlayingCards idOfCard)
        {
            // 持ち上げる（パースペクティブがかかっていて、持ち上げすぎると北へ移動したように見える）
            Vector3 lift = new Vector3(0.0f, 5.0f, 0.0f);

            Vector3? startPosition = null;
            Quaternion? startRotation = null;

            return new ViewMovement(
                startSeconds: startSeconds,
                duration: duration,
                target: Specification.GetIdOfGameObject(idOfCard),
                getBegin: getBegin,
                getEnd: () =>
                {
                    return new PositionAndRotationLazy(
                                getPosition: () =>
                                {
                                    // 初回アクセス時に、値固定
                                    if (startPosition == null)
                                    {
                                        startPosition = getBegin().GetPosition();
                                    }
                                    return (startPosition ?? throw new Exception()) + lift;
                                },
                                getRotation: () =>
                                {
                                    // 初回アクセス時に、値固定
                                    if (startRotation == null)
                                    {
                                        startRotation = getBegin().GetRotation();
                                    }
                                    var rot = startRotation ?? throw new Exception();
                                    var rotateY = -5; // -5°傾ける
                                    var rotateZ = -5; // -5°傾ける
                                    return Quaternion.Euler(
                                        rot.eulerAngles.x,
                                        rot.eulerAngles.y + rotateY,
                                        rot.eulerAngles.z + rotateZ);
                                });
                });
        }

        /// <summary>
        /// ピックアップしている場札を下ろす
        /// </summary>
        /// <param name="startSeconds">ゲーム内時間（秒）</param>
        /// <param name="duration">持続時間（秒）</param>
        /// <param name="idOfCard">カードId</param>
        internal static ViewMovement PutDownCardOfHand(
            float startSeconds,
            float duration,
            IdOfPlayingCards idOfCard)
        {
            var liftY = 5.0f; // 持ち上げる（パースペクティブがかかっていて、持ち上げすぎると北へ移動したように見える）
            var rotateY = -5; // -5°傾ける
            var rotateZ = -5; // -5°傾ける

            // 逆をする
            liftY = -liftY;
            rotateY = -rotateY;
            rotateZ = -rotateZ;
            var idOfGo = Specification.GetIdOfGameObject(idOfCard);
            var goCard = GameObjectStorage.Items[idOfGo]; // TODO ★ 各カードの座標は、ゲーム・オブジェクトから取得するのではなく、シミュレーターで保持しておきたい。シンクロしたくない

            Vector3? startPosition = null;
            Quaternion? startRotation = null;
            Vector3? endPosition = null;
            Quaternion? endRotation = null;

            return new ViewMovement(
                startSeconds: startSeconds,
                duration: duration,
                target: idOfGo,
                getBegin: () => new PositionAndRotationLazy(
                                getPosition: () =>
                                {
                                    // 初回アクセス時に、値固定
                                    if (startPosition == null)
                                    {
                                        startPosition = goCard.transform.position;
                                    }
                                    return startPosition ?? throw new Exception();
                                },
                                getRotation: () =>
                                {
                                    // 初回アクセス時に、値固定
                                    if (startRotation == null)
                                    {
                                        startRotation = goCard.transform.rotation;
                                    }
                                    return startRotation ?? throw new Exception();
                                }),
                getEnd: () => new PositionAndRotationLazy(
                                getPosition: () =>
                                {
                                    // 初回アクセス時に、値固定
                                    if (endPosition == null)
                                    {
                                        endPosition = new Vector3(goCard.transform.position.x, goCard.transform.position.y + liftY, goCard.transform.position.z);
                                    }
                                    return endPosition ?? throw new Exception();
                                },
                                getRotation: () =>
                                {
                                    // 初回アクセス時に、値固定
                                    if (endRotation == null)
                                    {
                                        endRotation = Quaternion.Euler(goCard.transform.rotation.eulerAngles.x, goCard.transform.rotation.eulerAngles.y + rotateY, goCard.transform.eulerAngles.z + rotateZ);
                                    }
                                    return endRotation ?? throw new Exception();
                                }));
        }
    }
}
