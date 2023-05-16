namespace Assets.Scripts.Scheduler.AnalogCommands.O1stTimelineSpan
{
    using Assets.Scripts.Coding;
    using Assets.Scripts.Vision.Models.World;
    using UnityEngine;

    /// <summary>
    /// ゲーム・オブジェクトの動き
    /// 
    /// - Lerpに使うもの
    /// </summary>
    internal class Model : IModel
    {
        // - その他（生成）

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="timeRange">時間範囲</param>
        /// <param name="target">ゲーム・オブジェクトId</param>
        /// <param name="getBegin">開始時の位置と回転</param>
        /// <param name="getEnd">終了時の位置と回転</param>
        /// <param name="onProgressOrNull">（あれば）進行中の処理</param>
        public Model(
            Range timeRange,
            IdOfGameObjects target,
            LazyArgs.GetValue<PositionAndRotationLazy> getBegin,
            LazyArgs.GetValue<PositionAndRotationLazy> getEnd,
            LazyArgs.SetValue<float> onProgressOrNull = null)
        {
            this.TimeRangeObj = timeRange;
            this.Target = target;
            this.GetBegin = getBegin;
            this.GetEnd = getEnd;
            this.OnProgressOrNull = onProgressOrNull;
        }

        // - プロパティ

        /// <summary>
        /// ゲーム時間範囲（単位：秒）
        /// </summary>
        public Range TimeRangeObj { get; private set; }

        /// <summary>
        /// 進行中の処理
        /// </summary>
        public LazyArgs.SetValue<float> OnProgressOrNull { get; private set; }

        internal IdOfGameObjects Target { get; private set; }

        internal LazyArgs.GetValue<PositionAndRotationLazy> GetBegin { get; private set; }

        internal LazyArgs.GetValue<PositionAndRotationLazy> GetEnd { get; private set; }

        // - メソッド

        /// <summary>
        /// 持続中
        /// </summary>
        /// <param name="progress">進捗 0.0 ～ 1.0</param>
        public void Lerp(float progress)
        {
            var gameObject = GameObjectStorage.Items[this.Target];

            var begin = this.GetBegin();
            var end = this.GetEnd();

            // ここで座標を更新していくから、
            // もし　自分自身の座標から参照した値を　セットしようとしたら、
            // 累計　になることがあるので　注意
            //
            // 開始地点から 終了地点まで 刻んで動け、という命令をしてるときに
            // 開始地点が 刻々と 進んでいる、ということがないようにすること。
            gameObject.transform.position = Vector3.Lerp(begin.GetPosition(), end.GetPosition(), progress);
            gameObject.transform.rotation = Quaternion.Lerp(begin.GetRotation(), end.GetRotation(), progress);
        }
    }
}
