namespace Assets.Scripts.Models.Timeline.Spans
{
    using UnityEngine;

    /// <summary>
    /// ゲーム・オブジェクトの動き
    /// 
    /// - Lerpに使うもの
    /// </summary>
    internal class CardMovementModel : AbstractSpan
    {
        // - その他（生成）

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="startSeconds">ゲーム内時間（秒）</param>
        /// <param name="duration">持続時間（秒）</param>
        /// <param name="beginPosition">開始位置</param>
        /// <param name="endPosition">終了位置</param>
        /// <param name="beginRotation">開始回転</param>
        /// <param name="endRotation">終了回転</param>
        /// <param name="gameObject">ゲーム・オブジェクト</param>
        public CardMovementModel(
            float startSeconds,
            float duration,
            Vector3 beginPosition,
            Vector3 endPosition,
            Quaternion beginRotation,
            Quaternion endRotation,
            GameObject gameObject)
            : base(startSeconds, duration)
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

        // - メソッド

        /// <summary>
        /// 持続中
        /// </summary>
        /// <param name="progress">進捗 0.0 ～ 1.0</param>
        public override void Lerp(float progress)
        {
            this.GameObject.transform.position = Vector3.Lerp(this.BeginPosition, this.EndPosition, progress);
            this.GameObject.transform.rotation = Quaternion.Lerp(this.BeginRotation, this.EndRotation, progress);
        }
    }
}
