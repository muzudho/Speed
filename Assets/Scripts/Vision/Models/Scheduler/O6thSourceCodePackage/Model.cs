namespace Assets.Scripts.Vision.Models.Scheduler.O6thSourceCodePackage
{
    using Assets.Scripts.ThinkingEngine.Models.CommandArgs;
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
            SourceCodes.Add(typeof(MoveCardsToHandFromPileModel).GetHashCode(), new ModelOfSchedulerO4thSourceCode.MoveCardsToHandFromPileView());
            SourceCodes.Add(typeof(MoveCardsToPileFromCenterStacksModel).GetHashCode(), new ModelOfSchedulerO4thSourceCode.MoveCardsToPileFromCenterStacksView());
            SourceCodes.Add(typeof(MoveCardToCenterStackFromHandModel).GetHashCode(), new ModelOfSchedulerO4thSourceCode.MoveCardToCenterStackFromHandView());
            SourceCodes.Add(typeof(MoveFocusToNextCardModel).GetHashCode(), new ModelOfSchedulerO4thSourceCode.MoveFocusToNextCardView());
            SourceCodes.Add(typeof(SetGameActive).GetHashCode(), new ModelOfSchedulerO4thSourceCode.SetGameActiveView());
            SourceCodes.Add(typeof(SetIdling).GetHashCode(), new ModelOfSchedulerO4thSourceCode.SetIdlingView());
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
