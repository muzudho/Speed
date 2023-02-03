namespace Assets.Scripts.Views
{
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
    }
}
