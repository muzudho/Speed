namespace Assets.Scripts.Scheduler.AnalogCommands.O4thComplex
{
    using Assets.Scripts.Coding;
    using Assets.Scripts.Vision.Models;
    using ModelOfGameBuffer = Assets.Scripts.ThinkingEngine.Models.Game.Buffer;
    using ModelOfGameWriter = Assets.Scripts.ThinkingEngine.Models.Game.Writer;
    using ModelOfInput = Assets.Scripts.Vision.Models.Input;
    using ModelOfScheduler = Assets.Scripts.Scheduler.AnalogCommands;
    using ModelOfSchedulerO1stTimelineSpan = Assets.Scripts.Scheduler.AnalogCommands.O1stTimelineSpan;
    using ModelOfThinkingEngineDigitalCommands = Assets.Scripts.ThinkingEngine.DigitalCommands;

    /// <summary>
    /// ｎプレイヤーの手札から場札へ、ｍ枚のカードを移動
    /// </summary>
    class SetGameActive : ItsAbstract
    {
        // - その他

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="startObj"></param>
        /// <param name="digitalCommand"></param>
        public SetGameActive(
            GameSeconds startObj,
            ModelOfThinkingEngineDigitalCommands.IModel digitalCommand)
            : base(startObj, digitalCommand)
        {
        }

        // - フィールド

        bool handled;

        // - メソッド

        /// <summary>
        /// ゲーム画面の同期を始めます
        /// </summary>
        public override void GenerateSpan(
            ModelOfGameBuffer.Model gameModelBuffer,
            ModelOfGameWriter.Model gameModelWriter,
            ModelOfInput.Init inputModel,
            ModelOfScheduler.Model schedulerModel,
            LazyArgs.SetValue<ModelOfSchedulerO1stTimelineSpan.IModel> setTimespan)
        {
            if (handled)
            {
                return;
            }

            var digitalCommand = (ModelOfThinkingEngineDigitalCommands.SetGameActive)this.DigitalCommand;

            // モデル更新：１回実行すれば充分
            gameModelWriter.IsGameActive = digitalCommand.IsGameActive;
            handled = true;

            // ビュー更新：なし
        }
    }
}
