namespace Assets.Scripts.Vision.Models.Scheduler.O4thGameOperation
{
    using Assets.Scripts.Coding;
    using Assets.Scripts.ThinkingEngine.Models;
    using Assets.Scripts.ThinkingEngine.Models.CommandArgs;
    using ModelOfSchedulerO1stTimelineSpan = Assets.Scripts.Vision.Models.Scheduler.O1stTimelineSpan;

    /// <summary>
    /// ｎプレイヤーの手札から場札へ、ｍ枚のカードを移動
    /// </summary>
    class SetGameActiveView : ItsAbstract
    {
        // - その他（生成）

        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        public override IModel NewThis()
        {
            return new SetGameActiveView();
        }

        // - フィールド

        bool handled;

        // - プロパティ

        SetGameActive GetModel(IGameOperationSpan timedGenerator)
        {
            return (SetGameActive)timedGenerator.TimedCommandArg.CommandArg;
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
