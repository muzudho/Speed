namespace Assets.Scripts.Views.Timeline
{
    using Assets.Scripts.Models;
    using SimulatorsOfTimeline = Assets.Scripts.Simulators.Timeline;

    /// <summary>
    /// タイム・スパン
    /// 
    /// - 指定した時間と、そのとき実行されるコマンドのペア
    /// </summary>
    internal abstract class AbstractSpanView : ISpanView
    {
        // - その他（生成）

        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        public abstract ISpanView Spawn();

        // - プロパティ

        // - メソッド

        virtual public void OnEnter(
            SimulatorsOfTimeline.TimeSpan timeSpan,
            GameModelBuffer gameModelBuffer,
            LazyArgs.SetValue<MovementViewModel> setMovementViewModel)
        {
            // Ignored
        }
    }
}
