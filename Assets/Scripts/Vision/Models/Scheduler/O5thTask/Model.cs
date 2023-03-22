namespace Assets.Scripts.Vision.Models.Scheduler.O5thTask
{
    using Assets.Scripts.Vision.Models.Scheduler.O4thGameOperation;
    using ModelOfSchedulerO2ndTimedCommandArgs = Assets.Scripts.Vision.Models.Scheduler.O2ndTimedCommandArgs;
    using ModelOfSchedulerO4thGameOperation = Assets.Scripts.Vision.Models.Scheduler.O4thGameOperation;

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
        /// <param name="args">スパン・モデル</param>
        /// <param name="gameOperation"></param>
        public Model(
            float startSeconds,
            ModelOfSchedulerO2ndTimedCommandArgs.Model args,
            ModelOfSchedulerO4thGameOperation.IModel gameOperation)
        {
            this.StartSeconds = startSeconds;
            this.Args = args;
            this.GameOperation = gameOperation;
        }

        // - プロパティ

        /// <summary>
        /// 開始時間（秒）
        /// </summary>
        public float StartSeconds { get; private set; }

        /// <summary>
        /// 終了時間（秒）
        /// </summary>
        public float EndSeconds => StartSeconds + this.Args.Duration;

        public ModelOfSchedulerO2ndTimedCommandArgs.Model Args { get; private set; }

        public ModelOfSchedulerO4thGameOperation.IModel GameOperation { get; private set; }
    }
}
