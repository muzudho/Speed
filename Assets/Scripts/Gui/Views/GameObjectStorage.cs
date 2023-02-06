namespace Assets.Scripts.Views
{
    using Assets.Scripts.ThinkingEngine;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// ゲーム・オブジェクトと、その Id の紐づけ
    /// </summary>
    static class GameObjectStorage
    {
        internal static Dictionary<IdOfGameObjects, GameObject> Items { get; private set; } =  new();

        internal static void Add(IdOfGameObjects id, GameObject gameObject)
        {
            Items.Add(id, gameObject);
        }

        internal static Dictionary<IdOfGameObjects, GameObject> CreatePlayingCards()
        {
            var list = new Dictionary<IdOfGameObjects, GameObject>();

            foreach (var item in Items)
            {
                if(IdMapping.TestPlayingCard(item.Key))
                {
                    list.Add(item.Key, item.Value);
                }
            }

            return list;
        }
    }
}
