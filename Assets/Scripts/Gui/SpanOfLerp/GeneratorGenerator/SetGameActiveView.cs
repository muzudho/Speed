namespace Assets.Scripts.Gui.SpanOfLerp.GeneratorGenerator
{
    using Assets.Scripts.ThinkingEngine.Model;
    using Assets.Scripts.ThinkingEngine.Model.CommandArgs;
    using SimulatorsOfTimeline = Assets.Scripts.Gui.SpanOfLerp.TimedGenerator;
    using SpanOfLeap = Assets.Scripts.Gui.SpanOfLerp;

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
        /// 
        /// - 手札の上の方からｎ枚抜いて、場札の後ろへ追加する
        /// - 画面上の場札は位置調整される
        /// </summary>
        public override void CreateSpanToLerp(
            SimulatorsOfTimeline.TimedGenerator timedGenerator,
            GameModelBuffer gameModelBuffer,
            LazyArgs.SetValue<SpanOfLeap.Model> setViewMovement)
        {
            if(handled)
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
