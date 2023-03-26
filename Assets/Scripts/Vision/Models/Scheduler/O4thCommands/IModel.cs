namespace Assets.Scripts.Vision.Models.Scheduler.O4thCommands
{
    using Assets.Scripts.Coding;
    using ModelOfGameBuffer = Assets.Scripts.ThinkingEngine.Models.GameBuffer;
    using ModelOfInput = Assets.Scripts.Vision.Models.Input;
    using ModelOfScheduler = Assets.Scripts.Vision.Models.Scheduler;
    using ModelOfSchedulerO1stTimelineSpan = Assets.Scripts.Vision.Models.Scheduler.O1stTimelineSpan;
    using ModelOfThinkingEngineCommand = Assets.Scripts.ThinkingEngine.Models.Commands;

    /// <summary>
    /// 画面用のコマンド
    /// 
    /// - タイムライン上に配置されたもの
    /// - スパン（IBasecaseSpan）を生成します
    /// </summary>
    interface IModel
    {
        // - プロパティ

        /// <summary>
        /// ゲーム時間範囲（単位：秒）
        /// </summary>
        ModelOfSchedulerO1stTimelineSpan.Range TimeRangeObj { get; }

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
            ModelOfGameBuffer.Model gameModelBuffer,
            ModelOfInput.Init inputModel,
            ModelOfScheduler.Model schedulerModel,
            LazyArgs.SetValue<ModelOfSchedulerO1stTimelineSpan.IModel> setTimespan);
    }
}
