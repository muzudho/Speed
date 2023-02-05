﻿namespace Assets.Scripts.Simulators
{
    using Assets.Scripts.Gui.GeneratorOfSpanOfLerp.Elements;
    using Assets.Scripts.ThikningEngine.CommandArgs;

    /// <summary>
    /// コマンド引数と、スパン・ビューを紐づけます
    /// </summary>
    internal class TimeSpan
    {
        // - その他（生成）

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startSeconds">ゲーム内時間（秒）</param>
        /// <param name="spanModel">スパン・モデル</param>
        public TimeSpan(float startSeconds, ICommandArgs spanModel, ISpanGenerator spanView)
        {
            this.StartSeconds = startSeconds;
            this.SpanModel = spanModel;
            this.SpanView = spanView;
        }

        // - プロパティ

        /// <summary>
        /// 開始時間（秒）
        /// </summary>
        public float StartSeconds { get; private set; }

        /// <summary>
        /// 持続時間（秒）
        /// </summary>
        public float Duration => Specification.GetDurationBy(SpanModel.GetType());

        /// <summary>
        /// 終了時間（秒）
        /// </summary>
        public float EndSeconds => StartSeconds + Duration;

        public ICommandArgs SpanModel { get; private set; }

        public ISpanGenerator SpanView { get; private set; }
    }
}
