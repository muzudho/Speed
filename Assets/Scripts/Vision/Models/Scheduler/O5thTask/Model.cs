namespace Assets.Scripts.Vision.Models.Scheduler.O5thTask
{
    using Assets.Scripts.Vision.Models.Scheduler.O4thCommands;
    using ModelOfThinkingEngineCommand = Assets.Scripts.ThinkingEngine.Models.Commands;
    using ModelOfSchedulerO4thCommand = Assets.Scripts.Vision.Models.Scheduler.O4thCommands;

    /// <summary>
    /// タスク
    /// 
    /// - ゲーム内時間と、時間付きコマンド引数と、ゲーム内操作　を紐づけたもの
    /// </summary>
    internal class Model : ITask
    {
        // - その他

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="startSeconds">ゲーム内時間（秒）</param>
        /// <param name="commandOfThinkingEngine">思考エンジン用のコマンド</param>
        /// <param name="commandOfScheduler">スケジューラー用のコマンド</param>
        public Model(
            float startSeconds,
            ModelOfThinkingEngineCommand.IModel commandOfThinkingEngine,
            ModelOfSchedulerO4thCommand.IModel commandOfScheduler)
        {
            this.StartSeconds = startSeconds;
            this.CommandOfThinkingEngine = commandOfThinkingEngine;
            this.CommandOfScheduler = commandOfScheduler;
        }

        // - プロパティ

        /// <summary>
        /// 開始時間（秒）
        /// </summary>
        public float StartSeconds { get; private set; }

        /// <summary>
        /// 終了時間（秒）
        /// </summary>
        public float EndSeconds => StartSeconds + CommandDurationMapping.GetDurationBy(this.CommandOfThinkingEngine.GetType());

        /// <summary>
        /// 思考エンジン用のコマンド
        /// </summary>
        public ModelOfThinkingEngineCommand.IModel CommandOfThinkingEngine { get; private set; }

        /// <summary>
        /// スケジューラー用のコマンド
        /// </summary>
        public ModelOfSchedulerO4thCommand.IModel CommandOfScheduler { get; private set; }
    }
}
