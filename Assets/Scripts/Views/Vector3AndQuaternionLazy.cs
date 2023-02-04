namespace Assets.Scripts.Views
{
    using UnityEngine;

    internal class Vector3AndQuaternionLazy
    {
        // - その他

        internal Vector3AndQuaternionLazy(LazyArgs.GetValue<Vector3> getVector3, LazyArgs.GetValue<Quaternion> getQuaternion)
        {
            GetVector3 = getVector3;
            GetQuaternion = getQuaternion;
        }

        // - プロパティ

        internal LazyArgs.GetValue<Vector3> GetVector3 { get; private set; }

        internal LazyArgs.GetValue<Quaternion> GetQuaternion { get; private set; }

    }
}
