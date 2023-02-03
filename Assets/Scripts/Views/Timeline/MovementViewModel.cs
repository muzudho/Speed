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
        /// <param name="getBeginPosition">開始位置</param>
        /// <param name="getEndPosition">終了位置</param>
        /// <param name="beginRotation">開始回転</param>
        /// <param name="endRotation">終了回転</param>
        /// <param name="idOfGameObject">Id</param>
        public MovementViewModel(
            float startSeconds,
            float duration,
            LazyArgs.GetValue<Vector3> getBeginPosition,
            LazyArgs.GetValue<Vector3> getEndPosition,
            LazyArgs.GetValue<Quaternion> getBeginRotation,
            LazyArgs.GetValue<Quaternion> getEndRotation,
            IdOfGameObjects idOfGameObject)
        {
            this.StartSeconds = startSeconds;
            this.Duration = duration;
            this.GetBeginPosition = getBeginPosition;
            this.GetEndPosition = getEndPosition;
            this.GetBeginRotation = getBeginRotation;
            this.GetEndRotation = getEndRotation;
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

        internal LazyArgs.GetValue<Vector3> GetBeginPosition { get; private set; }
        internal LazyArgs.GetValue<Vector3> GetEndPosition { get; private set; }
        internal LazyArgs.GetValue<Quaternion> GetBeginRotation { get; private set; }
        internal LazyArgs.GetValue<Quaternion> GetEndRotation { get; private set; }
        internal IdOfGameObjects IdOfGameObject { get; private set; }
    }
}
