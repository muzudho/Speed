using Assets.Scripts.Coding;

namespace Assets.Scripts.Vision.Models.Input
{
    /// <summary>
    /// 入力の意味
    /// 
    /// - 編集可
    /// </summary>
    internal class MeaningOfPlayer
    {
        // - その他

        internal MeaningOfPlayer(
            LazyArgs.GetValue<bool> onMoveCardToCenterStackNearMe,
            LazyArgs.GetValue<bool> onMoveCardToFarCenterStack,
            LazyArgs.GetValue<bool> onPickupCardToForward,
            LazyArgs.GetValue<bool> onPickupCardToBackward)
        {
            this.OnMoveCardToCenterStackNearMe = onMoveCardToCenterStackNearMe;
            this.OnMoveCardToFarCenterStack = onMoveCardToFarCenterStack;
            this.OnPickupCardToForward = onPickupCardToForward;
            this.OnPickupCardToBackward = onPickupCardToBackward;
        }

        // - プロパティ

        internal LazyArgs.GetValue<bool> OnMoveCardToCenterStackNearMe { get; private set; }

        internal LazyArgs.GetValue<bool> OnMoveCardToFarCenterStack { get; private set; }

        internal LazyArgs.GetValue<bool> OnPickupCardToForward { get; private set; }

        internal LazyArgs.GetValue<bool> OnPickupCardToBackward { get; private set; }
        


        /// <summary>
        /// 自分に近い方の台札へ置く
        /// 
        /// - 編集可
        /// </summary>
        internal bool MoveCardToCenterStackNearMe { get; set; } = false;

        /// <summary>
        /// 自分から遠い方の台札へ置く
        /// </summary>
        internal bool MoveCardToFarCenterStack { get; set; } = false;

        /// <summary>
        /// 自分から見て（今ピックアップしているカードの）右隣のカードをピックアップ
        /// </summary>
        internal bool PickupCardToForward { get; set; } = false;

        /// <summary>
        /// 自分から見て（今ピックアップしているカードの）左隣のカードをピックアップ
        /// </summary>
        internal bool PickupCardToBackward { get; set; } = false;
    }
}
