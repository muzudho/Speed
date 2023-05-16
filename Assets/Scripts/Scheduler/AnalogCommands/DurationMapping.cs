namespace Assets.Scripts.Scheduler.AnalogCommands
{
    using Assets.Scripts.Vision;
    using Assets.Scripts.Vision.Models;
    using System;
    using System.Collections.Generic;
    using ModelOfDigitalCommands = Assets.Scripts.ThinkingEngine.DigitalCommands;

    /// <summary>
    /// 思考エンジンと、画面の、コマンドの紐づき
    /// 
    /// - コマンド引数に、推定実行時間を紐づけます
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

            DurationOfModels.Add(typeof(ModelOfDigitalCommands.MoveCardsToHandFromPile).GetHashCode(), new GameSeconds(Commons.gameSpeedScale * (0.15f + durationOfMoveFocusToNextCard)));
            DurationOfModels.Add(typeof(ModelOfDigitalCommands.MoveCardsToPileFromCenterStacks).GetHashCode(), new GameSeconds(Commons.gameSpeedScale * 0.3f));
            DurationOfModels.Add(typeof(ModelOfDigitalCommands.MoveCardToCenterStackFromHand).GetHashCode(), new GameSeconds(Commons.gameSpeedScale * (0.15f + durationOfMoveFocusToNextCard)));
            DurationOfModels.Add(typeof(ModelOfDigitalCommands.MoveFocusToNextCard).GetHashCode(), new GameSeconds(Commons.gameSpeedScale * durationOfMoveFocusToNextCard));
            DurationOfModels.Add(typeof(ModelOfDigitalCommands.SetGameActive).GetHashCode(), new GameSeconds(Commons.gameSpeedScale * forMoment));
            DurationOfModels.Add(typeof(ModelOfDigitalCommands.SetIdling).GetHashCode(), new GameSeconds(Commons.gameSpeedScale * forMoment)); // Idling の duration は可変の想定
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
