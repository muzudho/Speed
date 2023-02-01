namespace Assets.Scripts.Views
{
    using Assets.Scripts.Models;
    using System.Collections.Generic;
    using UnityEngine;

    static class GameObjectStorage
    {
        internal static Dictionary<IdOfPlayingCards, GameObject> PlayingCards { get; private set; } =  new();

        internal static void Add(IdOfPlayingCards cardId, GameObject goCard)
        {
            PlayingCards.Add(cardId, goCard);
        }
    }
}
