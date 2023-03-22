namespace Assets.Scripts.Vision.Models.Scheduler.O6thSourceCodePackage
{
    using Assets.Scripts.ThinkingEngine.Models.CommandParameters;
    using System;
    using System.Collections.Generic;
    using ModelOfSchedulerO4thSourceCode = Assets.Scripts.Vision.Models.Scheduler.O4thSourceCode;

    /// <summary>
    /// マッピング
    /// </summary>
    internal class Model
    {
        static Model()
        {
            SourceCodes.Add(typeof(MoveCardsToHandFromPileModel).GetHashCode(), new ModelOfSchedulerO4thSourceCode.MoveCardsToHandFromPile());
            SourceCodes.Add(typeof(MoveCardsToPileFromCenterStacksModel).GetHashCode(), new ModelOfSchedulerO4thSourceCode.MoveCardsToPileFromCenterStacks());
            SourceCodes.Add(typeof(MoveCardToCenterStackFromHandModel).GetHashCode(), new ModelOfSchedulerO4thSourceCode.MoveCardToCenterStackFromHand());
            SourceCodes.Add(typeof(MoveFocusToNextCardModel).GetHashCode(), new ModelOfSchedulerO4thSourceCode.MoveFocusToNextCard());
            SourceCodes.Add(typeof(SetGameActive).GetHashCode(), new ModelOfSchedulerO4thSourceCode.SetGameActive());
            SourceCodes.Add(typeof(SetIdling).GetHashCode(), new ModelOfSchedulerO4thSourceCode.SetIdling());
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
