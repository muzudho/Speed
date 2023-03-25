namespace Assets.Scripts.Vision.Models.Input
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
        /// 入力の意味
        /// 
        /// - プレイヤー別
        /// </summary>
        internal MeaningOfPlayer[] MeaningOfPlayers { get; private set; } = new[]
        {
            new MeaningOfPlayer(
                onMoveCardToCenterStackNearMe: ()=>Input.GetKeyDown(KeyCode.DownArrow),
                onMoveCardToFarCenterStack: ()=>Input.GetKeyDown(KeyCode.UpArrow),
                onPickupCardToForward: ()=>Input.GetKeyDown(KeyCode.RightArrow),
                onPickupCardToBackward: ()=>Input.GetKeyDown(KeyCode.LeftArrow)),

            new MeaningOfPlayer(
                onMoveCardToCenterStackNearMe: ()=>Input.GetKeyDown(KeyCode.S),
                onMoveCardToFarCenterStack: ()=>Input.GetKeyDown(KeyCode.W),
                onPickupCardToForward: ()=>Input.GetKeyDown(KeyCode.D),
                onPickupCardToBackward: ()=>Input.GetKeyDown(KeyCode.A)),
        };

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
                this.MeaningOfPlayers[player].MoveCardToCenterStackNearMe = false;
                this.MeaningOfPlayers[player].MoveCardToFarCenterStack = false;
                this.MeaningOfPlayers[player].PickupCardToForward = false;
                this.MeaningOfPlayers[player].PickupCardToBackward = false;
            }

            Drawing = false;
        }

        /// <summary>
        /// 物理的なキー入力を、意味的に置き換える
        /// </summary>
        /// <param name="playerObj"></param>
        internal void UpdateFromInput(Player playerObj)
        {
            this.MeaningOfPlayers[playerObj.AsInt].MoveCardToCenterStackNearMe = this.MeaningOfPlayers[playerObj.AsInt].OnMoveCardToCenterStackNearMe();
            this.MeaningOfPlayers[playerObj.AsInt].MoveCardToFarCenterStack = this.MeaningOfPlayers[playerObj.AsInt].OnMoveCardToFarCenterStack();
            this.MeaningOfPlayers[playerObj.AsInt].PickupCardToForward = this.MeaningOfPlayers[playerObj.AsInt].OnPickupCardToForward();
            this.MeaningOfPlayers[playerObj.AsInt].PickupCardToBackward = this.MeaningOfPlayers[playerObj.AsInt].OnPickupCardToBackward();

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
            this.MeaningOfPlayers[playerObj.AsInt].MoveCardToCenterStackNearMe = moveCardToCenterStackNearMe;
            this.MeaningOfPlayers[playerObj.AsInt].MoveCardToFarCenterStack = moveCardToFarCenterStack;
            this.MeaningOfPlayers[playerObj.AsInt].PickupCardToForward = pickupCardToForward;
            this.MeaningOfPlayers[playerObj.AsInt].PickupCardToBackward = pickupCardToBackward;
            Drawing = drawing;
        }
    }
}
