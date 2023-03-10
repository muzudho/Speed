namespace Assets.Scripts.Vision.World.TimedCommandArgs
{
    using Assets.Scripts.ThinkingEngine.Model.CommandArgs;

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
