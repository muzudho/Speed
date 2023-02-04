﻿namespace Assets.Scripts.Views.Timeline
{
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
        /// <param name="getBegin">開始時の位置と回転</param>
        /// <param name="getEnd">終了時の位置と回転</param>
        /// <param name="idOfGameObject">Id</param>
        public MovementViewModel(
            float startSeconds,
            float duration,
            LazyArgs.GetValue<Vector3AndQuaternionLazy> getBegin,
            LazyArgs.GetValue<Vector3AndQuaternionLazy> getEnd,
            IdOfGameObjects idOfGameObject)
        {
            this.StartSeconds = startSeconds;
            this.Duration = duration;
            this.GetBegin = getBegin;
            this.GetEnd = getEnd;
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

        internal LazyArgs.GetValue<Vector3AndQuaternionLazy> GetBegin { get; private set; }

        internal LazyArgs.GetValue<Vector3AndQuaternionLazy> GetEnd { get; private set; }

        internal IdOfGameObjects IdOfGameObject { get; private set; }
    }
}
