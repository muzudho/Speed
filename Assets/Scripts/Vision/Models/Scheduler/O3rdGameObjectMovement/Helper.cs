namespace Assets.Scripts.Vision.Models.Scheduler.O3rdSpanGenerator
{
    using Assets.Scripts.Coding;
    using Assets.Scripts.Vision.Models.World;
    using ModelOfSchedulerO1stTimelineSpan = Assets.Scripts.Vision.Models.Scheduler.O1stTimelineSpan;

    static class Helper
    {
        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="startSeconds">ゲーム内時間（秒）</param>
        /// <param name="duration">持続時間（秒）</param>
        /// <param name="target">ゲーム・オブジェクトId</param>
        /// <param name="getBegin">開始時の位置と回転</param>
        /// <param name="getEnd">終了時の位置と回転</param>
        internal static ModelOfSchedulerO1stTimelineSpan.IModel GenerateSpan(
            float startSeconds,
            float duration,
            IdOfGameObjects target,
            LazyArgs.GetValue<PositionAndRotationLazy> getBegin,
            LazyArgs.GetValue<PositionAndRotationLazy> getEnd)
        {
            return new ModelOfSchedulerO1stTimelineSpan.Model(
                startSeconds: startSeconds,
                duration: duration,
                target: target,
                getBegin: getBegin,
                getEnd: getEnd);
        }
    }
}
