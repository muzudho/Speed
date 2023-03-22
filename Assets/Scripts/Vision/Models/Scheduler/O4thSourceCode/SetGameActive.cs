﻿namespace Assets.Scripts.Vision.Models.Scheduler.O4thSourceCode
{
    using Assets.Scripts.Coding;
    using Assets.Scripts.ThinkingEngine.Models;
    using ModelOfSchedulerO1stTimelineSpan = Assets.Scripts.Vision.Models.Scheduler.O1stTimelineSpan;
    using ModelOfThinkingEngineCommandParameter = Assets.Scripts.ThinkingEngine.Models.CommandParameters;

    /// <summary>
    /// ｎプレイヤーの手札から場札へ、ｍ枚のカードを移動
    /// </summary>
    class SetGameActive : ItsAbstract
    {
        // - その他（生成）

        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        public override IModel NewThis()
        {
            return new SetGameActive();
        }

        // - フィールド

        bool handled;

        // - メソッド

        /// <summary>
        /// ゲーム画面の同期を始めます
        /// </summary>
        public override void Build(
            ITask task,
            GameModelBuffer gameModelBuffer,
            LazyArgs.SetValue<ModelOfSchedulerO1stTimelineSpan.IModel> setTimelineSpan)
        {
            if (handled)
            {
                return;
            }

            // モデル更新：１回実行すれば充分
            gameModelBuffer.IsGameActive = GetArg(task).IsGameActive;
            handled = true;

            // ビュー更新：なし
        }

        ModelOfThinkingEngineCommandParameter.SetGameActive GetArg(ITask task)
        {
            return (ModelOfThinkingEngineCommandParameter.SetGameActive)task.Args;
        }
    }
}
