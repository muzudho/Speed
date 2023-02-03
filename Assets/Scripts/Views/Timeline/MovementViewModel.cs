namespace Assets.Scripts.Views.Timeline
{
    using UnityEngine;

    /// <summary>
    /// ゲーム・オブジェクトの動き
    /// 
    /// - Lerpに使うもの
    /// </summary>
    internal class MovementViewModel
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
        /// <param name="idOfGameObject">Id</param>
        public MovementViewModel(
            float startSeconds,
            float duration,
            Vector3 beginPosition,
            Vector3 endPosition,
            Quaternion beginRotation,
            Quaternion endRotation,
            IdOfGameObjects idOfGameObject)
        {
            this.StartSeconds = startSeconds;
            this.Duration = duration;
            this.BeginPosition = beginPosition;
            this.EndPosition = endPosition;
            this.BeginRotation = beginRotation;
            this.EndRotation = endRotation;
            this.IdOfGameObject = idOfGameObject;
        }

        // - プロパティ

        /// <summary>
        /// 開始時間（秒）
        /// </summary>
        public float StartSeconds { get; private set; }

        /// <summary>
        /// 持続時間（秒）
        /// </summary>
        public float Duration { get; private set; }

        public float EndSeconds => StartSeconds + Duration;

        internal Vector3 BeginPosition { get; private set; }
        internal Vector3 EndPosition { get; private set; }
        internal Quaternion BeginRotation { get; private set; }
        internal Quaternion EndRotation { get; private set; }
        internal IdOfGameObjects IdOfGameObject { get; private set; }
    }
}
