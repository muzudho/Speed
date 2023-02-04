namespace Assets.Scripts.Models
{
    using System;
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

        public Vector3Immutable(Vector3 source)
        {
            this.me = new Vector3(source.x, source.y, source.z);
        }

        // - フィールド

        Vector3 me;

        // - プロパティ

        public float X => me.x;

        public float Y => me.y;

        public float Z => me.z;

        // - メソッド

        public Vector3 Add(Vector3 adds)
        {
            return me + adds;
        }

        public Vector3 ToMutable()
        {
            return new Vector3(me.x, me.y, me.z);
        }
    }
}
