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

        /// <summary>
        /// 場札を並べる
        /// 
        /// - ２段階モーション
        ///     - １段階目：全ての場札を、少し扇状にカーブさせて整列させる
        ///     - ２段階目：ピックアップしていた場札を下ろしてしまっているので、ピックアップし直す
        /// - 左端は角度で言うと 112.0f
        /// </summary>
        /// <param name="startSeconds"></param>
        /// <param name="duration"></param>
        /// <param name="gameModel"></param>
        /// <param name="player"></param>
        /// <param name="indexOfPickup">ピックアップしている場札は何番目</param>
        /// <param name="idOfHandCards">場札のIdリスト</param>
        /// <param name="setViewMovement"></param>
        /// <exception cref="Exception"></exception>
        internal static void ArrangeHandCards(
            float startSeconds,
            float duration,
            int player,
            int indexOfPickup,
            List<IdOfPlayingCards> idOfHandCards,
            LazyArgs.SetValue<ViewMovement> setViewMovement)
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

            switch (player)
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
            Debug.Log($"[ArrangeHandCards] 再度持上げ handIndex:{indexOfPickup}");
            if (0 <= indexOfPickup && indexOfPickup < idOfHandCards.Count) // 範囲内なら
            {
                idOfPickupCard = idOfHandCards[indexOfPickup];
            }

            float theta = startTheta;
            int i = 0;
            foreach (var idOfHandCard in idOfHandCards) // 場札のIdリスト
            {
                float x = range * Mathf.Cos(theta + playerTheta) + ox;
                float z = range * Mathf.Sin(theta + playerTheta) + GameView.positionOfHandCardsOrigin[player].Z + offsetCircleCenterZ;

                var idOfGo = Specification.GetIdOfGameObject(idOfHandCard);

                // 目標地点
                var staticDestination = new PositionAndRotationLazy(
                    getPosition: () => new Vector3(x, GameView.positionOfHandCardsOrigin[player].Y, z),
                    getRotation: () => Quaternion.Euler(0, angleY, cardAngleZ));

                if (idOfHandCard != idOfPickupCard)
                {
                    Vector3? startPosition = null;
                    Quaternion? startRotation = null;

                    setViewMovement(new ViewMovement(
                        startSeconds: startSeconds,
                        duration: duration,
                        target: idOfGo,
                        getBegin: () =>
                        {
                            var goCard = GameObjectStorage.Items[idOfGo];
                            return new PositionAndRotationLazy(
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
                                });
                        },
                        getEnd: () => staticDestination));
                }
                else
                {
                    // ピックアップしている場札

                    // 目標地点　＋　ピックアップ操作
                    setViewMovement(MovementGenerator.PickupCardOfHand(
                        startSeconds: startSeconds,
                        duration: duration,
                        idOfCard: idOfPickupCard,
                        getBegin: () => staticDestination));
                }

                // 更新
                theta += thetaStep;
                i++;
            }
        }
    }
}
