﻿namespace Assets.Scripts.Vision.Models.Scheduler.O4thCommands
{
    using Assets.Scripts.Coding;
    using Assets.Scripts.ThinkingEngine.Models;
    using ModelOfSchedulerO1stTimelineSpan = Assets.Scripts.Vision.Models.Scheduler.O1stTimelineSpan;
    using ModelOfThinkingEngineCommand = Assets.Scripts.ThinkingEngine.Models.Commands;

    /// <summary>
    /// ソースコードのようなもの
    /// 
    /// - 指定した時間と、そのとき実行されるコマンドのペア
    /// </summary>
    internal abstract class ItsAbstract : IModel
    {
        // - その他（生成）

        protected ItsAbstract(ModelOfThinkingEngineCommand.IModel commandOfThinkingEngine)
        {
            this.CommandOfThinkingEngine = commandOfThinkingEngine;
        }

        // - プロパティ

        /// <summary>
        /// 思考エンジン用のコマンド
        /// </summary>
        public ModelOfThinkingEngineCommand.IModel CommandOfThinkingEngine { get; private set; }

        // - メソッド

        /// <summary>
        /// ビルド
        /// 
        /// - ゲーム画面の同期を始めます
        /// </summary>
        /// <param name="task"></param>
        /// <param name="gameModelBuffer"></param>
        /// <param name="setTimelineSpan"></param>
        virtual public void GenerateSpan(
            ITask task,
            GameModelBuffer gameModelBuffer,
            LazyArgs.SetValue<ModelOfSchedulerO1stTimelineSpan.IModel> setTimelineSpan)
        {
            // Ignored
        }
    }
}
