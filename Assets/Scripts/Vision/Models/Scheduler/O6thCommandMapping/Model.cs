namespace Assets.Scripts.Vision.Models.Scheduler.O6thCommandMapping
{
    using System;
    using System.Collections.Generic;
    using ModelOfSchedulerO4thCommand = Assets.Scripts.Vision.Models.Scheduler.O4thCommands;
    using ModelOfThinkingEngineCommand = Assets.Scripts.ThinkingEngine.Models.Commands;

    /// <summary>
    /// 思考エンジンのコマンドと、画面のコマンドが１対１対応
    /// </summary>
    internal class Model
    {
        static Model()
        {
            SourceCodes.Add(typeof(ModelOfThinkingEngineCommand.MoveCardsToHandFromPile).GetHashCode(), new ModelOfSchedulerO4thCommand.MoveCardsToHandFromPile());
            SourceCodes.Add(typeof(ModelOfThinkingEngineCommand.MoveCardsToPileFromCenterStacks).GetHashCode(), new ModelOfSchedulerO4thCommand.MoveCardsToPileFromCenterStacks());
            SourceCodes.Add(typeof(ModelOfThinkingEngineCommand.MoveCardToCenterStackFromHand).GetHashCode(), new ModelOfSchedulerO4thCommand.MoveCardToCenterStackFromHand());
            SourceCodes.Add(typeof(ModelOfThinkingEngineCommand.MoveFocusToNextCard).GetHashCode(), new ModelOfSchedulerO4thCommand.MoveFocusToNextCard());
            SourceCodes.Add(typeof(ModelOfThinkingEngineCommand.SetGameActive).GetHashCode(), new ModelOfSchedulerO4thCommand.SetGameActive());
            SourceCodes.Add(typeof(ModelOfThinkingEngineCommand.SetIdling).GetHashCode(), new ModelOfSchedulerO4thCommand.SetIdling());
        }

        // - プロパティ

        static Dictionary<int, ModelOfSchedulerO4thCommand.IModel> SourceCodes = new();

        // - メソッド

        internal static ModelOfSchedulerO4thCommand.IModel NewSourceCodeFromModel(Type type)
        {
            return SourceCodes[type.GetHashCode()].NewThis();
        }
    }
}
