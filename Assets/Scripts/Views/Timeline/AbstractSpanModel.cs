namespace Assets.Scripts.Views.Timeline
{
    using Assets.Scripts.Models;
    using Assets.Scripts.Views;

    /// <summary>
    /// タイム・スパン
    /// 
    /// - 指定した時間と、そのとき実行されるコマンドのペア
    /// </summary>
    internal abstract class AbstractSpanModel : ISpanModel
    {
        // - その他（生成）

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeSpan">タイム・スパン</param>
        public AbstractSpanModel(TimeSpan timeSpan)
        {
            this.TimeSpan = timeSpan;
        }

        // - プロパティ

        /// <summary>
        /// タイム・スパン
        /// </summary>
        public TimeSpan TimeSpan { get; private set; }

        // - メソッド

        virtual public void OnEnter(
            GameModelBuffer gameModelBuffer,
            GameViewModel gameViewModel,
            LazyArgs.SetValue<CardMovementViewModel> setLaunchedSpanModel)
        {
            // Ignored
        }
    }
}
