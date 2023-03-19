﻿namespace Assets.Scripts.Vision.Models.Timeline.O4thGameOperation
{
    using Assets.Scripts.Coding;
    using Assets.Scripts.ThinkingEngine.Models;
    using Assets.Scripts.ThinkingEngine.Models.CommandArgs;
    using ModelOfTimelineO1stSpan = Assets.Scripts.Vision.Models.Timeline.O1stSpan;

    /// <summary>
    /// ｎプレイヤーの手札から場札へ、ｍ枚のカードを移動
    /// </summary>
    class SetGameActiveView : ItsAbstract
    {
        // - その他（生成）

        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        public override IModel NewThis()
        {
            return new SetGameActiveView();
        }

        // - フィールド

        bool handled;

        // - プロパティ

        SetGameActive GetModel(IGameOperationSpan timedGenerator)
        {
            return (SetGameActive)timedGenerator.TimedCommandArg.CommandArg;
        }

        // - メソッド

        /// <summary>
        /// ゲーム画面の同期を始めます
        /// </summary>
        public override void CreateSpan(
            IGameOperationSpan timedGenerator,
            GameModelBuffer gameModelBuffer,
            LazyArgs.SetValue<ModelOfTimelineO1stSpan.IBasecaseSpan> setViewMovement)
        {
            if (handled)
            {
                return;
            }

            // モデル更新：１回実行すれば充分
            gameModelBuffer.IsGameActive = GetModel(timedGenerator).IsGameActive;
            handled = true;

            // ビュー更新：なし
        }
    }
}
