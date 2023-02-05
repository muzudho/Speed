namespace Assets.Scripts.Views.Moves
{
    using Assets.Scripts.Views.Timeline;
    using System;
    using UnityEngine;

    /// <summary>
    /// 台札から手札へのカードの移動
    /// </summary>
    internal static class MoveToMoveCardsToPileFromCenterStacks
    {
        /// <summary>
        /// ムーブメント生成
        /// </summary>
        /// <param name="startSeconds">ゲーム内時間（秒）</param>
        /// <param name="duration">持続時間（秒）</param>
        /// <param name="target">ゲーム・オブジェクトId</param>
        /// <param name="getEnd">終了時の位置と回転</param>
        /// <returns></returns>
        internal static ViewMovement Generate(
            float startSeconds,
            float duration,
            IdOfGameObjects target,
            LazyArgs.GetValue<PositionAndRotationLazy> getEnd)
        {
            Vector3? startPosition = null;
            Quaternion? startRotation = null;

            return new ViewMovement(
                startSeconds: startSeconds,
                duration: duration,
                target: target,
                getBegin: () => new PositionAndRotationLazy(
                        getPosition: () =>
                        {
                            // 初回アクセス時に、値固定
                            if (startPosition == null)
                            {
                                startPosition = GameObjectStorage.Items[target].transform.position;
                            }
                            return startPosition ?? throw new Exception();
                        },
                        getRotation: () =>
                        {
                            // 初回アクセス時に、値固定
                            if (startRotation == null)
                            {
                                startRotation = GameObjectStorage.Items[target].transform.rotation;
                            }
                            return startRotation ?? throw new Exception();
                        }),
                getEnd: getEnd);
        }
    }
}
