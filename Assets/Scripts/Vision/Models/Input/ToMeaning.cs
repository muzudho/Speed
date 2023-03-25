namespace Assets.Scripts.Vision.Models.Input
{
    using UnityEngine;
    using ModelOfInputOfPlayer = Assets.Scripts.Vision.Models.Input.Players;

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
        internal ModelOfInputOfPlayer.Meaning[] MeaningOfPlayers { get; private set; } = new[]
        {
            new ModelOfInputOfPlayer.Meaning(
                onMoveCardToCenterStackNearMe: ()=>Input.GetKeyDown(KeyCode.DownArrow),
                onMoveCardToFarCenterStack: ()=>Input.GetKeyDown(KeyCode.UpArrow),
                onPickupCardToForward: ()=>Input.GetKeyDown(KeyCode.RightArrow),
                onPickupCardToBackward: ()=>Input.GetKeyDown(KeyCode.LeftArrow),
                onDrawing: ()=>Input.GetKeyDown(KeyCode.Space)),    // １プレイヤーと、２プレイヤーの２回判定されてしまう

            new ModelOfInputOfPlayer.Meaning(
                onMoveCardToCenterStackNearMe: ()=>Input.GetKeyDown(KeyCode.S),
                onMoveCardToFarCenterStack: ()=>Input.GetKeyDown(KeyCode.W),
                onPickupCardToForward: ()=>Input.GetKeyDown(KeyCode.D),
                onPickupCardToBackward: ()=>Input.GetKeyDown(KeyCode.A),
                onDrawing:()=>Input.GetKeyDown(KeyCode.Space)),     // １プレイヤーと、２プレイヤーの２回判定されてしまう
        };
    }
}
