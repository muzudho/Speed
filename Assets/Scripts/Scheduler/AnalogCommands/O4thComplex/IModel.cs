namespace Assets.Scripts.Scheduler.AnalogCommands.O4thComplex
{
    using Assets.Scripts.Coding;
    using System.Collections.Generic;
    using ModelOfAnalogCommand1stTimelineSpan = Assets.Scripts.Scheduler.AnalogCommands.O1stTimelineSpan;
    using ModelOfAnalogCommands = Assets.Scripts.Scheduler.AnalogCommands;
    using ModelOfDigitalCommands = Assets.Scripts.ThinkingEngine.DigitalCommands;
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
        /// ゲーム画面の同期を始めます
        /// </summary>
        /// <param name="gameModelWriter">ゲームの内部状態（編集用）</param>
        List<ModelOfAnalogCommand1stTimelineSpan.IModel> CreateTimespanList(
            ModelOfGameBuffer.Model gameModelBuffer,
            ModelOfGameWriter.Model gameModelWriter,
            ModelOfInput.Init inputModel,
            ModelOfAnalogCommands.Model schedulerModel);
    }
}
