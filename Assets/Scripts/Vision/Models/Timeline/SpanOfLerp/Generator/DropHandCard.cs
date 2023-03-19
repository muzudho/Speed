namespace Assets.Scripts.Vision.Models.Timeline.SpanOfLerp.Generator
{
    using Assets.Scripts.ThinkingEngine.Models;
    using Assets.Scripts.Vision.Models.World;
    using System;
    using UnityEngine;
    using VisionOfTimelineO4thElement = Assets.Scripts.Vision.Models.Timeline.O4thElement;

    /// <summary>
    /// 指定のカードを下ろす
    /// 
    /// - ピックアップしている場札用
    /// </summary>
    static class DropHandCard
    {
        /// <summary>
        /// ムーブメント生成
        /// </summary>
        /// <param name="startSeconds">ゲーム内時間（秒）</param>
        /// <param name="duration">持続時間（秒）</param>
        /// <param name="idOfCard">ピックアップしているカードのId</param>
        internal static VisionOfTimelineO4thElement.Model Generate(
            float startSeconds,
            float duration,
            IdOfPlayingCards idOfCard)
        {
            var idOfGo = IdMapping.GetIdOfGameObject(idOfCard);

            Vector3? startPosition = null;
            Quaternion? startRotation = null;
            Vector3? endPosition = null;
            Quaternion? endRotation = null;

            return new VisionOfTimelineO4thElement.Model(
                startSeconds: startSeconds,
                duration: duration,
                target: idOfGo,
                getBegin: () => new PositionAndRotationLazy(
                    getPosition: () =>
                    {
                        // 初回アクセス時に、値固定
                        if (startPosition == null)
                        {
                            startPosition = GameObjectStorage.Items[idOfGo].transform.position;
                        }
                        return startPosition ?? throw new Exception();
                    },
                    getRotation: () =>
                    {
                        // 初回アクセス時に、値固定
                        if (startRotation == null)
                        {
                            startRotation = GameObjectStorage.Items[idOfGo].transform.rotation;
                        }
                        return startRotation ?? throw new Exception();
                    }),
                getEnd: () => new PositionAndRotationLazy(
                    getPosition: () =>
                    {
                        // 初回アクセス時に、値固定
                        if (endPosition == null)
                        {
                            var goCard = GameObjectStorage.Items[idOfGo];
                            endPosition = goCard.transform.position - Commons.yOfPickup.ToMutable();
                        }
                        return endPosition ?? throw new Exception();
                    },
                    getRotation: () =>
                    {
                        // 初回アクセス時に、値固定
                        if (endRotation == null)
                        {
                            var goCard = GameObjectStorage.Items[idOfGo];
                            endRotation = Quaternion.Euler(
                                x: goCard.transform.eulerAngles.x,
                                y: goCard.transform.eulerAngles.y - Commons.rotationOfPickup.EulerAnglesY,
                                z: goCard.transform.eulerAngles.z - Commons.rotationOfPickup.EulerAnglesZ);
                        }
                        return endRotation ?? throw new Exception();
                    }));
        }
    }
}
