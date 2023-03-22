namespace Assets.Scripts.Vision.Models.Scheduler.O6thSourceCodePackage
{
    using System;
    using System.Collections.Generic;
    using ModelOfSchedulerO4thCommandParameter = Assets.Scripts.Vision.Models.Scheduler.O4thCommandParameters;
    using ModelOfThinkingEngineCommandParameter = Assets.Scripts.ThinkingEngine.Models.CommandParameters;

    /// <summary>
    /// マッピング
    /// 
    /// TODO パラメーターと、ソースコードが、１対１対応している
    /// </summary>
    internal class Model
    {
        static Model()
        {
            SourceCodes.Add(typeof(ModelOfThinkingEngineCommandParameter.MoveCardsToHandFromPile).GetHashCode(), new ModelOfSchedulerO4thCommandParameter.MoveCardsToHandFromPile());
            SourceCodes.Add(typeof(ModelOfThinkingEngineCommandParameter.MoveCardsToPileFromCenterStacks).GetHashCode(), new ModelOfSchedulerO4thCommandParameter.MoveCardsToPileFromCenterStacks());
            SourceCodes.Add(typeof(ModelOfThinkingEngineCommandParameter.MoveCardToCenterStackFromHand).GetHashCode(), new ModelOfSchedulerO4thCommandParameter.MoveCardToCenterStackFromHand());
            SourceCodes.Add(typeof(ModelOfThinkingEngineCommandParameter.MoveFocusToNextCard).GetHashCode(), new ModelOfSchedulerO4thCommandParameter.MoveFocusToNextCard());
            SourceCodes.Add(typeof(ModelOfThinkingEngineCommandParameter.SetGameActive).GetHashCode(), new ModelOfSchedulerO4thCommandParameter.SetGameActive());
            SourceCodes.Add(typeof(ModelOfThinkingEngineCommandParameter.SetIdling).GetHashCode(), new ModelOfSchedulerO4thCommandParameter.SetIdling());
        }

        // - プロパティ

        static Dictionary<int, ModelOfSchedulerO4thCommandParameter.IModel> SourceCodes = new();

        // - メソッド

        internal static ModelOfSchedulerO4thCommandParameter.IModel NewSourceCodeFromModel(Type type)
        {
            return SourceCodes[type.GetHashCode()].NewThis();
        }
    }
}
