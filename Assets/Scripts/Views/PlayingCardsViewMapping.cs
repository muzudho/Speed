namespace Assets.Scripts.Views
{
    using Assets.Scripts.Models;
    using System.Collections.Generic;
    using UnityEngine;

    internal static class PlayingCardsViewMapping
    {
        internal static Dictionary<PlayingCards, GameObject> Mapping { get; private set; } =  new();

        internal static void Add(PlayingCards cardId, GameObject goCard)
        {
            Mapping.Add(cardId, goCard);
        }
    }
}
