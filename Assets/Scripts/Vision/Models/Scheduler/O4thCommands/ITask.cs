namespace Assets.Scripts.Vision.Models.Scheduler.O4thCommands
{
    using ModelOfThinkingEngineCommand = Assets.Scripts.ThinkingEngine.Models.Commands;

    internal interface ITask
    {
        // - プロパティ

        /// <summary>
        /// 開始時間（秒）
        /// </summary>
        float StartSeconds { get; }

        /// <summary>
        /// 終了時間（秒）
        /// </summary>
        float EndSeconds { get; }

        IModel CommandOfScheduler { get; }
    }
}
