namespace Assets.Scripts.Vision.Input
{
    using UnityEngine;

    /// <summary>
    /// キー入力の解析
    /// </summary>
    internal class ToMeaning
    {
        // - プロパティ

        /// <summary>
        /// 自分に近い方の台札へ置く
        /// </summary>
        internal bool[] MoveCardToCenterStackNearMe { get; private set; } = new[] { false, false };

        /// <summary>
        /// 自分から遠い方の台札へ置く
        /// </summary>
        internal bool[] MoveCardToFarCenterStack { get; private set; } = new[] { false, false };

        /// <summary>
        /// 自分から見て（今ピックアップしているカードの）右隣のカードをピックアップ
        /// </summary>
        internal bool[] PickupCardToForward { get; private set; } = new[] { false, false };

        /// <summary>
        /// 自分から見て（今ピックアップしているカードの）左隣のカードをピックアップ
        /// </summary>
        internal bool[] PickupCardToBackward { get; private set; } = new[] { false, false };

        /// <summary>
        /// 手札から場札を補充する
        /// </summary>
        internal bool Drawing { get; private set; } = false;

        // - メソッド

        /// <summary>
        /// 解析結果を全部消す
        /// </summary>
        internal void Clear()
        {
            for (var player = 0; player < 2; player++)
            {
                MoveCardToCenterStackNearMe[player] = false;
                MoveCardToFarCenterStack[player] = false;
                PickupCardToForward[player] = false;
                PickupCardToBackward[player] = false;
            }

            Drawing = false;
        }

        /// <summary>
        /// 物理的なキー入力を、意味的に置き換える
        /// </summary>
        /// <param name="player"></param>
        internal void UpdateFromInput(int player)
        {
            if (player == 0)
            {
                MoveCardToCenterStackNearMe[player] = Input.GetKeyDown(KeyCode.DownArrow);
                MoveCardToFarCenterStack[player] = Input.GetKeyDown(KeyCode.UpArrow);
                PickupCardToForward[player] = Input.GetKeyDown(KeyCode.RightArrow);
                PickupCardToBackward[player] = Input.GetKeyDown(KeyCode.LeftArrow);
            }
            else
            {
                MoveCardToCenterStackNearMe[player] = Input.GetKeyDown(KeyCode.S);
                MoveCardToFarCenterStack[player] = Input.GetKeyDown(KeyCode.W);
                PickupCardToForward[player] = Input.GetKeyDown(KeyCode.D);
                PickupCardToBackward[player] = Input.GetKeyDown(KeyCode.A);
            }

            Drawing = Input.GetKeyDown(KeyCode.Space); // １プレイヤーと、２プレイヤーの２回判定されてしまう
        }

        /// <summary>
        /// 解析結果を全部上書きする
        /// </summary>
        internal void Overwrite(
            int player,
            bool moveCardToCenterStackNearMe,
            bool moveCardToFarCenterStack,
            bool pickupCardToForward,
            bool pickupCardToBackward,
            bool drawing)
        {
            MoveCardToCenterStackNearMe[player] = moveCardToCenterStackNearMe;
            MoveCardToFarCenterStack[player] = moveCardToFarCenterStack;
            PickupCardToForward[player] = pickupCardToForward;
            PickupCardToBackward[player] = pickupCardToBackward;
            Drawing = drawing;
        }
    }
}
