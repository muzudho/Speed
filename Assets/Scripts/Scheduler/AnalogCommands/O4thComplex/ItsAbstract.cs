﻿namespace Assets.Scripts.Scheduler.AnalogCommands.O4thComplex
{
    using Assets.Scripts.Coding;
    using Assets.Scripts.Vision.Models;
    using System.Collections.Generic;
    using ModelOfAnalogCommand1stTimelineSpan = Assets.Scripts.Scheduler.AnalogCommands.O1stTimelineSpan;
    using ModelOfAnalogCommands = Assets.Scripts.Scheduler.AnalogCommands;
    using ModelOfDigitalCommands = Assets.Scripts.ThinkingEngine.DigitalCommands;
    using ModelOfGameBuffer = Assets.Scripts.ThinkingEngine.Models.Game.Buffer;
    using ModelOfGameWriter = Assets.Scripts.ThinkingEngine.Models.Game.Writer;
    using ModelOfInput = Assets.Scripts.Vision.Models.Input;
    using ModelOfObservableGame = Assets.Scripts.ThinkingEngine.Models.Game.Observable;

    /// <summary>
    /// ソースコードのようなもの
    /// 
    /// - 指定した時間と、そのとき実行されるコマンドのペア
    /// </summary>
    internal abstract class ItsAbstract : IModel
    {
        // - その他（生成）

        protected ItsAbstract(
            GameSeconds startTimeObj,
            ModelOfDigitalCommands.IModel digitalCommand)
        {
            this.TimeRangeObj = new ModelOfAnalogCommand1stTimelineSpan.Range(startTimeObj, DurationMapping.GetDurationBy(digitalCommand.GetType()));
            this.DigitalCommand = digitalCommand;
        }

        // - プロパティ

        /// <summary>
        /// ゲーム時間範囲（単位：秒）
        /// </summary>
        public ModelOfAnalogCommand1stTimelineSpan.Range TimeRangeObj { get; private set; }

        /// <summary>
        /// 思考エンジン用のコマンド
        /// </summary>
        public ModelOfDigitalCommands.IModel DigitalCommand { get; private set; }

        // - メソッド

        /// <summary>
        /// 準備
        /// </summary>
        public abstract void Setup(ModelOfObservableGame.Model observableGameModel);

        /// <summary>
        /// ビルド
        /// 
        /// - ゲーム画面の同期を始めます
        /// </summary>
        /// <param name="commandOfScheduler"></param>
        /// <param name="gameModelWriter"></param>
        /// <param name="setTimespan"></param>
        public abstract List<ModelOfAnalogCommand1stTimelineSpan.IModel> CreateTimespanList(
            ModelOfGameWriter.Model gameModelWriter,
            ModelOfInput.Init inputModel,
            ModelOfAnalogCommands.Model schedulerModel);
    }
}
