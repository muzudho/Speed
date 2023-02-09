namespace Assets.Scripts.Gui.SpanOfLerp.TimedGenerator
{
    using Assets.Scripts.Gui.SpanOfLerp.GeneratorGenerator;
    using Assets.Scripts.ThinkingEngine.Model.CommandArgs;
    using System;
    using System.Collections.Generic;

    internal class Mapping
    {
        static Mapping()
        {
            ViewsFromModel.Add(typeof(MoveCardsToHandFromPileModel).GetHashCode(), new MoveCardsToHandFromPileView());
            ViewsFromModel.Add(typeof(MoveCardsToPileFromCenterStacksModel).GetHashCode(), new MoveCardsToPileFromCenterStacksView());
            ViewsFromModel.Add(typeof(MoveCardToCenterStackFromHandModel).GetHashCode(), new MoveCardToCenterStackFromHandView());
            ViewsFromModel.Add(typeof(MoveFocusToNextCardModel).GetHashCode(), new MoveFocusToNextCardView());
            ViewsFromModel.Add(typeof(SetGameActive).GetHashCode(), new SetGameActiveView());
            ViewsFromModel.Add(typeof(SetIdling).GetHashCode(), new SetIdlingView());
        }

        // - プロパティ

        internal static Dictionary<int, ISpanGenerator> ViewsFromModel = new();

        // - メソッド

        internal static ISpanGenerator SpawnViewFromModel(Type type)
        {
            return ViewsFromModel[type.GetHashCode()].Spawn();
        }
    }
}
