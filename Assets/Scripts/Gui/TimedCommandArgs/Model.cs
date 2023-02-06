namespace Assets.Scripts.Gui.TimedCommandArgs
{
    using Assets.Scripts.ThinkingEngine.CommandArgs;

    internal class Model
    {
        // - その他

        internal Model(ICommandArg commandArgs)
        {
            this.CommandArgs = commandArgs;
            this.Duration = DurationMapping.GetDurationBy(CommandArgs.GetType());
        }

        // - プロパティ

        internal ICommandArg CommandArgs { get; private set; }

        /// <summary>
        /// 持続時間（秒）
        /// </summary>
        internal float Duration { get; private set; }
    }
}
