namespace Assets.Scripts.Vision.World.Views
{
    using UnityEngine;

    internal class PositionAndRotation
    {
        // - その他

        internal PositionAndRotation(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
        }

        // - プロパティ

        internal Vector3 Position { get; private set; }

        internal Quaternion Rotation { get; private set; }

    }
}
