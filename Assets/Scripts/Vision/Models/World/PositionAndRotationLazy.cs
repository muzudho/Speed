namespace Assets.Scripts.Vision.Models.World
{
    using Assets.Scripts.Coding;
    using UnityEngine;

    internal class PositionAndRotationLazy
    {
        // - その他

        internal PositionAndRotationLazy(LazyArgs.GetValue<Vector3> getPosition, LazyArgs.GetValue<Quaternion> getRotation)
        {
            GetPosition = getPosition;
            GetRotation = getRotation;
        }

        // - プロパティ

        internal LazyArgs.GetValue<Vector3> GetPosition { get; private set; }

        internal LazyArgs.GetValue<Quaternion> GetRotation { get; private set; }

    }
}
