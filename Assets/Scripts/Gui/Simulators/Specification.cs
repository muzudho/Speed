﻿namespace Assets.Scripts.Simulators
{
    using Assets.Scripts.Engine.CommandArgs;
    using Assets.Scripts.Gui.Models;
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
            Bind(IdOfPlayingCards.Clubs1, IdOfGameObjects.Clubs1);
            Bind(IdOfPlayingCards.Clubs2, IdOfGameObjects.Clubs2);
            Bind(IdOfPlayingCards.Clubs3, IdOfGameObjects.Clubs3);
            Bind(IdOfPlayingCards.Clubs4, IdOfGameObjects.Clubs4);
            Bind(IdOfPlayingCards.Clubs5, IdOfGameObjects.Clubs5);
            Bind(IdOfPlayingCards.Clubs6, IdOfGameObjects.Clubs6);
            Bind(IdOfPlayingCards.Clubs7, IdOfGameObjects.Clubs7);
            Bind(IdOfPlayingCards.Clubs8, IdOfGameObjects.Clubs8);
            Bind(IdOfPlayingCards.Clubs9, IdOfGameObjects.Clubs9);
            Bind(IdOfPlayingCards.Clubs10, IdOfGameObjects.Clubs10);
            Bind(IdOfPlayingCards.Clubs11, IdOfGameObjects.Clubs11);
            Bind(IdOfPlayingCards.Clubs12, IdOfGameObjects.Clubs12);
            Bind(IdOfPlayingCards.Clubs13, IdOfGameObjects.Clubs13);

            Bind(IdOfPlayingCards.Diamonds1, IdOfGameObjects.Diamonds1);
            Bind(IdOfPlayingCards.Diamonds2, IdOfGameObjects.Diamonds2);
            Bind(IdOfPlayingCards.Diamonds3, IdOfGameObjects.Diamonds3);
            Bind(IdOfPlayingCards.Diamonds4, IdOfGameObjects.Diamonds4);
            Bind(IdOfPlayingCards.Diamonds5, IdOfGameObjects.Diamonds5);
            Bind(IdOfPlayingCards.Diamonds6, IdOfGameObjects.Diamonds6);
            Bind(IdOfPlayingCards.Diamonds7, IdOfGameObjects.Diamonds7);
            Bind(IdOfPlayingCards.Diamonds8, IdOfGameObjects.Diamonds8);
            Bind(IdOfPlayingCards.Diamonds9, IdOfGameObjects.Diamonds9);
            Bind(IdOfPlayingCards.Diamonds10, IdOfGameObjects.Diamonds10);
            Bind(IdOfPlayingCards.Diamonds11, IdOfGameObjects.Diamonds11);
            Bind(IdOfPlayingCards.Diamonds12, IdOfGameObjects.Diamonds12);
            Bind(IdOfPlayingCards.Diamonds13, IdOfGameObjects.Diamonds13);

            Bind(IdOfPlayingCards.Hearts1, IdOfGameObjects.Hearts1);
            Bind(IdOfPlayingCards.Hearts2, IdOfGameObjects.Hearts2);
            Bind(IdOfPlayingCards.Hearts3, IdOfGameObjects.Hearts3);
            Bind(IdOfPlayingCards.Hearts4, IdOfGameObjects.Hearts4);
            Bind(IdOfPlayingCards.Hearts5, IdOfGameObjects.Hearts5);
            Bind(IdOfPlayingCards.Hearts6, IdOfGameObjects.Hearts6);
            Bind(IdOfPlayingCards.Hearts7, IdOfGameObjects.Hearts7);
            Bind(IdOfPlayingCards.Hearts8, IdOfGameObjects.Hearts8);
            Bind(IdOfPlayingCards.Hearts9, IdOfGameObjects.Hearts9);
            Bind(IdOfPlayingCards.Hearts10, IdOfGameObjects.Hearts10);
            Bind(IdOfPlayingCards.Hearts11, IdOfGameObjects.Hearts11);
            Bind(IdOfPlayingCards.Hearts12, IdOfGameObjects.Hearts12);
            Bind(IdOfPlayingCards.Hearts13, IdOfGameObjects.Hearts13);

            Bind(IdOfPlayingCards.Spades1, IdOfGameObjects.Spades1);
            Bind(IdOfPlayingCards.Spades2, IdOfGameObjects.Spades2);
            Bind(IdOfPlayingCards.Spades3, IdOfGameObjects.Spades3);
            Bind(IdOfPlayingCards.Spades4, IdOfGameObjects.Spades4);
            Bind(IdOfPlayingCards.Spades5, IdOfGameObjects.Spades5);
            Bind(IdOfPlayingCards.Spades6, IdOfGameObjects.Spades6);
            Bind(IdOfPlayingCards.Spades7, IdOfGameObjects.Spades7);
            Bind(IdOfPlayingCards.Spades8, IdOfGameObjects.Spades8);
            Bind(IdOfPlayingCards.Spades9, IdOfGameObjects.Spades9);
            Bind(IdOfPlayingCards.Spades10, IdOfGameObjects.Spades10);
            Bind(IdOfPlayingCards.Spades11, IdOfGameObjects.Spades11);
            Bind(IdOfPlayingCards.Spades12, IdOfGameObjects.Spades12);
            Bind(IdOfPlayingCards.Spades13, IdOfGameObjects.Spades13);

            // 隣の場札をピックアップする秒
            float durationOfMoveFocusToNextCard = 0.15f;

            DurationOfModels.Add(typeof(MoveCardsToHandFromPileModel).GetHashCode(), 0.15f + durationOfMoveFocusToNextCard);
            DurationOfModels.Add(typeof(MoveCardsToPileFromCenterStacksModel).GetHashCode(), 0.3f);
            DurationOfModels.Add(typeof(MoveCardToCenterStackFromHandModel).GetHashCode(), 0.15f + durationOfMoveFocusToNextCard);
            DurationOfModels.Add(typeof(MoveFocusToNextCardModel).GetHashCode(), durationOfMoveFocusToNextCard);

            ViewsFromModel.Add(typeof(MoveCardsToHandFromPileModel).GetHashCode(), new MoveCardsToHandFromPileView());
            ViewsFromModel.Add(typeof(MoveCardsToPileFromCenterStacksModel).GetHashCode(), new MoveCardsToPileFromCenterStacksView());
            ViewsFromModel.Add(typeof(MoveCardToCenterStackFromHandModel).GetHashCode(), new MoveCardToCenterStackFromHandView());
            ViewsFromModel.Add(typeof(MoveFocusToNextCardModel).GetHashCode(), new MoveFocusToNextCardView());
        }

        // - プロパティ

        internal static Dictionary<int, float> DurationOfModels = new();

        internal static Dictionary<int, ISpanView> ViewsFromModel = new();

        static Dictionary<IdOfPlayingCards, IdOfGameObjects> IdFromPlayingCardToGameObject = new();
        static Dictionary<IdOfGameObjects, IdOfPlayingCards> IdFromGameObjectToPlayingCard = new();

        // - メソッド

        static void Bind(IdOfPlayingCards idOfCard, IdOfGameObjects idOfGo)
        {
            IdFromPlayingCardToGameObject.Add(idOfCard, idOfGo);
            IdFromGameObjectToPlayingCard.Add(idOfGo, idOfCard);
        }

        internal static float GetDurationBy(Type type)
        {
            return DurationOfModels[type.GetHashCode()];
        }

        internal static ISpanView SpawnViewFromModel(Type type)
        {
            return ViewsFromModel[type.GetHashCode()].Spawn();
        }

        internal static IdOfGameObjects GetIdOfGameObject(IdOfPlayingCards id)
        {
            return IdFromPlayingCardToGameObject[id];
        }

        internal static IdOfPlayingCards GetIdOfPlayingCard(IdOfGameObjects id)
        {
            return IdFromGameObjectToPlayingCard[id];
        }

        internal static bool TestPlayingCard(IdOfGameObjects id)
        {
            return IdFromGameObjectToPlayingCard.ContainsKey(id);
        }
    }
}