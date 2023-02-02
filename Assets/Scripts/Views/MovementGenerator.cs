namespace Assets.Scripts.Views
{
    using Assets.Scripts.Models;
    using Assets.Scripts.Models.Timeline.Spans;
    using UnityEngine;

    internal static class MovementGenerator
    {
        /// <summary>
        /// 場札を持ち上げる
        /// </summary>
        /// <param name="startSeconds">ゲーム内時間（秒）</param>
        /// <param name="duration">持続時間（秒）</param>
        /// <param name="player"></param>
        /// <param name="handIndex"></param>
        internal static CardMovementModel PickupCardOfHand(float startSeconds, float duration, GameModel gameModel, int player, int handIndex)
        {
            var idOfFocusedHandCard = gameModel.GetCardAtOfPlayerHand(player, handIndex);

            var liftY = 5.0f; // 持ち上げる（パースペクティブがかかっていて、持ち上げすぎると北へ移動したように見える）
            var rotateY = -5; // -5°傾ける
            var rotateZ = -5; // -5°傾ける
            var goCard = GameObjectStorage.PlayingCards[idOfFocusedHandCard];

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
                gameObject: goCard);
        }

        /// <summary>
        /// ピックアップしているカードを場に戻す
        /// </summary>
        /// <param name="startSeconds">ゲーム内時間（秒）</param>
        /// <param name="duration">持続時間（秒）</param>
        /// <param name="card"></param>
        internal static CardMovementModel PutDownCardOfHand(float startSeconds, float duration, GameModel gameModel, int player, int handIndex)
        {
            var idOfCard = gameModel.GetCardAtOfPlayerHand(player, handIndex);

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
                gameObject: goCard);
        }
    }
}
