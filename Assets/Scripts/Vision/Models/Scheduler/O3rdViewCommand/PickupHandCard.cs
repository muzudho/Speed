﻿namespace Assets.Scripts.Vision.Models.Scheduler.O3rdViewCommand
{
    using Assets.Scripts.Coding;
    using Assets.Scripts.ThinkingEngine.Models;
    using Assets.Scripts.Vision.Models.World;
    using System;
    using UnityEngine;
    using ModelOfSchedulerO1stTimelineSpan = Assets.Scripts.Vision.Models.Scheduler.O1stTimelineSpan;

    /// <summary>
    /// 場札を持ち上げる
    /// 
    /// - 場札のホーム・ポジション + 持ち上げる高さ
    /// - 場札のホーム・ローテーション + 一定の傾き
    /// </summary>
    internal static class PickupHandCard
    {
        /// <summary>
        /// 前もって算出
        /// </summary>
        /// <param name="homePositionOfHand"></param>
        /// <param name="homeRotationOfHand"></param>
        /// <returns></returns>
        internal static PositionAndRotation CalculateEnd(
            Vector3 homePositionOfHand,
            Quaternion homeRotationOfHand)
        {
            return new PositionAndRotation(
                position: homePositionOfHand + Commons.yOfPickup.ToMutable(),
                rotation: Quaternion.Euler(
                    homeRotationOfHand.eulerAngles.x,
                    homeRotationOfHand.eulerAngles.y + Commons.rotationOfPickup.EulerAnglesY,
                    homeRotationOfHand.eulerAngles.z + Commons.rotationOfPickup.EulerAnglesZ));
        }

        /// <summary>
        /// ムーブメント生成
        /// </summary>
        /// <param name="timeRange"></param>
        /// <param name="getBegin"></param>
        /// <param name="idOfCard">カードId</param>
        internal static ModelOfSchedulerO1stTimelineSpan.IModel GenerateSpan(
            ModelOfSchedulerO1stTimelineSpan.Range timeRange,
            LazyArgs.GetValue<PositionAndRotationLazy> getBegin,
            IdOfPlayingCards idOfCard)
        {
            // 持ち上がっている状態は、初回アクセス時に確定
            Vector3? startPosition = null;
            Quaternion? startRotation = null;
            Vector3? endPosition = null;
            Quaternion? endRotation = null;

            return new ModelOfSchedulerO1stTimelineSpan.Model(
                timeRange: timeRange,
                target: IdMapping.GetIdOfGameObject(idOfCard),
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
                                endPosition = getBegin().GetPosition() + Commons.yOfPickup.ToMutable();
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
                                    rot.eulerAngles.y + Commons.rotationOfPickup.EulerAnglesY,
                                    rot.eulerAngles.z + Commons.rotationOfPickup.EulerAnglesZ);
                            }

                            return endRotation ?? throw new Exception();
                        });
                });
        }
    }
}