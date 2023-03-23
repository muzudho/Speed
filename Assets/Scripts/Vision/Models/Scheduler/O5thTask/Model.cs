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
            ModelOfSchedulerO4thCommand.IModel commandOfScheduler)
        {
            this.CommandOfScheduler = commandOfScheduler;
        }

        // - プロパティ

        /// <summary>
        /// スケジューラー用のコマンド
        /// </summary>
        public ModelOfSchedulerO4thCommand.IModel CommandOfScheduler { get; private set; }
    }
}
