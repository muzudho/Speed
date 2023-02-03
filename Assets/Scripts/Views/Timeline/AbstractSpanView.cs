namespace Assets.Scripts.Views.Timeline
{
    using Assets.Scripts.Models;
    using Assets.Scripts.Views;

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
        public abstract ISpanView Spawn(TimeSpanView timeSpan);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeSpan">タイム・スパン</param>
        public AbstractSpanView(TimeSpanView timeSpan)
        {
            this.TimeSpan = timeSpan;
        }

        // - プロパティ

        /// <summary>
        /// タイム・スパン
        /// </summary>
        public TimeSpanView TimeSpan { get; private set; }

        // - メソッド

        virtual public void OnEnter(
            TimeSpanView timeSpan,
            GameModelBuffer gameModelBuffer,
            GameViewModel gameViewModel,
            LazyArgs.SetValue<MovementViewModel> setMovementViewModel)
        {
            // Ignored
        }
    }
}
