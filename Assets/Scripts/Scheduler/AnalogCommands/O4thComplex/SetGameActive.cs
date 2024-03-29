﻿namespace Assets.Scripts.Scheduler.AnalogCommands.O4thComplex
{
    using Assets.Scripts.Vision.Models;
    using System.Collections.Generic;
    using ModelOfAnalogCommand1stTimelineSpan = Assets.Scripts.Scheduler.AnalogCommands.O1stTimelineSpan;
    using ModelOfAnalogCommands = Assets.Scripts.Scheduler.AnalogCommands;
    using ModelOfDigitalCommands = Assets.Scripts.ThinkingEngine.DigitalCommands;
    using ModelOfGameWriter = Assets.Scripts.ThinkingEngine.Models.Game.Writer;
    using ModelOfInput = Assets.Scripts.Vision.Models.Input;
    using ModelOfObservableGame = Assets.Scripts.ThinkingEngine.Models.Game.Observable;

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
            ModelOfDigitalCommands.IModel digitalCommand)
            : base(startObj, digitalCommand)
        {
        }

        // - フィールド

        bool handled;

        // - メソッド

        /// <summary>
        /// 準備
        /// </summary>
        public override void Setup(ModelOfObservableGame.Model observableGameModel)
        {

        }

        /// <summary>
        /// ゲーム画面の同期を始めます
        /// </summary>
        public override List<ModelOfAnalogCommand1stTimelineSpan.IModel> CreateTimespanList(
            ModelOfGameWriter.Model gameModelWriter,
            ModelOfInput.Init inputModel,
            ModelOfAnalogCommands.Model schedulerModel)
        {
            var result = new List<ModelOfAnalogCommand1stTimelineSpan.IModel>();

            if (handled)
            {
                return result;
            }

            var digitalCommand = (ModelOfDigitalCommands.SetGameActive)this.DigitalCommand;

            // モデル更新：１回実行すれば充分
            gameModelWriter.IsGameActive = digitalCommand.IsGameActive;
            handled = true;

            // ビュー更新：なし

            return result;
        }
    }
}
