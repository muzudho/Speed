namespace Assets.Scripts.Gui.Models
{
    using UnityEngine;

    /// <summary>
    /// 読取専用
    /// </summary>
    internal class QuaternionImmutable
    {
        // - その他

        public static QuaternionImmutable Euler(float x, float y, float z)
        {
            return new QuaternionImmutable(Quaternion.Euler(x, y, z));
        }

        public QuaternionImmutable(Quaternion source)
        {
            this.me = new Quaternion(source.x, source.y, source.z, source.w);
        }

        // - フィールド

        Quaternion me;

        // - プロパティ

        public float EulerAnglesX => me.eulerAngles.x;

        public float EulerAnglesY => me.eulerAngles.y;

        public float EulerAnglesZ => me.eulerAngles.z;

        // - メソッド

        public Quaternion ToMutable()
        {
            return new Quaternion(me.x, me.y, me.z, me.w);
        }
    }
}
