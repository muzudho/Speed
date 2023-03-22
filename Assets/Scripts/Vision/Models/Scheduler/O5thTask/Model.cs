namespace Assets.Scripts.Vision.Models.Scheduler.O5thTask
{
    using Assets.Scripts.Vision.Models.Scheduler.O2ndTaskParameters;
    using Assets.Scripts.Vision.Models.Scheduler.O4thSourceCode;
    using ModelOfSchedulerO2ndTaskParameters = Assets.Scripts.Vision.Models.Scheduler.O2ndTaskParameters;
    using ModelOfSchedulerO4thSourceCode = Assets.Scripts.Vision.Models.Scheduler.O4thSourceCode;

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
        /// <param name="sourceCode"></param>
        public Model(
            float startSeconds,
            ModelOfSchedulerO2ndTaskParameters.Model args,
            ModelOfSchedulerO4thSourceCode.IModel sourceCode)
        {
            this.StartSeconds = startSeconds;
            this.Args = args;
            this.SourceCode = sourceCode;
        }

        // - プロパティ

        /// <summary>
        /// 開始時間（秒）
        /// </summary>
        public float StartSeconds { get; private set; }

        /// <summary>
        /// 終了時間（秒）
        /// </summary>
        public float EndSeconds => StartSeconds + DurationMapping.GetDurationBy(this.Args.GetType());

        /// <summary>
        /// 引数のようなもの
        /// </summary>
        public ModelOfSchedulerO2ndTaskParameters.Model Args { get; private set; }

        /// <summary>
        /// ソースコードのようなもの
        /// </summary>
        public ModelOfSchedulerO4thSourceCode.IModel SourceCode { get; private set; }
    }
}
