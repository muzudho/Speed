namespace Assets.Scripts.Views
{
    using Assets.Scripts.Models;
    using Assets.Scripts.Models.Timeline;
    using UnityEngine;

    internal static class MovementGenerator
    {
        /// <summary>
        /// 場札を持ち上げる
        /// </summary>
        /// <param name="startSeconds">ゲーム内時間（秒）</param>
        /// <param name="duration">持続時間（秒）</param>
        /// <param name="idOfCard">カードId</param>
        internal static CardMovementModel PickupCardOfHand(
            float startSeconds, float duration, IdOfPlayingCards idOfCard)
        {
            var liftY = 5.0f; // 持ち上げる（パースペクティブがかかっていて、持ち上げすぎると北へ移動したように見える）
            var rotateY = -5; // -5°傾ける
            var rotateZ = -5; // -5°傾ける

            // TODO ★★ 登録時点の座標ではなく、実行時のその時点の座標を起点にしたい
            var goCard = GameObjectStorage.PlayingCards[idOfCard];

            return new CardMovementModel(
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
                idOfCard: idOfCard);
        }

        /// <summary>
        /// ピックアップしている場札を下ろす
        /// </summary>
        /// <param name="startSeconds">ゲーム内時間（秒）</param>
        /// <param name="duration">持続時間（秒）</param>
        /// <param name="idOfCard">カードId</param>
        internal static CardMovementModel PutDownCardOfHand(float startSeconds, float duration, IdOfPlayingCards idOfCard)
        {
            var liftY = 5.0f; // 持ち上げる（パースペクティブがかかっていて、持ち上げすぎると北へ移動したように見える）
            var rotateY = -5; // -5°傾ける
            var rotateZ = -5; // -5°傾ける

            // 逆をする
            liftY = -liftY;
            rotateY = -rotateY;
            rotateZ = -rotateZ;
            var goCard = GameObjectStorage.PlayingCards[idOfCard];

            return new CardMovementModel(
                startSeconds: startSeconds,
                duration: duration,
                beginPosition: goCard.transform.position,
                endPosition: new Vector3(goCard.transform.position.x, goCard.transform.position.y + liftY, goCard.transform.position.z),
                beginRotation: goCard.transform.rotation,
                endRotation: Quaternion.Euler(goCard.transform.rotation.eulerAngles.x, goCard.transform.rotation.eulerAngles.y + rotateY, goCard.transform.eulerAngles.z + rotateZ),
                idOfCard: idOfCard);
        }
    }
}
