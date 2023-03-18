namespace Assets.Scripts.Vision.World.SpanOfLerp.GeneratorGenerator
{
    using Assets.Scripts.Coding;
    using Assets.Scripts.ThinkingEngine.Models;
    using Assets.Scripts.ThinkingEngine.Models.CommandArgs;
    using SimulatorsOfTimeline = Assets.Scripts.Vision.World.SpanOfLerp.TimedGenerator;
    using SpanOfLeap = Assets.Scripts.Vision.World.SpanOfLerp;

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
            LazyArgs.SetValue<SpanOfLeap.Model> setViewMovement)
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
