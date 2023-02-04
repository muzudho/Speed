namespace Assets.Scripts.Models
{
    using UnityEngine;

    /// <summary>
    /// 読取専用
    /// </summary>
    internal class Vector3Immutable
    {
        // - その他

        public Vector3Immutable(float x, float y, float z)
        {
            this.me = new Vector3(x, y, z);
        }

        Vector3 me;

        public Vector3 Add(Vector3 adds)
        {
            return me + adds;
        }
    }
}
