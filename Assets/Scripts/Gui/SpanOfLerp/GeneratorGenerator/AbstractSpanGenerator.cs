namespace Assets.Scripts.Gui.SpanOfLerp.GeneratorGenerator
{
    using Assets.Scripts.ThinkingEngine.Model;
    using SimulatorsOfTimeline = Assets.Scripts.Gui.SpanOfLerp.TimedGenerator;
    using SpanOfLeap = Assets.Scripts.Gui.SpanOfLerp;

    /// <summary>
    /// スパン生成器
    /// 
    /// - 指定した時間と、そのとき実行されるコマンドのペア
    /// </summary>
    internal abstract class AbstractSpanGenerator : ISpanGenerator
    {
        // - その他（生成）

        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        public abstract ISpanGenerator Spawn();

        // - プロパティ

        // - メソッド

        /// <summary>
        /// ゲーム画面の同期を始めます
        /// </summary>
        /// <param name="timedGenerator"></param>
        /// <param name="gameModelBuffer"></param>
        /// <param name="setViewMovement"></param>
        virtual public void CreateSpanToLerp(
            SimulatorsOfTimeline.TimedGenerator timedGenerator,
            GameModelBuffer gameModelBuffer,
            LazyArgs.SetValue<SpanOfLeap.Model> setViewMovement)
        {
            // Ignored
        }
    }
}
