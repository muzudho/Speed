namespace Assets.Scripts.Vision.Models.Scheduler.O4thGameOperation
{
    using Assets.Scripts.Coding;
    using Assets.Scripts.ThinkingEngine.Models;
    using Assets.Scripts.ThinkingEngine.Models.CommandArgs;
    using ModelOfSchedulerO1stTimelineSpan = Assets.Scripts.Vision.Models.Scheduler.O1stTimelineSpan;

    /// <summary>
    /// なんにもしません
    /// </summary>
    class SetIdlingView : ItsAbstract
    {
        // - その他（生成）

        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        public override IModel NewThis()
        {
            return new SetIdlingView();
        }

        // - フィールド

        // - プロパティ

        SetIdling GetModel(IGameOperationSpan timedGenerator)
        {
            return (SetIdling)timedGenerator.TimedCommandArg.CommandArg;
        }

        // - メソッド

        /// <summary>
        /// ゲーム画面の同期を始めます
        /// </summary>
        public override void CreateSpan(
            IGameOperationSpan timedGenerator,
            GameModelBuffer gameModelBuffer,
            LazyArgs.SetValue<ModelOfSchedulerO1stTimelineSpan.IModel> setViewMovement)
        {
            // なんにもしません
        }
    }
}
