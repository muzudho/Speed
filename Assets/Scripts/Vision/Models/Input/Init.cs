namespace Assets.Scripts.Vision.Models.Input
{
    using Assets.Scripts.ThinkingEngine;
    using UnityEngine;
    using ModelOfInput = Assets.Scripts.Vision.Models.Input;
    using ModelOfThinkingEngine = Assets.Scripts.ThinkingEngine.Models;

    /// <summary>
    /// 入力モデル
    /// </summary>
    internal class Init
    {
        // - その他

        internal void CleanUp()
        {
            foreach (var playerObj in Commons.Players)
            {
                this.GetPlayer(playerObj).CleanUp();
            }
        }

        // - プロパティ

        /// <summary>
        /// プレイヤーの入力
        /// 
        /// - プレイヤー別
        /// </summary>
        internal ModelOfInput.Player[] Players { get; private set; } = new ModelOfInput.Player[]
        {
            new ModelOfInput.Player(
                playerIdObj: Commons.Player1,
                nearCenterStackPlace: Commons.RightCenterStack,     // 1Pは右の台札にカードを置ける
                farCenterStackPlace: Commons.LeftCenterStack,       // 1Pは左の台札にカードを置ける
                meaning: new ModelOfInput.Meaning(
                    onMoveCardToCenterStackNearMe: ()=>Input.GetKeyDown(KeyCode.DownArrow),
                    onMoveCardToFarCenterStack: ()=>Input.GetKeyDown(KeyCode.UpArrow),
                    onPickupCardToForward: ()=>Input.GetKeyDown(KeyCode.RightArrow),
                    onPickupCardToBackward: ()=>Input.GetKeyDown(KeyCode.LeftArrow),
                    onDrawing: ()=>Input.GetKeyDown(KeyCode.Return))),

            new ModelOfInput.Player(
                playerIdObj: Commons.Player2,
                nearCenterStackPlace: Commons.LeftCenterStack,      // 2Pは左の台札にカードを置ける
                farCenterStackPlace: Commons.RightCenterStack,      // 2Pは右の台札にカードを置ける
                meaning: new ModelOfInput.Meaning(
                    onMoveCardToCenterStackNearMe: ()=>Input.GetKeyDown(KeyCode.S),
                    onMoveCardToFarCenterStack: ()=>Input.GetKeyDown(KeyCode.W),
                    onPickupCardToForward: ()=>Input.GetKeyDown(KeyCode.D),
                    onPickupCardToBackward: ()=>Input.GetKeyDown(KeyCode.A),
                    onDrawing:()=>Input.GetKeyDown(KeyCode.Space))),
        };

        internal ModelOfInput.Player GetPlayer(ModelOfThinkingEngine.Player playerObj)
        {
            return this.Players[playerObj.AsInt];
        }
    }
}
