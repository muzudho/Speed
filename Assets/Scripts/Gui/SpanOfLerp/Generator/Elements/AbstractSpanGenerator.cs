namespace Assets.Scripts.Gui.SpanOfLerp.Generator.Elements
{
    using Assets.Scripts.ThikningEngine;
    using Assets.Scripts.Views.Timeline;
    using SimulatorsOfTimeline = Assets.Scripts.Simulators;

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
            LazyArgs.SetValue<SpanToLerp> setViewMovement)
        {
            // Ignored
        }
    }
}
