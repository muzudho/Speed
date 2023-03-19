namespace Assets.Scripts.Vision.Models.Timeline.O6thGameOperationMapping
{
    using Assets.Scripts.ThinkingEngine.Models.CommandArgs;
    using System;
    using System.Collections.Generic;
    using ModelOfTimelineO4thGameOperation = Assets.Scripts.Vision.Models.Timeline.O4thGameOperation;

    /// <summary>
    /// マッピング
    /// </summary>
    internal class Model
    {
        static Model()
        {
            GameOperations.Add(typeof(MoveCardsToHandFromPileModel).GetHashCode(), new ModelOfTimelineO4thGameOperation.MoveCardsToHandFromPileView());
            GameOperations.Add(typeof(MoveCardsToPileFromCenterStacksModel).GetHashCode(), new ModelOfTimelineO4thGameOperation.MoveCardsToPileFromCenterStacksView());
            GameOperations.Add(typeof(MoveCardToCenterStackFromHandModel).GetHashCode(), new ModelOfTimelineO4thGameOperation.MoveCardToCenterStackFromHandView());
            GameOperations.Add(typeof(MoveFocusToNextCardModel).GetHashCode(), new ModelOfTimelineO4thGameOperation.MoveFocusToNextCardView());
            GameOperations.Add(typeof(SetGameActive).GetHashCode(), new ModelOfTimelineO4thGameOperation.SetGameActiveView());
            GameOperations.Add(typeof(SetIdling).GetHashCode(), new ModelOfTimelineO4thGameOperation.SetIdlingView());
        }

        // - プロパティ

        static Dictionary<int, ModelOfTimelineO4thGameOperation.IModel> GameOperations = new();

        // - メソッド

        internal static ModelOfTimelineO4thGameOperation.IModel NewGameOperationFromModel(Type type)
        {
            return GameOperations[type.GetHashCode()].NewThis();
        }
    }
}
