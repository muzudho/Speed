namespace Assets.Scripts.Vision.World.SpanOfLerp.Generator
{
    using Assets.Scripts.ThinkingEngine.Model;
    using Assets.Scripts.Vision.World.Views;
    using System;
    using UnityEngine;
    using SpanOfLeap = Assets.Scripts.Vision.World.SpanOfLerp;

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
        /// <param name="startSeconds">ゲーム内時間（秒）</param>
        /// <param name="duration">持続時間（秒）</param>
        /// <param name="getBegin"></param>
        /// <param name="idOfCard">カードId</param>
        internal static SpanOfLeap.Model Generate(
            float startSeconds,
            float duration,
            int player,
            int place,
            IdOfPlayingCards target,
            IdOfPlayingCards idOfPreviousTop)
        {
            // 台札の新しい天辺の座標
            Vector3 nextTop;
            {
                nextTop = GameView.CreatePositionOfNewCenterStackCard(
                            place: place,
                            previousTop: idOfPreviousTop);
            }

            var targetGo = IdMapping.GetIdOfGameObject(target);

            Vector3? startPosition = null;
            Quaternion? startRotation = null;
            Vector3? endPosition = null;
            Quaternion? endRotation = null;

            return new SpanOfLeap.Model(
                startSeconds: startSeconds,
                duration: duration,
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
                            endPosition = nextTop + GameView.ShakePosition(place);
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
                            var shake = GameView.ShakeRotation();
                            float yByPlayer;
                            if (player == 0) // １プレイヤーの方を 180°回転させる
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
                    }));
        }

    }
}
