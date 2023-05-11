namespace Assets.Scripts.Vision.Models.Scheduler.O4thComplexCommands
{
    using Assets.Scripts.Coding;
    using ModelOfGameBuffer = Assets.Scripts.ThinkingEngine.Models.Game.Buffer;
    using ModelOfGameWriter = Assets.Scripts.ThinkingEngine.Models.Game.Writer;
    using ModelOfInput = Assets.Scripts.Vision.Models.Input;
    using ModelOfScheduler = Assets.Scripts.Vision.Models.Scheduler;
    using ModelOfSchedulerO1stTimelineSpan = Assets.Scripts.Vision.Models.Scheduler.O1stTimelineSpan;
    using ModelOfThinkingEngineCommand = Assets.Scripts.ThinkingEngine.Models.Commands;

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
        /// <param name="command"></param>
        public SetGameActive(
            GameSeconds startObj,
            ModelOfThinkingEngineCommand.IModel command)
            : base(startObj, command)
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

            var command = (ModelOfThinkingEngineCommand.SetGameActive)this.CommandOfThinkingEngine;

            // モデル更新：１回実行すれば充分
            gameModelWriter.IsGameActive = command.IsGameActive;
            handled = true;

            // ビュー更新：なし
        }
    }
}
