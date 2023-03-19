namespace Assets.Scripts.Vision.Models.Timeline.O3rdBElements
{
    using Assets.Scripts.Coding;
    using Assets.Scripts.ThinkingEngine.Models;
    using Assets.Scripts.ThinkingEngine.Models.CommandArgs;
    using ModelOfTimelineO1stElement = Assets.Scripts.Vision.Models.Timeline.O1stElements;

    /// <summary>
    /// なんにもしません
    /// </summary>
    class SetIdlingView : AbstractSpanGenerator
    {
        // - その他（生成）

        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        public override ISpanGenerator Spawn()
        {
            return new SetIdlingView();
        }

        // - フィールド

        // - プロパティ

        SetIdling GetModel(ModelOfTimelineO1stElement.TimedGenerator timedGenerator)
        {
            return (SetIdling)timedGenerator.TimedCommandArg.CommandArg;
        }

        // - メソッド

        /// <summary>
        /// ゲーム画面の同期を始めます
        /// </summary>
        public override void CreateSpanToLerp(
            ITimedGenerator timedGenerator,
            GameModelBuffer gameModelBuffer,
            LazyArgs.SetValue<IFinalLevelSpan> setViewMovement)
        {
            // なんにもしません
        }
    }
}
