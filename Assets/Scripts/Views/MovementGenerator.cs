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
        /// <param name="idOfCard">カードId</param>
        internal static MovementViewModel PickupCardOfHand(
            float startSeconds, float duration, IdOfPlayingCards idOfCard)
        {
            var liftY = 5.0f; // 持ち上げる（パースペクティブがかかっていて、持ち上げすぎると北へ移動したように見える）
            var rotateY = -5; // -5°傾ける
            var rotateZ = -5; // -5°傾ける

            // TODO ★★ 登録時点の座標ではなく、実行時のその時点の座標を起点にしたい
            var idOfGo = Specification.GetIdOfGameObject(idOfCard);
            var goCard = GameObjectStorage.Items[idOfGo];

            return new MovementViewModel(
                startSeconds: startSeconds,
                duration: duration,
                beginPosition: goCard.transform.position,
                endPosition: new Vector3(
                    goCard.transform.position.x,
                    goCard.transform.position.y + liftY,
                    goCard.transform.position.z),
                beginRotation: goCard.transform.rotation,
                endRotation: Quaternion.Euler(
                    goCard.transform.rotation.eulerAngles.x,
                    goCard.transform.rotation.eulerAngles.y + rotateY,
                    goCard.transform.eulerAngles.z + rotateZ),
                idOfGameObject: idOfGo);
        }

        /// <summary>
        /// ピックアップしている場札を下ろす
        /// </summary>
        /// <param name="startSeconds">ゲーム内時間（秒）</param>
        /// <param name="duration">持続時間（秒）</param>
        /// <param name="idOfCard">カードId</param>
        internal static MovementViewModel PutDownCardOfHand(float startSeconds, float duration, IdOfPlayingCards idOfCard)
        {
            var liftY = 5.0f; // 持ち上げる（パースペクティブがかかっていて、持ち上げすぎると北へ移動したように見える）
            var rotateY = -5; // -5°傾ける
            var rotateZ = -5; // -5°傾ける

            // 逆をする
            liftY = -liftY;
            rotateY = -rotateY;
            rotateZ = -rotateZ;
            var idOfGo = Specification.GetIdOfGameObject(idOfCard);
            var goCard = GameObjectStorage.Items[idOfGo];

            return new MovementViewModel(
                startSeconds: startSeconds,
                duration: duration,
                beginPosition: goCard.transform.position,
                endPosition: new Vector3(goCard.transform.position.x, goCard.transform.position.y + liftY, goCard.transform.position.z),
                beginRotation: goCard.transform.rotation,
                endRotation: Quaternion.Euler(goCard.transform.rotation.eulerAngles.x, goCard.transform.rotation.eulerAngles.y + rotateY, goCard.transform.eulerAngles.z + rotateZ),
                idOfGameObject: idOfGo);
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
        /// <param name="duration1"></param>
        /// <param name="duration2"></param>
        /// <param name="gameModel"></param>
        /// <param name="player"></param>
        /// <param name="numberOfHandCards">場札の枚数</param>
        /// <param name="indexOfPickup">ピックアップしている場札は何番目</param>
        /// <param name="idOfHands">場札のIdリスト</param>
        /// <param name="handCardMinY">場札の最低Y座標</param>
        /// <param name="handCardsOriginZ">場札の基準Z座標</param>
        /// <param name="setCardMovementModel"></param>
        /// <exception cref="Exception"></exception>
        internal static void ArrangeHandCards(
            float startSeconds,
            float duration1,
            float duration2,
            int player,
            int numberOfHandCards,
            int indexOfPickup,
            List<IdOfPlayingCards> idOfHands,
            float handCardMinY,
            float handCardsOriginZ,
            LazyArgs.SetValue<MovementViewModel> setCardMovementModel)
        {
            // 最大25枚の場札が並べるように調整してある

            float cardAngleZ = -5; // カードの少しの傾き

            int range = 200; // 半径。大きな円にするので、中心を遠くに離したい
            int offsetCircleCenterZ; // 中心位置の調整

            float angleY;
            float playerTheta;
            float angleStep = -1.83f;
            float startTheta = (numberOfHandCards * Mathf.Abs(angleStep) / 2 - Mathf.Abs(angleStep) / 2 + 90.0f) * Mathf.Deg2Rad;
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

            float theta = startTheta;
            foreach (var idOfCard in idOfHands) // 場札のIdリスト
            {
                float x = range * Mathf.Cos(theta + playerTheta) + ox;
                float z = range * Mathf.Sin(theta + playerTheta) + handCardsOriginZ + offsetCircleCenterZ;

                var idOfGo = Specification.GetIdOfGameObject(idOfCard);
                var goCard = GameObjectStorage.Items[idOfGo];
                setCardMovementModel(new MovementViewModel(
                    startSeconds: startSeconds,
                    duration: duration1,
                    beginPosition: goCard.transform.position,
                    endPosition: new Vector3(x, handCardMinY, z),
                    beginRotation: goCard.transform.rotation,
                    endRotation: Quaternion.Euler(0, angleY, cardAngleZ),
                    idOfGameObject: idOfGo));


                // 更新
                theta += thetaStep;
            }

            // 場札を並べなおすと、持ち上げていたカードを下ろしてしまうので、再度、持ち上げる
            {
                Debug.Log($"[ArrangeHandCards] 再度持上げ handIndex:{indexOfPickup}");

                if (0 <= indexOfPickup && indexOfPickup < numberOfHandCards) // 範囲内なら
                {
                    var idOfPickuppedCard = idOfHands[indexOfPickup]; // ピックアップしている場札

                    // 抜いたカードの右隣のカードを（有れば）ピックアップする
                    setCardMovementModel(MovementGenerator.PickupCardOfHand(
                        startSeconds: startSeconds + duration1,
                        duration: duration2,
                        idOfCard: idOfPickuppedCard));
                }
            }
        }
    }
}
