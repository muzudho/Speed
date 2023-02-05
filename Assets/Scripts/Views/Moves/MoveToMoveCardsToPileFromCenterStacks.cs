namespace Assets.Scripts.Views.Moves
{
    using Assets.Scripts.Views.Timeline;

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
        /// <param name="getBegin">開始時の位置と回転</param>
        /// <param name="getEnd">終了時の位置と回転</param>
        /// <returns></returns>
        internal static ViewMovement Generate(
            float startSeconds,
            float duration,
            IdOfGameObjects target,
            LazyArgs.GetValue<PositionAndRotationLazy> getBegin,
            LazyArgs.GetValue<PositionAndRotationLazy> getEnd)
        {
            return new ViewMovement(
                startSeconds: startSeconds,
                duration: duration,
                target: target,
                getBegin: getBegin,
                getEnd: getEnd);
        }
    }
}
