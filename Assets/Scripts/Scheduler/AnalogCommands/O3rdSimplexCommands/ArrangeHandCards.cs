namespace Assets.Scripts.Scheduler.AnalogCommands.O3rdSimplexCommands
{
    using Assets.Scripts.Coding;
    using Assets.Scripts.ThinkingEngine.Models;
    using Assets.Scripts.Vision.Models.World;
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using ModelOfSchedulerO1stTimelineSpan = Assets.Scripts.Scheduler.AnalogCommands.O1stTimelineSpan;
    using ModelOfSchedulerO3rdSimplexCommand = Assets.Scripts.Scheduler.AnalogCommands.O3rdSimplexCommands;

    /// <summary>
    /// 場札を並べる
    /// 
    /// - ２段階モーション
    ///     - １段階目：全ての場札を、少し扇状にカーブさせて整列させる
    ///     - ２段階目：ピックアップしていた場札を下ろしてしまっているので、ピックアップし直す
    /// - 左端は角度で言うと 112.0f
    /// </summary>
    static class ArrangeHandCards
    {
        /// <summary>
        /// ムーブメント生成
        /// </summary>
        /// <param name="timeRange"></param>
        /// <param name="playerObj"></param>
        /// <param name="indexOfPickupObj">ピックアップしている場札は何番目</param>
        /// <param name="idOfHandCards">場札のIdリスト</param>
        /// <param name="setTimespan"></param>
        /// <exception cref="Exception"></exception>
        internal static void GenerateSpan(
            ModelOfSchedulerO1stTimelineSpan.Range timeRange,
            Player playerObj,
            HandCardIndex indexOfPickupObj,
            List<IdOfPlayingCards> idOfHandCards,
            bool keepPickup,
            LazyArgs.SetValue<ModelOfSchedulerO1stTimelineSpan.IModel> setTimespan,
            LazyArgs.SetValue<float> onProgressOrNull)
        {
            // 最大25枚の場札が並べるように調整してある

            float cardAngleZ = -5; // カードの少しの傾き

            int range = 200; // 半径。大きな円にするので、中心を遠くに離したい
            int offsetCircleCenterZ; // 中心位置の調整

            float angleY;
            float playerTheta;
            float angleStep = -1.83f;
            float startTheta = (idOfHandCards.Count * Mathf.Abs(angleStep) / 2 - Mathf.Abs(angleStep) / 2 + 90.0f) * Mathf.Deg2Rad;
            float thetaStep = angleStep * Mathf.Deg2Rad; ; // 時計回り

            float ox = 0.0f;

            switch (playerObj.AsInt)
            {
                case 0:
                    // １プレイヤー
                    angleY = 180.0f;
                    playerTheta = 0;
                    offsetCircleCenterZ = -190;
                    break;

                case 1:
                    // ２プレイヤー
                    angleY = 0.0f;
                    playerTheta = 180 * Mathf.Deg2Rad;
                    offsetCircleCenterZ = 188;  // カメラのパースペクティブが付いているから、目視で調整
                    break;

                default:
                    throw new Exception();
            }

            // 場札を並べなおすと、持ち上げていたカードを下ろしてしまうので、再度、持ち上げる
            IdOfPlayingCards idOfPickupCard = IdOfPlayingCards.None;    // ピックアップしている場札
            // Debug.Log($"[ArrangeHandCards] 再度持上げ handIndex:{indexOfPickup}");
            if (HandCardIndex.First <= indexOfPickupObj && indexOfPickupObj.AsInt < idOfHandCards.Count) // 範囲内なら
            {
                idOfPickupCard = idOfHandCards[indexOfPickupObj.AsInt];
            }

            float theta = startTheta;
            int i = 0;
            foreach (var idOfHandCard in idOfHandCards) // 場札のIdリスト
            {
                float x = range * Mathf.Cos(theta + playerTheta) + ox;
                float z = range * Mathf.Sin(theta + playerTheta) + Vision.Commons.positionOfHandCardsOrigin[playerObj.AsInt].Z + offsetCircleCenterZ;

                var idOfGo = IdMapping.GetIdOfGameObject(idOfHandCard);

                // 目標地点
                var staticDestination = new PositionAndRotationLazy(
                    getPosition: () => new Vector3(x, Vision.Commons.positionOfHandCardsOrigin[playerObj.AsInt].Y, z),
                    getRotation: () => Quaternion.Euler(0, angleY, cardAngleZ));

                if (keepPickup && idOfHandCard == idOfPickupCard)
                {
                    // ピックアップしている場札をキープ

                    Vector3? startPosition = null;
                    Quaternion? startRotation = null;

                    // ピックアップ後の座標を計算したい。ピックアップ前の座標は指定する
                    var endPositionAndRotation = ModelOfSchedulerO3rdSimplexCommand.PickupHandCard.CalculateEnd(
                        homePositionOfHand: staticDestination.GetPosition(),
                        homeRotationOfHand: staticDestination.GetRotation());

                    setTimespan(new ModelOfSchedulerO1stTimelineSpan.Model(
                        timeRange: timeRange,
                        target: idOfGo,
                        getBegin: () =>
                        {
                            return new PositionAndRotationLazy(
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
                                });
                        },
                        getEnd: () => new PositionAndRotationLazy(
                            getPosition: () => endPositionAndRotation.Position,
                            getRotation: () => endPositionAndRotation.Rotation
                            ),
                        onProgressOrNull: onProgressOrNull));
                }
                else
                {
                    Vector3? startPosition = null;
                    Quaternion? startRotation = null;

                    setTimespan(new ModelOfSchedulerO1stTimelineSpan.Model(
                        timeRange: timeRange,
                        target: idOfGo,
                        getBegin: () =>
                        {
                            return new PositionAndRotationLazy(
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
                                });
                        },
                        getEnd: () => staticDestination,
                        onProgressOrNull: onProgressOrNull));
                }

                // 更新
                theta += thetaStep;
                i++;
            }
        }
    }
}
