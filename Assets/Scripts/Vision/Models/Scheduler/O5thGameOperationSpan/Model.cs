namespace Assets.Scripts.Vision.Models.Scheduler.O5thGameOperationSpan
{
    using Assets.Scripts.Vision.Models.Scheduler.O4thGameOperation;
    using ModelOfSchedulerO2ndTimedCommandArgs = Assets.Scripts.Vision.Models.Scheduler.O2ndTimedCommandArgs;
    using ModelOfSchedulerO4thGameOperation = Assets.Scripts.Vision.Models.Scheduler.O4thGameOperation;

    /// <summary>
    /// ゲーム内時間と、時間付きコマンド引数と、ゲーム内操作　を紐づけたもの
    /// </summary>
    internal class Model : IGameOperationSpan
    {
        // - その他

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="startSeconds">ゲーム内時間（秒）</param>
        /// <param name="timedCommandArg">スパン・モデル</param>
        /// <param name="gameOperation"></param>
        public Model(
            float startSeconds,
            ModelOfSchedulerO2ndTimedCommandArgs.Model timedCommandArg,
            ModelOfSchedulerO4thGameOperation.IModel gameOperation)
        {
            this.StartSeconds = startSeconds;
            this.TimedCommandArg = timedCommandArg;
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
        public float EndSeconds => StartSeconds + this.TimedCommandArg.Duration;

        public ModelOfSchedulerO2ndTimedCommandArgs.Model TimedCommandArg { get; private set; }

        public ModelOfSchedulerO4thGameOperation.IModel GameOperation { get; private set; }
    }
}
