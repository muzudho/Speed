namespace Assets.Scripts.Vision.Models.Scheduler.O4thCommands
{
    using ModelOfSchedulerO1stTimelineSpan = Assets.Scripts.Vision.Models.Scheduler.O1stTimelineSpan;

    internal interface ITask
    {
        // - プロパティ

        /// <summary>
        /// ゲーム時間範囲（単位：秒）
        /// </summary>
        ModelOfSchedulerO1stTimelineSpan.Range TimeRangeObj { get; }

        IModel CommandOfScheduler { get; }
    }
}
