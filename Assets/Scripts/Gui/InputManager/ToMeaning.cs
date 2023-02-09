namespace Assets.Scripts.Gui.InputManager
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
        }
    }
}
