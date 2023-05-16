namespace Assets.Scripts.Scheduler.AnalogCommands.O7thTimeline
{
    using Assets.Scripts.ThinkingEngine.Models;
    using Assets.Scripts.Vision.Models;
    using System.Collections.Generic;
    using UnityEngine;
    using ModelOfObservableGame = Assets.Scripts.ThinkingEngine.Models.Game.Observable;
    using ModelOfAnalogCommand4thComplex = Assets.Scripts.Scheduler.AnalogCommands.O4thComplex;
    using ModelOfAnalogCommand6thMapping = Assets.Scripts.Scheduler.AnalogCommands.O6thDACommandMapping;
    using ModelOfDigitalCommands = Assets.Scripts.ThinkingEngine.DigitalCommands;

    /// <summary>
    /// タイムライン
    /// 
    /// - 編集可能（Mutable）
    /// </summary>
    internal class Model
    {
        // - その他（生成）

        #region その他（生成）
        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="gameModel"></param>
        public Model(ModelOfObservableGame.Model gameModel)
        {
            this.GameModel = gameModel;

            this.CleanUp();
        }
        #endregion

        // - プロパティ

        #region プロパティ（ゲーム・モデル）
        /// <summary>
        /// ゲーム・モデル
        /// </summary>
        internal ModelOfObservableGame.Model GameModel { get; private set; }
        #endregion

        #region プロパティ（スケジュールに登録されている残りの項目）
        /// <summary>
        /// スケジュールに登録されている残りの項目
        /// 
        /// - 実行されると、 `ongoingItems` へ移動する
        /// </summary>

        List<ModelOfAnalogCommand4thComplex.IModel> analogCommands = new();

        internal List<ModelOfAnalogCommand4thComplex.IModel> AnalogCommands => this.analogCommands;
        #endregion

        #region プロパティ（タイム・ライン作成用カウンター）
        /// <summary>
        /// タイム・ライン作成用カウンター
        /// 
        /// - プレイヤー別
        /// </summary>
        internal GameSeconds[] ScheduledTimesObj { get; private set; } = new GameSeconds[2];
        #endregion

        // - メソッド

        #region メソッド（コマンド追加）
        /// <summary>
        /// コマンド追加
        /// </summary>
        /// <param name="digitalCommand">コマンド</param>
        internal void AddCommand(
            GameSeconds startObj,
            ModelOfDigitalCommands.IModel digitalCommand)
        {
            this.AnalogCommands.Add(ModelOfAnalogCommand6thMapping.Model.WrapCommand(
                startObj: startObj,
                digitalCommand: digitalCommand));
        }
        #endregion

        #region メソッド（追加）
        /// <summary>
        /// 追加
        /// 
        /// - タイムを自動的に付ける
        /// </summary>
        /// <param name="playerObj"></param>
        /// <param name="digitalCommand">コマンド引数</param>
        internal void AddWithinScheduler(Player playerObj, ModelOfDigitalCommands.IModel digitalCommand)
        {
            var analogCommand = ModelOfAnalogCommand6thMapping.Model.WrapCommand(
                        startObj: this.ScheduledTimesObj[playerObj.AsInt],
                        digitalCommand: digitalCommand);

            this.AnalogCommands.Add(analogCommand);

            // 次の時間
            this.ScheduledTimesObj[playerObj.AsInt] = new GameSeconds(
                this.ScheduledTimesObj[playerObj.AsInt].AsFloat + analogCommand.TimeRangeObj.DurationObj.AsFloat);
        }
        #endregion

        internal void AddScheduleSeconds(Player playerObj, GameSeconds time)
        {
            this.ScheduledTimesObj[playerObj.AsInt] = new GameSeconds(this.ScheduledTimesObj[playerObj.AsInt].AsFloat + time.AsFloat);
        }

        internal ModelOfAnalogCommand4thComplex.IModel GetCommandAt(int index)
        {
            return this.AnalogCommands[index];
        }

        internal int GetCountCommands()
        {
            return this.AnalogCommands.Count;
        }

        internal void RemoveAt(int index)
        {
            this.AnalogCommands.RemoveAt(index);
        }

        internal void DebugWrite()
        {
            Debug.Log($"[Assets.Scripts.Scheduler.AnalogCommands.O7thTimeline.Model DebugWrite] commands.Count:{analogCommands.Count}");
        }

        internal float LastSeconds() => Mathf.Max(this.ScheduledTimesObj[0].AsFloat, ScheduledTimesObj[1].AsFloat);

        #region メソッド（初期化）
        /// <summary>
        /// 初期化
        /// </summary>
        internal void CleanUp()
        {
            this.analogCommands.Clear();

            this.ScheduledTimesObj[0] = GameSeconds.Zero;
            this.ScheduledTimesObj[1] = GameSeconds.Zero;
        }
        #endregion
    }
}
