namespace Assets.Scripts.Vision.Models.Scheduler.O2ndTaskArgs
{
    using Assets.Scripts.ThinkingEngine.Models.CommandArgs;

    /// <summary>
    /// タスクの引数
    /// </summary>
    internal class Model
    {
        // - その他

        internal Model(ICommandArg commandArg)
        {
            this.CommandArg = commandArg;
            this.Duration = DurationMapping.GetDurationBy(CommandArg.GetType());
        }

        // - プロパティ

        internal ICommandArg CommandArg { get; private set; }

        /// <summary>
        /// 持続時間（秒）
        /// </summary>
        internal float Duration { get; private set; }
    }
}
