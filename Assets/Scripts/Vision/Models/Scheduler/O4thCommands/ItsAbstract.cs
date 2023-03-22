namespace Assets.Scripts.Vision.Models.Scheduler.O4thCommands
{
    using Assets.Scripts.Coding;
    using Assets.Scripts.ThinkingEngine.Models;
    using ModelOfSchedulerO1stTimelineSpan = Assets.Scripts.Vision.Models.Scheduler.O1stTimelineSpan;

    /// <summary>
    /// ソースコードのようなもの
    /// 
    /// - 指定した時間と、そのとき実行されるコマンドのペア
    /// </summary>
    internal abstract class ItsAbstract : IModel
    {
        // - その他（生成）

        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        public abstract IModel NewThis();

        // - メソッド

        /// <summary>
        /// ビルド
        /// 
        /// - ゲーム画面の同期を始めます
        /// </summary>
        /// <param name="task"></param>
        /// <param name="gameModelBuffer"></param>
        /// <param name="setTimelineSpan"></param>
        virtual public void Build(
            ITask task,
            GameModelBuffer gameModelBuffer,
            LazyArgs.SetValue<ModelOfSchedulerO1stTimelineSpan.IModel> setTimelineSpan)
        {
            // Ignored
        }
    }
}
