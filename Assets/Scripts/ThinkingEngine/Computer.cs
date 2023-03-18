namespace Assets.Scripts.ThinkingEngine
{
    using ModelOfGame = Assets.Scripts.ThinkingEngine.Models.Game;

    /// <summary>
    /// コンピューター・プレイヤー
    /// </summary>
    internal class Computer
    {
        // - その他

        internal Computer(int number)
        {
            this.Number = number;
        }

        // - プロパティ

        /// <summary>
        /// プレイヤー番号
        /// 
        /// - 1プレイヤーなら0
        /// </summary>
        public int Number { get; private set; }

        /// <summary>
        /// 自分に近い方の台札へ置く
        /// </summary>
        internal bool MoveCardToCenterStackNearMe { get; private set; }

        /// <summary>
        /// 自分から遠い方の台札へ置く
        /// </summary>
        internal bool MoveCardToFarCenterStack { get; private set; }

        /// <summary>
        /// 自分から見て（今ピックアップしているカードの）右隣のカードをピックアップ
        /// </summary>
        internal bool PickupCardToForward { get; private set; }

        /// <summary>
        /// 自分から見て（今ピックアップしているカードの）左隣のカードをピックアップ
        /// </summary>
        internal bool PickupCardToBackward { get; private set; }

        /// <summary>
        /// 手札から場札を補充する
        /// </summary>
        internal bool Drawing { get; private set; }

        // - メソッド

        /// <summary>
        /// コンピューター・プレイヤーが思考して、操作を決める
        /// </summary>
        /// <param name="gameModel">現在の局面</param>
        internal void Think(ModelOfGame.Default gameModel)
        {
            // 今回の入力予定
            var moveCardToCenterStackNearMe = false;
            var moveCardToFarCenterStack = false;
            var pickupCardToForward = false;
            var pickupCardToBackward = false;
            var drawing = false;

            // 順繰りにやってるだけ
            if (this.MoveCardToCenterStackNearMe == false && this.MoveCardToFarCenterStack == false && this.PickupCardToForward == false && this.Drawing == false)
            {
                moveCardToCenterStackNearMe = true;
            }
            else if (this.MoveCardToCenterStackNearMe)
            {
                moveCardToCenterStackNearMe = false;
                moveCardToFarCenterStack = true;
            }
            else if (this.MoveCardToFarCenterStack)
            {
                moveCardToFarCenterStack = false;
                pickupCardToForward = true;
            }
            else if (this.PickupCardToForward)
            {
                pickupCardToForward = false;
                drawing = true;
            }
            else if (this.Drawing)
            {
                drawing = false;
                moveCardToCenterStackNearMe = true;
            }

            // 今回の入力
            this.MoveCardToCenterStackNearMe = moveCardToCenterStackNearMe;
            this.MoveCardToFarCenterStack = moveCardToFarCenterStack;
            this.PickupCardToForward = pickupCardToForward;
            this.PickupCardToBackward = pickupCardToBackward;
            this.Drawing = drawing;
        }
    }
}
