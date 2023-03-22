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
        }

        // - プロパティ

        internal ModelOfCommandParameter.IModel CommandArg { get; private set; }
    }
}
