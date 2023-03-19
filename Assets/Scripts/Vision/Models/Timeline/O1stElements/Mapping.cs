namespace Assets.Scripts.Vision.Models.Timeline.O1stElements
{
    using Assets.Scripts.ThinkingEngine.Models.CommandArgs;
    using System;
    using System.Collections.Generic;
    using ModelOfTimelineO3rdBElement = Assets.Scripts.Vision.Models.Timeline.O3rdBElements;

    internal class Mapping
    {
        static Mapping()
        {
            ViewsFromModel.Add(typeof(MoveCardsToHandFromPileModel).GetHashCode(), new ModelOfTimelineO3rdBElement.MoveCardsToHandFromPileView());
            ViewsFromModel.Add(typeof(MoveCardsToPileFromCenterStacksModel).GetHashCode(), new ModelOfTimelineO3rdBElement.MoveCardsToPileFromCenterStacksView());
            ViewsFromModel.Add(typeof(MoveCardToCenterStackFromHandModel).GetHashCode(), new ModelOfTimelineO3rdBElement.MoveCardToCenterStackFromHandView());
            ViewsFromModel.Add(typeof(MoveFocusToNextCardModel).GetHashCode(), new ModelOfTimelineO3rdBElement.MoveFocusToNextCardView());
            ViewsFromModel.Add(typeof(SetGameActive).GetHashCode(), new ModelOfTimelineO3rdBElement.SetGameActiveView());
            ViewsFromModel.Add(typeof(SetIdling).GetHashCode(), new ModelOfTimelineO3rdBElement.SetIdlingView());
        }

        // - プロパティ

        static Dictionary<int, ModelOfTimelineO3rdBElement.ISpanGenerator> ViewsFromModel = new();

        // - メソッド

        internal static ModelOfTimelineO3rdBElement.ISpanGenerator SpawnViewFromModel(Type type)
        {
            return ViewsFromModel[type.GetHashCode()].Spawn();
        }
    }
}
