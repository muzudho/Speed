namespace Assets.Scripts.Vision.Input
{
    using Assets.Scripts.ThinkingEngine;
    using Assets.Scripts.ThinkingEngine.Models;
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
        /// <param name="playerObj"></param>
        internal void UpdateFromInput(Player playerObj)
        {
            if (playerObj == Commons.Player1)
            {
                MoveCardToCenterStackNearMe[playerObj.AsInt] = Input.GetKeyDown(KeyCode.DownArrow);
                MoveCardToFarCenterStack[playerObj.AsInt] = Input.GetKeyDown(KeyCode.UpArrow);
                PickupCardToForward[playerObj.AsInt] = Input.GetKeyDown(KeyCode.RightArrow);
                PickupCardToBackward[playerObj.AsInt] = Input.GetKeyDown(KeyCode.LeftArrow);
            }
            else
            {
                MoveCardToCenterStackNearMe[playerObj.AsInt] = Input.GetKeyDown(KeyCode.S);
                MoveCardToFarCenterStack[playerObj.AsInt] = Input.GetKeyDown(KeyCode.W);
                PickupCardToForward[playerObj.AsInt] = Input.GetKeyDown(KeyCode.D);
                PickupCardToBackward[playerObj.AsInt] = Input.GetKeyDown(KeyCode.A);
            }

            Drawing = Input.GetKeyDown(KeyCode.Space); // １プレイヤーと、２プレイヤーの２回判定されてしまう
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
            MoveCardToCenterStackNearMe[playerObj.AsInt] = moveCardToCenterStackNearMe;
            MoveCardToFarCenterStack[playerObj.AsInt] = moveCardToFarCenterStack;
            PickupCardToForward[playerObj.AsInt] = pickupCardToForward;
            PickupCardToBackward[playerObj.AsInt] = pickupCardToBackward;
            Drawing = drawing;
        }
    }
}
