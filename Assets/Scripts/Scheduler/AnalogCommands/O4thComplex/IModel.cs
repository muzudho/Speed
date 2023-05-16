namespace Assets.Scripts.Scheduler.AnalogCommands.O4thComplex
{
    using System.Collections.Generic;
    using ModelOfAnalogCommand1stTimelineSpan = Assets.Scripts.Scheduler.AnalogCommands.O1stTimelineSpan;
    using ModelOfAnalogCommands = Assets.Scripts.Scheduler.AnalogCommands;
    using ModelOfDigitalCommands = Assets.Scripts.ThinkingEngine.DigitalCommands;
    using ModelOfObservableGame = Assets.Scripts.ThinkingEngine.Models.Game.Observable;
    using ModelOfGameBuffer = Assets.Scripts.ThinkingEngine.Models.Game.Buffer;
    using ModelOfGameWriter = Assets.Scripts.ThinkingEngine.Models.Game.Writer;
    using ModelOfInput = Assets.Scripts.Vision.Models.Input;

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
        ModelOfAnalogCommand1stTimelineSpan.Range TimeRangeObj { get; }

        /// <summary>
        /// 思考エンジン用のコマンド
        /// </summary>
        ModelOfDigitalCommands.IModel DigitalCommand { get; }

        // - メソッド

        /// <summary>
        /// 準備
        /// 
        /// - 内部状態、表示状態を読み取る
        /// </summary>
        void Setup(ModelOfObservableGame.Model observableGameModel);

        /// <summary>
        /// ゲーム画面の同期を始めます
        /// 
        /// - 内部状態、表示状態を書き換える
        /// </summary>
        /// <param name="gameModelWriter">ゲームの内部状態（編集用）</param>
        List<ModelOfAnalogCommand1stTimelineSpan.IModel> CreateTimespanList(
            ModelOfGameWriter.Model gameModelWriter,
            ModelOfInput.Init inputModel,
            ModelOfAnalogCommands.Model schedulerModel);
    }
}
