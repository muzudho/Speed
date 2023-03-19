namespace Assets.Scripts.Vision.Models.Scheduler.O6thGameOperationMapping
{
    using Assets.Scripts.ThinkingEngine.Models.CommandArgs;
    using System;
    using System.Collections.Generic;
    using ModelOfSchedulerO4thGameOperation = Assets.Scripts.Vision.Models.Scheduler.O4thGameOperation;

    /// <summary>
    /// マッピング
    /// </summary>
    internal class Model
    {
        static Model()
        {
            GameOperations.Add(typeof(MoveCardsToHandFromPileModel).GetHashCode(), new ModelOfSchedulerO4thGameOperation.MoveCardsToHandFromPileView());
            GameOperations.Add(typeof(MoveCardsToPileFromCenterStacksModel).GetHashCode(), new ModelOfSchedulerO4thGameOperation.MoveCardsToPileFromCenterStacksView());
            GameOperations.Add(typeof(MoveCardToCenterStackFromHandModel).GetHashCode(), new ModelOfSchedulerO4thGameOperation.MoveCardToCenterStackFromHandView());
            GameOperations.Add(typeof(MoveFocusToNextCardModel).GetHashCode(), new ModelOfSchedulerO4thGameOperation.MoveFocusToNextCardView());
            GameOperations.Add(typeof(SetGameActive).GetHashCode(), new ModelOfSchedulerO4thGameOperation.SetGameActiveView());
            GameOperations.Add(typeof(SetIdling).GetHashCode(), new ModelOfSchedulerO4thGameOperation.SetIdlingView());
        }

        // - プロパティ

        static Dictionary<int, ModelOfSchedulerO4thGameOperation.IModel> GameOperations = new();

        // - メソッド

        internal static ModelOfSchedulerO4thGameOperation.IModel NewGameOperationFromModel(Type type)
        {
            return GameOperations[type.GetHashCode()].NewThis();
        }
    }
}
