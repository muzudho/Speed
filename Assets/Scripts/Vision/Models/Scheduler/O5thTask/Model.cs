namespace Assets.Scripts.Vision.Models.Scheduler.O5thTask
{
    using Assets.Scripts.Vision.Models.Scheduler.O4thCommands;
    using ModelOfSchedulerO1stTimelineSpan = Assets.Scripts.Vision.Models.Scheduler.O1stTimelineSpan;
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
            this.TimeRangeObj = new ModelOfSchedulerO1stTimelineSpan.Range(startTimeObj, CommandDurationMapping.GetDurationBy(commandOfScheduler.CommandOfThinkingEngine.GetType()));
            this.CommandOfScheduler = commandOfScheduler;
        }

        // - プロパティ

        /// <summary>
        /// ゲーム時間範囲（単位：秒）
        /// </summary>
        public ModelOfSchedulerO1stTimelineSpan.Range TimeRangeObj { get; private set; }

        /// <summary>
        /// スケジューラー用のコマンド
        /// </summary>
        public ModelOfSchedulerO4thCommand.IModel CommandOfScheduler { get; private set; }
    }
}
