namespace Assets.Scripts.Vision.Models.Scheduler.O4thCommands
{
    using Assets.Scripts.Coding;
    using Assets.Scripts.ThinkingEngine.Models;
    using ModelOfSchedulerO1stTimelineSpan = Assets.Scripts.Vision.Models.Scheduler.O1stTimelineSpan;
    using ModelOfThinkingEngineCommand = Assets.Scripts.ThinkingEngine.Models.Commands;

    /// <summary>
    /// ソースコードのようなもの
    /// 
    /// - タイムライン上に配置されたもの
    /// - スパン（IBasecaseSpan）を生成します
    /// </summary>
    interface IModel
    {
        // - プロパティ

        /// <summary>
        /// 思考エンジン用のコマンド
        /// </summary>
        ModelOfThinkingEngineCommand.IModel CommandOfThinkingEngine { get; }

        // - メソッド

        /// <summary>
        /// ゲーム画面の同期を始めます
        /// </summary>
        /// <param name="gameModelBuffer">ゲームの内部状態（編集可能）</param>
        void GenerateSpan(
            ITask task,
            GameModelBuffer gameModelBuffer,
            LazyArgs.SetValue<ModelOfSchedulerO1stTimelineSpan.IModel> setTimelineSpan);
    }
}
