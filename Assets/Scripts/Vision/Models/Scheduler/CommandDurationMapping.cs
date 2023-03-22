namespace Assets.Scripts.Vision.Models.Scheduler
{
    using System;
    using System.Collections.Generic;
    using ModelOfThinkingEngineCommand = Assets.Scripts.ThinkingEngine.Models.Commands;

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

            DurationOfModels.Add(typeof(ModelOfThinkingEngineCommand.MoveCardsToHandFromPile).GetHashCode(), 0.15f + durationOfMoveFocusToNextCard);
            DurationOfModels.Add(typeof(ModelOfThinkingEngineCommand.MoveCardsToPileFromCenterStacks).GetHashCode(), 0.3f);
            DurationOfModels.Add(typeof(ModelOfThinkingEngineCommand.MoveCardToCenterStackFromHand).GetHashCode(), 0.15f + durationOfMoveFocusToNextCard);
            DurationOfModels.Add(typeof(ModelOfThinkingEngineCommand.MoveFocusToNextCard).GetHashCode(), durationOfMoveFocusToNextCard);
            DurationOfModels.Add(typeof(ModelOfThinkingEngineCommand.SetGameActive).GetHashCode(), forMoment);
            DurationOfModels.Add(typeof(ModelOfThinkingEngineCommand.SetIdling).GetHashCode(), forMoment); // Idling の duration は可変の想定
        }

        // - プロパティ

        internal static Dictionary<int, float> DurationOfModels = new();

        // - メソッド

        internal static float GetDurationBy(Type type)
        {
            return DurationOfModels[type.GetHashCode()];
        }
    }
}
