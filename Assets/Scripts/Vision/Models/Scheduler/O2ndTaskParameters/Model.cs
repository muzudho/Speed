namespace Assets.Scripts.Vision.Models.Scheduler.O2ndTaskParameters
{
    using ModelOfCommandParameter = Assets.Scripts.ThinkingEngine.Models.CommandParameters;

    /// <summary>
    /// タスクの引数
    /// 
    /// - コマンドは瞬間的に実行してしまうから、時間を付けよう、というもの
    /// </summary>
    internal class Model
    {
        // - その他

        internal Model(ModelOfCommandParameter.IModel commandArg)
        {
            this.CommandArg = commandArg;
            this.Duration = DurationMapping.GetDurationBy(CommandArg.GetType());
        }

        // - プロパティ

        internal ModelOfCommandParameter.IModel CommandArg { get; private set; }

        /// <summary>
        /// 持続時間（秒）
        /// </summary>
        internal float Duration { get; private set; }
    }
}
