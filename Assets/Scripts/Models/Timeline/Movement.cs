﻿namespace Assets.Scripts.Models.Timeline
{
    using UnityEngine;

    /// <summary>
    /// ゲーム・オブジェクトの動き
    /// 
    /// - Lerpに使うもの
    /// </summary>
    internal class Movement
    {
        // - その他（生成）

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="beginPosition">開始位置</param>
        /// <param name="endPosition">終了位置</param>
        /// <param name="beginRotation">開始回転</param>
        /// <param name="endRotation">終了回転</param>
        /// <param name="gameObject">ゲーム・オブジェクト</param>
        public Movement(
            Vector3 beginPosition,
            Vector3 endPosition,
            Quaternion beginRotation,
            Quaternion endRotation,
            GameObject gameObject)
        {
            this.BeginPosition = beginPosition;
            this.EndPosition = endPosition;
            this.BeginRotation = beginRotation;
            this.EndRotation = endRotation;
            this.GameObject = gameObject;
        }

        // - プロパティ

        internal Vector3 BeginPosition { get; private set; }
        internal Vector3 EndPosition { get; private set; }
        internal Quaternion BeginRotation { get; private set; }
        internal Quaternion EndRotation { get; private set; }
        internal GameObject GameObject { get; private set; }
    }
}
