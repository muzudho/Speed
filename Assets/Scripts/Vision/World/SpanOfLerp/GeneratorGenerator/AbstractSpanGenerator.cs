namespace Assets.Scripts.Vision.World.SpanOfLerp.GeneratorGenerator
{
    using Assets.Scripts.Coding;
    using Assets.Scripts.ThinkingEngine.Models;
    using SimulatorsOfTimeline = Assets.Scripts.Vision.World.SpanOfLerp.TimedGenerator;
    using SpanOfLeap = Assets.Scripts.Vision.World.SpanOfLerp;

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
