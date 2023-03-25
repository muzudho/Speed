namespace Assets.Scripts.Vision.Models.Input.Players
{
    using Assets.Scripts.Coding;
    using Assets.Scripts.ThinkingEngine.Models;

    /// <summary>
    /// 入力の意味
    /// 
    /// - 編集可
    /// - プレイヤー１人分
    /// </summary>
    internal class Meaning
    {
        // - その他

        internal Meaning(
            LazyArgs.GetValue<bool> onMoveCardToCenterStackNearMe,
            LazyArgs.GetValue<bool> onMoveCardToFarCenterStack,
            LazyArgs.GetValue<bool> onPickupCardToForward,
            LazyArgs.GetValue<bool> onPickupCardToBackward,
            LazyArgs.GetValue<bool> onDrawing)
        {
            this.OnMoveCardToCenterStackNearMe = onMoveCardToCenterStackNearMe;
            this.OnMoveCardToFarCenterStack = onMoveCardToFarCenterStack;
            this.OnPickupCardToForward = onPickupCardToForward;
            this.OnPickupCardToBackward = onPickupCardToBackward;
            this.OnDrawing = onDrawing;
        }

        // - プロパティ

        /// <summary>
        /// 自分に近い方の台札へ置く
        /// 
        /// - 編集可
        /// </summary>
        internal bool MoveCardToCenterStackNearMe { get; set; } = false;

        /// <summary>
        /// 自分から遠い方の台札へ置く
        /// 
        /// - 編集可
        /// </summary>
        internal bool MoveCardToFarCenterStack { get; set; } = false;

        /// <summary>
        /// 自分から見て（今ピックアップしているカードの）右隣のカードをピックアップ
        /// 
        /// - 編集可
        /// </summary>
        internal bool PickupCardToForward { get; set; } = false;

        /// <summary>
        /// 自分から見て（今ピックアップしているカードの）左隣のカードをピックアップ
        /// 
        /// - 編集可
        /// </summary>
        internal bool PickupCardToBackward { get; set; } = false;

        /// <summary>
        /// 手札から場札を補充する
        /// 
        /// - 編集可
        /// </summary>
        internal bool Drawing { get; set; } = false;

        /// <summary>
        /// キー入力：自分に近い方の台札へ置く
        /// </summary>
        internal LazyArgs.GetValue<bool> OnMoveCardToCenterStackNearMe { get; private set; }

        /// <summary>
        /// キー入力：自分から遠い方の台札へ置く
        /// </summary>
        internal LazyArgs.GetValue<bool> OnMoveCardToFarCenterStack { get; private set; }

        /// <summary>
        /// キー入力：自分から見て（今ピックアップしているカードの）右隣のカードをピックアップ
        /// </summary>
        internal LazyArgs.GetValue<bool> OnPickupCardToForward { get; private set; }

        /// <summary>
        /// キー入力：自分から見て（今ピックアップしているカードの）左隣のカードをピックアップ
        /// </summary>
        internal LazyArgs.GetValue<bool> OnPickupCardToBackward { get; private set; }

        /// <summary>
        /// キー入力：手札から場札を補充する
        /// </summary>
        internal LazyArgs.GetValue<bool> OnDrawing { get; private set; }

        // - メソッド

        /// <summary>
        /// 解析結果を全部消す
        /// </summary>
        internal void Clear()
        {
            this.MoveCardToCenterStackNearMe = false;
            this.MoveCardToFarCenterStack = false;
            this.PickupCardToForward = false;
            this.PickupCardToBackward = false;
            this.Drawing = false;
        }

        /// <summary>
        /// 解析結果を全部上書きする
        /// </summary>
        internal void Overwrite(
            Player playerObj,
            bool moveCardToCenterStackNearMe,
            bool moveCardToFarCenterStack,
            bool pickupCardToForward,
            bool pickupCardToBackward,
            bool drawing)
        {
            this.MoveCardToCenterStackNearMe = moveCardToCenterStackNearMe;
            this.MoveCardToFarCenterStack = moveCardToFarCenterStack;
            this.PickupCardToForward = pickupCardToForward;
            this.PickupCardToBackward = pickupCardToBackward;
            this.Drawing = drawing;
        }

        /// <summary>
        /// 物理的なキー入力を、意味的に置き換える
        /// </summary>
        /// <param name="playerObj"></param>
        internal void UpdateFromInput()
        {
            this.MoveCardToCenterStackNearMe = this.OnMoveCardToCenterStackNearMe();
            this.MoveCardToFarCenterStack = this.OnMoveCardToFarCenterStack();
            this.PickupCardToForward = this.OnPickupCardToForward();
            this.PickupCardToBackward = this.OnPickupCardToBackward();
            this.Drawing = this.OnDrawing();
        }
    }
}
