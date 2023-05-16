namespace Assets.Scripts.Scheduler.AnalogCommands.O3rdSimplex
{
    using Assets.Scripts.ThinkingEngine.Models;
    using Assets.Scripts.Vision.Models.World;
    using System;
    using UnityEngine;
    using ModelOfAnalogCommand1stTimelineSpan = Assets.Scripts.Scheduler.AnalogCommands.O1stTimelineSpan;
    using ModelOfVisionCommons = Assets.Scripts.Vision.Commons;

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
        /// <param name="timeRange"></param>
        /// <param name="idOfCard">ピックアップしているカードのId</param>
        internal static ModelOfAnalogCommand1stTimelineSpan.IModel CreateTimespan(
            ModelOfAnalogCommand1stTimelineSpan.Range timeRange,
            IdOfPlayingCards idOfCard)
        {
            var idOfGo = IdMapping.GetIdOfGameObject(idOfCard);

            Vector3? startPosition = null;
            Quaternion? startRotation = null;
            Vector3? endPosition = null;
            Quaternion? endRotation = null;

            return new ModelOfAnalogCommand1stTimelineSpan.Model(
                timeRange: timeRange,
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

                            // 「ピックアップ」で上げた分だけ、下げる
                            endPosition = goCard.transform.position - ModelOfVisionCommons.yOfPickup.ToMutable();
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
                                y: goCard.transform.eulerAngles.y - ModelOfVisionCommons.rotationOfPickup.EulerAnglesY,
                                z: goCard.transform.eulerAngles.z - ModelOfVisionCommons.rotationOfPickup.EulerAnglesZ);
                        }
                        return endRotation ?? throw new Exception();
                    }));
        }
    }
}
