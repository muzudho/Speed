namespace Assets.Scripts.Vision.Models.Scheduler.O4thGameOperation
{
    using Assets.Scripts.Coding;
    using Assets.Scripts.ThinkingEngine.Models;
    using ModelOfSchedulerO1stTimelineSpan = Assets.Scripts.Vision.Models.Scheduler.O1stTimelineSpan;

    /// <summary>
    /// スパン生成器
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

        // - プロパティ

        // - メソッド

        /// <summary>
        /// ゲーム画面の同期を始めます
        /// </summary>
        /// <param name="timedGenerator"></param>
        /// <param name="gameModelBuffer"></param>
        /// <param name="setViewMovement"></param>
        virtual public void CreateSpan(
            IGameOperationSpan timedGenerator,
            GameModelBuffer gameModelBuffer,
            LazyArgs.SetValue<ModelOfSchedulerO1stTimelineSpan.IModel> setViewMovement)
        {
            // Ignored
        }
    }
}
