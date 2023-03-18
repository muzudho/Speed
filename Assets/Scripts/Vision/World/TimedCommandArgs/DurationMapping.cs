namespace Assets.Scripts.Vision.World.TimedCommandArgs
{
    using Assets.Scripts.ThinkingEngine.Models.CommandArgs;
    using System;
    using System.Collections.Generic;

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

            DurationOfModels.Add(typeof(MoveCardsToHandFromPileModel).GetHashCode(), 0.15f + durationOfMoveFocusToNextCard);
            DurationOfModels.Add(typeof(MoveCardsToPileFromCenterStacksModel).GetHashCode(), 0.3f);
            DurationOfModels.Add(typeof(MoveCardToCenterStackFromHandModel).GetHashCode(), 0.15f + durationOfMoveFocusToNextCard);
            DurationOfModels.Add(typeof(MoveFocusToNextCardModel).GetHashCode(), durationOfMoveFocusToNextCard);
            DurationOfModels.Add(typeof(SetGameActive).GetHashCode(), forMoment);
            DurationOfModels.Add(typeof(SetIdling).GetHashCode(), forMoment); // Idling の duration は可変の想定
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
