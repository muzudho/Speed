namespace Assets.Scripts.Vision.Models.Scheduler
{
    using System;
    using System.Collections.Generic;
    using ModelOfThinkingEngineCommandParameter = Assets.Scripts.ThinkingEngine.Models.CommandParameters;

    /// <summary>
    /// コマンド引数に、推定実行時間を紐づけます
    /// </summary>
    internal class DurationMapping
    {
        // - その他

        static DurationMapping()
        {
            // 一瞬
            float forMoment = 0.16f;

            // 隣の場札をピックアップする秒
            float durationOfMoveFocusToNextCard = 0.15f;

            DurationOfModels.Add(typeof(ModelOfThinkingEngineCommandParameter.MoveCardsToHandFromPile).GetHashCode(), 0.15f + durationOfMoveFocusToNextCard);
            DurationOfModels.Add(typeof(ModelOfThinkingEngineCommandParameter.MoveCardsToPileFromCenterStacks).GetHashCode(), 0.3f);
            DurationOfModels.Add(typeof(ModelOfThinkingEngineCommandParameter.MoveCardToCenterStackFromHand).GetHashCode(), 0.15f + durationOfMoveFocusToNextCard);
            DurationOfModels.Add(typeof(ModelOfThinkingEngineCommandParameter.MoveFocusToNextCard).GetHashCode(), durationOfMoveFocusToNextCard);
            DurationOfModels.Add(typeof(ModelOfThinkingEngineCommandParameter.SetGameActive).GetHashCode(), forMoment);
            DurationOfModels.Add(typeof(ModelOfThinkingEngineCommandParameter.SetIdling).GetHashCode(), forMoment); // Idling の duration は可変の想定
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
