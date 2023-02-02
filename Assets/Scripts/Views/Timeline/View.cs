namespace Assets.Scripts.Views.Timeline
{
    using ModelsOfTimeline = Assets.Scripts.Models.Timeline;

    /// <summary>
    /// タイムライン・ビュー
    /// </summary>
    internal class View
    {
        // - その他（生成）

        public View(ModelsOfTimeline.Model model)
        {
            this.Model = model;
        }

        // - プロパティ

        internal ModelsOfTimeline.Model Model { get; private set; }

        // - メソッド

        /// <summary>
        /// モーションの補間
        /// </summary>
        /// <param name="elapsedSeconds">ゲーム内消費時間（秒）</param>
        internal void Lerp(float elapsedSeconds)
        {
            // TODO ★
            this.Model.Lerp(elapsedSeconds);
        }

    }
}
