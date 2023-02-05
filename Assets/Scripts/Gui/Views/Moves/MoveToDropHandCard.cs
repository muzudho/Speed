namespace Assets.Scripts.Views.Moves
{
    using Assets.Scripts.Simulators;
    using Assets.Scripts.ThikningEngine;
    using Assets.Scripts.ThinkingEngine;
    using Assets.Scripts.Views.Timeline;
    using System;
    using UnityEngine;

    /// <summary>
    /// ピックアップした場札を下ろす動き
    /// </summary>
    static class MoveToDropHandCard
    {
        /// <summary>
        /// ムーブメント生成
        /// 
        /// - ピックアップしている場札を下ろす
        /// </summary>
        /// <param name="startSeconds">ゲーム内時間（秒）</param>
        /// <param name="duration">持続時間（秒）</param>
        /// <param name="idOfCard">カードId</param>
        internal static SpanToLerp Generate(
            float startSeconds,
            float duration,
            IdOfPlayingCards idOfCard)
        {
            // 逆をする
            var idOfGo = Definition.GetIdOfGameObject(idOfCard);

            Vector3? startPosition = null;
            Quaternion? startRotation = null;
            Vector3? endPosition = null;
            Quaternion? endRotation = null;

            return new SpanToLerp(
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
                            endPosition = goCard.transform.position - GameView.yOfPickup.ToMutable();
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
                                y: goCard.transform.eulerAngles.y - GameView.rotationOfPickup.EulerAnglesY,
                                z: goCard.transform.eulerAngles.z - GameView.rotationOfPickup.EulerAnglesZ);
                        }
                        return endRotation ?? throw new Exception();
                    }));
        }
    }
}
