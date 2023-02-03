﻿namespace Assets.Scripts.Simulators.Timeline
{
    using Assets.Scripts.Models;
    using Assets.Scripts.Views.Timeline;
    using Assets.Scripts.Views;
    using ModelsOfTimeline = Assets.Scripts.Models.Timeline;
    using UnityEngine;
    using Assets.Scripts.Models.Timeline;
    using Assets.Scripts.Models.Timeline.Spans;

    internal class Simulator
    {
        // - その他（生成）

        public Simulator(ModelsOfTimeline.Model model)
        {
            this.Model = model;
        }

        // - プロパティ

        internal ModelsOfTimeline.Model Model { get; private set; }

        /// <summary>
        /// タイム・ライン作成用カウンター
        /// 
        /// - プレイヤー別
        /// </summary>
        internal float[] ScheduledSeconds { get; private set; } = { 0.0f, 0.0f };

        // - メソッド

        internal void AddScheduleSeconds(int player, float seconds)
        {
            this.ScheduledSeconds[player] += seconds;
        }

        /// <summary>
        /// コマンドを消化
        /// </summary>
        /// <param name="elapsedSeconds">ゲーム内消費時間（秒）</param>
        /// <param name="gameModelBuffer">ゲームの内部状態（編集可能）</param>
        /// <param name="gameViewModel">画面表示の状態（編集可能）</param>
        internal void OnEnter(
            float elapsedSeconds,
            GameModelBuffer gameModelBuffer,
            GameViewModel gameViewModel,
            LazyArgs.SetValue<MovementViewModel> setMovementViewModel)
        {
            // TODO ★ スレッド・セーフにしたい
            // キューに溜まっている分を全て消化
            int i = 0;
            while (i < this.Model.GetCountItems())
            {
                var spanView = this.Model.GetItemAt(i);

                // まだ
                if (elapsedSeconds < spanView.TimeSpan.StartSeconds)
                {
                    i++;
                    continue;
                }

                // 起動
                // ----
                Debug.Log($"[Assets.Scripts.Models.Timeline.Model OnEnter] タイム・スパン実行 span.StartSeconds:{spanView.TimeSpan.StartSeconds} <= elapsedSeconds:{elapsedSeconds}");

                // スケジュールから除去
                this.Model.RemoveAt(i);

                // 実行
                spanView.OnEnter(
                    spanView.TimeSpan,
                    gameModelBuffer,
                    gameViewModel,
                    setMovementViewModel: setMovementViewModel);
            }
        }
    }
}
