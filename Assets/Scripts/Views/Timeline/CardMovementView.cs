namespace Assets.Scripts.Views
{
    using Assets.Scripts.Models.Timeline;
    using Assets.Scripts.Views.Timeline;
    using UnityEngine;

    /// <summary>
    /// モデル側から、ビュー側への注文
    /// </summary>
    internal class CardMovementView
    {

        // - その他（生成）

        public CardMovementView(CardMovementViewModel cardMovementModel)
        {
            this.Model = cardMovementModel;
        }

        // - プロパティ

        public CardMovementViewModel Model { get; private set; }

        // - メソッド

        /// <summary>
        /// 持続中
        /// </summary>
        /// <param name="progress">進捗 0.0 ～ 1.0</param>
        public void Lerp(float progress)
        {
            var gameObject = GameObjectStorage.PlayingCards[this.Model.IdOfCard];

            gameObject.transform.position = Vector3.Lerp(this.Model.BeginPosition, this.Model.EndPosition, progress);
            gameObject.transform.rotation = Quaternion.Lerp(this.Model.BeginRotation, this.Model.EndRotation, progress);
        }
    }
}
