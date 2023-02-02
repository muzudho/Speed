namespace Assets.Scripts.Views
{
    using Assets.Scripts.Models.Timeline.Spans;

    /// <summary>
    /// モデル側から、ビュー側への注文
    /// </summary>
    internal class CardMovementView
    {

        // - その他（生成）

        public CardMovementView(CardMovementModel cardMovementModel)
        {
            this.Model = cardMovementModel;
        }

        // - プロパティ

        public CardMovementModel Model { get; private set; }
    }
}
