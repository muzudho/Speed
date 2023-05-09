namespace Assets.Scripts.Vision.Models.Scheduler.O3rdSimplexCommands
{
    using Assets.Scripts.Coding;
    using Assets.Scripts.ThinkingEngine.Models;
    using Assets.Scripts.Vision.Models.World;
    using System;
    using UnityEngine;
    using ModelOfSchedulerO1stTimelineSpan = Assets.Scripts.Vision.Models.Scheduler.O1stTimelineSpan;

    /// <summary>
    /// 指定のカードを、台札の上へ置く
    /// 
    /// - ピックアップしているカード用
    /// </summary>
    internal static class PutCardToCenterStack
    {
        /// <summary>
        /// ムーブメント生成
        /// </summary>
        /// <param name="timeRange"></param>
        /// <param name="getBegin"></param>
        /// <param name="idOfCard">カードId</param>
        internal static ModelOfSchedulerO1stTimelineSpan.IModel GenerateSpan(
            ModelOfSchedulerO1stTimelineSpan.Range timeRange,
            Player playerObj,
            CenterStackPlace placeObj,
            IdOfPlayingCards target,
            IdOfPlayingCards idOfPreviousTop,
            LazyArgs.SetValue<float> onProgressOrNull)
        {
            // 台札の新しい天辺の座標
            Vector3 nextTop;
            {
                nextTop = Commons.CreatePositionOfNewCenterStackCard(
                            placeObj: placeObj,
                            previousTop: idOfPreviousTop);
            }

            var targetGo = IdMapping.GetIdOfGameObject(target);

            Vector3? startPosition = null;
            Quaternion? startRotation = null;
            Vector3? endPosition = null;
            Quaternion? endRotation = null;

            return new ModelOfSchedulerO1stTimelineSpan.Model(
                timeRange: timeRange,
                target: targetGo,
                getBegin: () => new PositionAndRotationLazy(
                    getPosition: () =>
                    {
                        // 初回アクセス時に、値固定
                        if (startPosition == null)
                        {
                            startPosition = GameObjectStorage.Items[targetGo].transform.position; // 抜いた場札
                        }
                        return startPosition ?? throw new Exception();
                    },
                    getRotation: () =>
                    {
                        // 初回アクセス時に、値固定
                        if (startRotation == null)
                        {
                            startRotation = GameObjectStorage.Items[targetGo].transform.rotation; // 抜いた場札
                        }
                        return startRotation ?? throw new Exception();
                    }),
                getEnd: () => new PositionAndRotationLazy(
                    getPosition: () =>
                    {
                        // 初回アクセス時に、値固定
                        if (endPosition == null)
                        {
                            endPosition = nextTop + Commons.ShakePosition(placeObj);
                        }
                        return endPosition ?? throw new Exception();
                    },
                    getRotation: () =>
                    {
                        // 初回アクセス時に、値固定
                        if (endRotation == null)
                        {
                            // １プレイヤー、２プレイヤーでカードの向きが違う
                            // また、元の捻りを保存していないと、補間で大回転してしまうようだ

                            var src = GameObjectStorage.Items[targetGo].transform.rotation; // 抜いた場札
                            var shake = Commons.ShakeRotation();
                            float yByPlayer;
                            if (playerObj.AsInt == 0) // １プレイヤーの方を 180°回転させる
                            {
                                yByPlayer = 180.0f;
                            }
                            else
                            {
                                yByPlayer = 0.0f;
                            }

                            endRotation = Quaternion.Euler(
                                x: src.x + shake.x,
                                y: src.y + shake.y + yByPlayer,
                                z: src.z + shake.z);
                        }
                        return endRotation ?? throw new Exception();
                    }),
                onProgressOrNull: onProgressOrNull);
        }

    }
}
