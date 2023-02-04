namespace Assets.Scripts.Views
{
    using Assets.Scripts.Views.Timeline;
    using UnityEngine;

    /// <summary>
    /// モデル側から、ビュー側への注文
    /// </summary>
    internal class MovementView
    {

        // - その他（生成）

        public MovementView(MovementViewModel movementViewModel)
        {
            this.Model = movementViewModel;
        }

        // - プロパティ

        public MovementViewModel Model { get; private set; }

        // - メソッド

        /// <summary>
        /// 持続中
        /// </summary>
        /// <param name="progress">進捗 0.0 ～ 1.0</param>
        public void Lerp(float progress)
        {
            var gameObject = GameObjectStorage.Items[this.Model.IdOfGameObject];

            gameObject.transform.position = Vector3.Lerp(this.Model.GetBegin().GetVector3(), this.Model.GetEnd().GetVector3(), progress);
            gameObject.transform.rotation = Quaternion.Lerp(this.Model.GetBegin().GetQuaternion(), this.Model.GetEnd().GetQuaternion(), progress);
        }
    }
}
