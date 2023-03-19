namespace Assets.Scripts.Vision.Models.Timeline.SpanOfLerp.GeneratorGenerator
{
    using Assets.Scripts.Coding;
    using Assets.Scripts.ThinkingEngine.Models;
    using Assets.Scripts.ThinkingEngine.Models.CommandArgs;
    using SimulatorsOfTimeline = Assets.Scripts.Vision.Models.Timeline.SpanOfLerp.TimedGenerator;
    using VisionOfTimelineO4thElement = Assets.Scripts.Vision.Models.Timeline.O4thElement;

    /// <summary>
    /// ｎプレイヤーの手札から場札へ、ｍ枚のカードを移動
    /// </summary>
    class SetGameActiveView : AbstractSpanGenerator
    {
        // - その他（生成）

        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        public override ISpanGenerator Spawn()
        {
            return new SetGameActiveView();
        }

        // - フィールド

        bool handled;

        // - プロパティ

        SetGameActive GetModel(SimulatorsOfTimeline.TimedGenerator timedGenerator)
        {
            return (SetGameActive)timedGenerator.TimedCommandArg.CommandArg;
        }

        // - メソッド

        /// <summary>
        /// ゲーム画面の同期を始めます
        /// </summary>
        public override void CreateSpanToLerp(
            SimulatorsOfTimeline.TimedGenerator timedGenerator,
            GameModelBuffer gameModelBuffer,
            LazyArgs.SetValue<VisionOfTimelineO4thElement.Model> setViewMovement)
        {
            if (handled)
            {
                return;
            }

            // モデル更新：１回実行すれば充分
            gameModelBuffer.IsGameActive = GetModel(timedGenerator).IsGameActive;
            handled = true;

            // ビュー更新：なし
        }
    }
}
