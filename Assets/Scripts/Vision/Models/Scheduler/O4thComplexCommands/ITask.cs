namespace Assets.Scripts.Vision.Models.Scheduler.O4thComplexCommands
{
    using ModelOfSchedulerO1stTimelineSpan = Assets.Scripts.Vision.Models.Scheduler.O1stTimelineSpan;

    internal interface ITask
    {
        // - プロパティ

        IModel CommandOfScheduler { get; }
    }
}
