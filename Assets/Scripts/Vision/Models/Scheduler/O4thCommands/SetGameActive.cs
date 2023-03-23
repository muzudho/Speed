namespace Assets.Scripts.Vision.Models.Scheduler.O4thCommands
{
    using Assets.Scripts.Coding;
    using Assets.Scripts.ThinkingEngine.Models;
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
        /// <param name="commandOfThinkingEngine"></param>
        public SetGameActive(ModelOfThinkingEngineCommand.IModel commandOfThinkingEngine)
            : base(commandOfThinkingEngine)
        {
        }

        // - フィールド

        bool handled;

        // - メソッド

        /// <summary>
        /// ゲーム画面の同期を始めます
        /// </summary>
        public override void GenerateSpan(
            ITask task,
            GameModelBuffer gameModelBuffer,
            LazyArgs.SetValue<ModelOfSchedulerO1stTimelineSpan.IModel> setTimelineSpan)
        {
            if (handled)
            {
                return;
            }

            // モデル更新：１回実行すれば充分
            gameModelBuffer.IsGameActive = GetCommandOfThinkingEngine(task).IsGameActive;
            handled = true;

            // ビュー更新：なし
        }

        ModelOfThinkingEngineCommand.SetGameActive GetCommandOfThinkingEngine(ITask task)
        {
            return (ModelOfThinkingEngineCommand.SetGameActive)task.CommandOfScheduler.CommandOfThinkingEngine;
        }
    }
}
