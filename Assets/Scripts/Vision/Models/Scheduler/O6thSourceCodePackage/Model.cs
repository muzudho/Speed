namespace Assets.Scripts.Vision.Models.Scheduler.O6thSourceCodePackage
{
    using System;
    using System.Collections.Generic;
    using ModelOfSchedulerO4thSourceCode = Assets.Scripts.Vision.Models.Scheduler.O4thSourceCode;
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
            SourceCodes.Add(typeof(ModelOfThinkingEngineCommandParameter.MoveCardsToHandFromPile).GetHashCode(), new ModelOfSchedulerO4thSourceCode.MoveCardsToHandFromPile());
            SourceCodes.Add(typeof(ModelOfThinkingEngineCommandParameter.MoveCardsToPileFromCenterStacks).GetHashCode(), new ModelOfSchedulerO4thSourceCode.MoveCardsToPileFromCenterStacks());
            SourceCodes.Add(typeof(ModelOfThinkingEngineCommandParameter.MoveCardToCenterStackFromHand).GetHashCode(), new ModelOfSchedulerO4thSourceCode.MoveCardToCenterStackFromHand());
            SourceCodes.Add(typeof(ModelOfThinkingEngineCommandParameter.MoveFocusToNextCard).GetHashCode(), new ModelOfSchedulerO4thSourceCode.MoveFocusToNextCard());
            SourceCodes.Add(typeof(ModelOfThinkingEngineCommandParameter.SetGameActive).GetHashCode(), new ModelOfSchedulerO4thSourceCode.SetGameActive());
            SourceCodes.Add(typeof(ModelOfThinkingEngineCommandParameter.SetIdling).GetHashCode(), new ModelOfSchedulerO4thSourceCode.SetIdling());
        }

        // - プロパティ

        static Dictionary<int, ModelOfSchedulerO4thSourceCode.IModel> SourceCodes = new();

        // - メソッド

        internal static ModelOfSchedulerO4thSourceCode.IModel NewSourceCodeFromModel(Type type)
        {
            return SourceCodes[type.GetHashCode()].NewThis();
        }
    }
}
