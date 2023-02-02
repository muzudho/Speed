﻿namespace Assets.Scripts.Models.Timeline
{
    using Assets.Scripts.Models.Timeline.Commands;
    using UnityEngine;

    /// <summary>
    /// 指定した時間と、そのとき実行されるコマンドのペア
    /// </summary>
    class TimedItem
    {
        // - その他（生成）

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startSeconds">ゲーム内時間（秒）</param>
        /// <param name="duration">持続時間（秒）</param>
        /// <param name="command">コマンド</param>
        internal TimedItem(float startSeconds, float duration, ICommand command)
        {
            this.StartSeconds = startSeconds;
            this.Duration = duration;
            this.Command = command;
        }

        // - プロパティ

        /// <summary>
        /// 開始時間（秒）
        /// </summary>
        internal float StartSeconds { get; private set; }

        /// <summary>
        /// 持続時間（秒）
        /// </summary>
        internal float Duration { get; private set; }

        /// <summary>
        /// 終了時間（秒）
        /// </summary>
        internal float EndSeconds
        {
            get
            {
                return StartSeconds + Duration;
            }
        }

        internal ICommand Command { get; private set; }

        // - メソッド

        internal void Lerp(float elapsedSeconds)
        {
            float progress = (elapsedSeconds - this.StartSeconds) / this.Duration;
            

            // 超えることがある
            if (1.0f < progress)
            {
                // 1.0 を超えるのもよくない
                Debug.Log($"[Lerp] progress:{progress} (Over 1.0)");
                progress = 1.0f;
            }

            this.Command.Lerp(progress);
        }

        internal void OnLeave()
        {
            this.Command.OnLeave();
        }
    }
}
