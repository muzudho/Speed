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
        /// <param name="startTimeObj">ゲーム内時間（秒）</param>
        /// <param name="commandOfScheduler">スケジューラー用のコマンド</param>
        public Model(
            GameSeconds startTimeObj,
            ModelOfSchedulerO4thCommand.IModel commandOfScheduler)
        {
            this.StartTimeObj = startTimeObj;
            this.CommandOfScheduler = commandOfScheduler;
        }

        // - プロパティ

        /// <summary>
        /// 開始時間（秒）
        /// 
        /// TODO ★ Start,End,Durationは、 TimeRangeクラスを作って分けたい
        /// </summary>
        public GameSeconds StartTimeObj { get; private set; }

        /// <summary>
        /// 終了時間（秒）
        /// </summary>
        public GameSeconds EndTimeObj => new GameSeconds(StartTimeObj.AsFloat + CommandDurationMapping.GetDurationBy(this.CommandOfScheduler.CommandOfThinkingEngine.GetType()).AsFloat);

        /// <summary>
        /// スケジューラー用のコマンド
        /// </summary>
        public ModelOfSchedulerO4thCommand.IModel CommandOfScheduler { get; private set; }
    }
}
