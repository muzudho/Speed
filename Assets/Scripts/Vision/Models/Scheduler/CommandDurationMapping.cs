namespace Assets.Scripts.Vision.Models.Scheduler
{
    using System;
    using System.Collections.Generic;
    using ModelOfThinkingEngineDigitalCommands = Assets.Scripts.ThinkingEngine.DigitalCommands;

    /// <summary>
    /// 思考エンジンと、画面の、コマンドの紐づき
    /// 
    /// - コマンド引数に、推定実行時間を紐づけます
    /// </summary>
    internal class CommandDurationMapping
    {
        // - その他

        static CommandDurationMapping()
        {
            // 一瞬
            float forMoment = 0.16f;

            // 隣の場札をピックアップする秒
            float durationOfMoveFocusToNextCard = 0.15f;

            DurationOfModels.Add(typeof(ModelOfThinkingEngineDigitalCommands.MoveCardsToHandFromPile).GetHashCode(), new GameSeconds(Commons.gameSpeedScale * (0.15f + durationOfMoveFocusToNextCard)));
            DurationOfModels.Add(typeof(ModelOfThinkingEngineDigitalCommands.MoveCardsToPileFromCenterStacks).GetHashCode(), new GameSeconds(Commons.gameSpeedScale * 0.3f));
            DurationOfModels.Add(typeof(ModelOfThinkingEngineDigitalCommands.MoveCardToCenterStackFromHand).GetHashCode(), new GameSeconds(Commons.gameSpeedScale * (0.15f + durationOfMoveFocusToNextCard)));
            DurationOfModels.Add(typeof(ModelOfThinkingEngineDigitalCommands.MoveFocusToNextCard).GetHashCode(), new GameSeconds(Commons.gameSpeedScale * durationOfMoveFocusToNextCard));
            DurationOfModels.Add(typeof(ModelOfThinkingEngineDigitalCommands.SetGameActive).GetHashCode(), new GameSeconds(Commons.gameSpeedScale * forMoment));
            DurationOfModels.Add(typeof(ModelOfThinkingEngineDigitalCommands.SetIdling).GetHashCode(), new GameSeconds(Commons.gameSpeedScale * forMoment)); // Idling の duration は可変の想定
        }

        // - プロパティ

        internal static Dictionary<int, GameSeconds> DurationOfModels = new();

        // - メソッド

        internal static GameSeconds GetDurationBy(Type type)
        {
            return DurationOfModels[type.GetHashCode()];
        }
    }
}
