namespace Assets.Scripts.Simulators.Timeline
{
    using Assets.Scripts.Models;
    using Assets.Scripts.Models.Timeline.Spans;
    using Assets.Scripts.Views;
    using Assets.Scripts.Views.Timeline;
    using Assets.Scripts.Views.Timeline.Spans;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// モデルには時間、空間の概念がないので、
    /// モデルに時間、空間を一意に紐づける働きをします
    /// </summary>
    internal static class Specification
    {
        // - その他

        /// <summary>
        /// 静的初期化
        /// </summary>
        static Specification()
        {
            IdFromPlayingCardToGameObject.Add(IdOfPlayingCards.Clubs1, IdOfGameObjects.Clubs1);
            IdFromPlayingCardToGameObject.Add(IdOfPlayingCards.Clubs2, IdOfGameObjects.Clubs2);
            IdFromPlayingCardToGameObject.Add(IdOfPlayingCards.Clubs3, IdOfGameObjects.Clubs3);
            IdFromPlayingCardToGameObject.Add(IdOfPlayingCards.Clubs4, IdOfGameObjects.Clubs4);
            IdFromPlayingCardToGameObject.Add(IdOfPlayingCards.Clubs5, IdOfGameObjects.Clubs5);
            IdFromPlayingCardToGameObject.Add(IdOfPlayingCards.Clubs6, IdOfGameObjects.Clubs6);
            IdFromPlayingCardToGameObject.Add(IdOfPlayingCards.Clubs7, IdOfGameObjects.Clubs7);
            IdFromPlayingCardToGameObject.Add(IdOfPlayingCards.Clubs8, IdOfGameObjects.Clubs8);
            IdFromPlayingCardToGameObject.Add(IdOfPlayingCards.Clubs9, IdOfGameObjects.Clubs9);
            IdFromPlayingCardToGameObject.Add(IdOfPlayingCards.Clubs10, IdOfGameObjects.Clubs10);
            IdFromPlayingCardToGameObject.Add(IdOfPlayingCards.Clubs11, IdOfGameObjects.Clubs11);
            IdFromPlayingCardToGameObject.Add(IdOfPlayingCards.Clubs12, IdOfGameObjects.Clubs12);
            IdFromPlayingCardToGameObject.Add(IdOfPlayingCards.Clubs13, IdOfGameObjects.Clubs13);

            IdFromPlayingCardToGameObject.Add(IdOfPlayingCards.Diamonds1, IdOfGameObjects.Diamonds1);
            IdFromPlayingCardToGameObject.Add(IdOfPlayingCards.Diamonds2, IdOfGameObjects.Diamonds2);
            IdFromPlayingCardToGameObject.Add(IdOfPlayingCards.Diamonds3, IdOfGameObjects.Diamonds3);
            IdFromPlayingCardToGameObject.Add(IdOfPlayingCards.Diamonds4, IdOfGameObjects.Diamonds4);
            IdFromPlayingCardToGameObject.Add(IdOfPlayingCards.Diamonds5, IdOfGameObjects.Diamonds5);
            IdFromPlayingCardToGameObject.Add(IdOfPlayingCards.Diamonds6, IdOfGameObjects.Diamonds6);
            IdFromPlayingCardToGameObject.Add(IdOfPlayingCards.Diamonds7, IdOfGameObjects.Diamonds7);
            IdFromPlayingCardToGameObject.Add(IdOfPlayingCards.Diamonds8, IdOfGameObjects.Diamonds8);
            IdFromPlayingCardToGameObject.Add(IdOfPlayingCards.Diamonds9, IdOfGameObjects.Diamonds9);
            IdFromPlayingCardToGameObject.Add(IdOfPlayingCards.Diamonds10, IdOfGameObjects.Diamonds10);
            IdFromPlayingCardToGameObject.Add(IdOfPlayingCards.Diamonds11, IdOfGameObjects.Diamonds11);
            IdFromPlayingCardToGameObject.Add(IdOfPlayingCards.Diamonds12, IdOfGameObjects.Diamonds12);
            IdFromPlayingCardToGameObject.Add(IdOfPlayingCards.Diamonds13, IdOfGameObjects.Diamonds13);

            IdFromPlayingCardToGameObject.Add(IdOfPlayingCards.Hearts1, IdOfGameObjects.Hearts1);
            IdFromPlayingCardToGameObject.Add(IdOfPlayingCards.Hearts2, IdOfGameObjects.Hearts2);
            IdFromPlayingCardToGameObject.Add(IdOfPlayingCards.Hearts3, IdOfGameObjects.Hearts3);
            IdFromPlayingCardToGameObject.Add(IdOfPlayingCards.Hearts4, IdOfGameObjects.Hearts4);
            IdFromPlayingCardToGameObject.Add(IdOfPlayingCards.Hearts5, IdOfGameObjects.Hearts5);
            IdFromPlayingCardToGameObject.Add(IdOfPlayingCards.Hearts6, IdOfGameObjects.Hearts6);
            IdFromPlayingCardToGameObject.Add(IdOfPlayingCards.Hearts7, IdOfGameObjects.Hearts7);
            IdFromPlayingCardToGameObject.Add(IdOfPlayingCards.Hearts8, IdOfGameObjects.Hearts8);
            IdFromPlayingCardToGameObject.Add(IdOfPlayingCards.Hearts9, IdOfGameObjects.Hearts9);
            IdFromPlayingCardToGameObject.Add(IdOfPlayingCards.Hearts10, IdOfGameObjects.Hearts10);
            IdFromPlayingCardToGameObject.Add(IdOfPlayingCards.Hearts11, IdOfGameObjects.Hearts11);
            IdFromPlayingCardToGameObject.Add(IdOfPlayingCards.Hearts12, IdOfGameObjects.Hearts12);
            IdFromPlayingCardToGameObject.Add(IdOfPlayingCards.Hearts13, IdOfGameObjects.Hearts13);

            IdFromPlayingCardToGameObject.Add(IdOfPlayingCards.Spades1, IdOfGameObjects.Spades1);
            IdFromPlayingCardToGameObject.Add(IdOfPlayingCards.Spades2, IdOfGameObjects.Spades2);
            IdFromPlayingCardToGameObject.Add(IdOfPlayingCards.Spades3, IdOfGameObjects.Spades3);
            IdFromPlayingCardToGameObject.Add(IdOfPlayingCards.Spades4, IdOfGameObjects.Spades4);
            IdFromPlayingCardToGameObject.Add(IdOfPlayingCards.Spades5, IdOfGameObjects.Spades5);
            IdFromPlayingCardToGameObject.Add(IdOfPlayingCards.Spades6, IdOfGameObjects.Spades6);
            IdFromPlayingCardToGameObject.Add(IdOfPlayingCards.Spades7, IdOfGameObjects.Spades7);
            IdFromPlayingCardToGameObject.Add(IdOfPlayingCards.Spades8, IdOfGameObjects.Spades8);
            IdFromPlayingCardToGameObject.Add(IdOfPlayingCards.Spades9, IdOfGameObjects.Spades9);
            IdFromPlayingCardToGameObject.Add(IdOfPlayingCards.Spades10, IdOfGameObjects.Spades10);
            IdFromPlayingCardToGameObject.Add(IdOfPlayingCards.Spades11, IdOfGameObjects.Spades11);
            IdFromPlayingCardToGameObject.Add(IdOfPlayingCards.Spades12, IdOfGameObjects.Spades12);
            IdFromPlayingCardToGameObject.Add(IdOfPlayingCards.Spades13, IdOfGameObjects.Spades13);

            // 隣の場札をピックアップする秒
            float durationOfMoveFocusToNextCard = 0.15f;

            DurationOfModels.Add(typeof(MoveCardsToHandFromPileModel).GetHashCode(), 0.15f + durationOfMoveFocusToNextCard);
            DurationOfModels.Add(typeof(MoveCardsToPileFromCenterStacksModel).GetHashCode(), 0.3f);
            DurationOfModels.Add(typeof(MoveCardToCenterStackFromHandModel).GetHashCode(), 0.15f + durationOfMoveFocusToNextCard);
            DurationOfModels.Add(typeof(MoveFocusToNextCardModel).GetHashCode(), durationOfMoveFocusToNextCard);

            Views.Add(typeof(MoveCardsToHandFromPileView).GetHashCode(), new MoveCardsToHandFromPileView(null));
            Views.Add(typeof(MoveCardsToPileFromCenterStacksView).GetHashCode(), new MoveCardsToPileFromCenterStacksView(null));
            Views.Add(typeof(MoveCardToCenterStackFromHandView).GetHashCode(), new MoveCardToCenterStackFromHandView(null));
            Views.Add(typeof(MoveFocusToNextCardView).GetHashCode(), new MoveFocusToNextCardView(null));
        }

        // - プロパティ

        internal static Dictionary<int, float> DurationOfModels = new();

        internal static Dictionary<int, ISpanView> Views = new();

        internal static Dictionary<IdOfPlayingCards, IdOfGameObjects> IdFromPlayingCardToGameObject = new();

        // - メソッド

        internal static float GetDurationBy(Type type)
        {
            return DurationOfModels[type.GetHashCode()];
        }

        internal static ISpanView SpawnView(Type type, TimeSpanView timeSpan)
        {
            return Views[type.GetHashCode()].Spawn(timeSpan);
        }
    }
}
