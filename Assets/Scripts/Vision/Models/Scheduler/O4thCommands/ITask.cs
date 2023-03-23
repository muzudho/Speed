namespace Assets.Scripts.Vision.Models.Scheduler.O4thCommands
{
    using ModelOfThinkingEngineCommand = Assets.Scripts.ThinkingEngine.Models.Commands;

    internal interface ITask
    {
        // - プロパティ

        /// <summary>
        /// 開始時間（秒）
        /// </summary>
        GameSeconds StartTimeObj { get; }

        /// <summary>
        /// 終了時間（秒）
        /// </summary>
        GameSeconds EndTimeObj { get; }

        IModel CommandOfScheduler { get; }
    }
}
