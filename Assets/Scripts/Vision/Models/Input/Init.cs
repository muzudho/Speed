﻿namespace Assets.Scripts.Vision.Models.Input
{
    using Assets.Scripts.ThinkingEngine;
    using UnityEngine;
    using ModelOfInput = Assets.Scripts.Vision.Models.Input;

    /// <summary>
    /// 入力モデル
    /// </summary>
    internal class Init
    {
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
                onDrawing: ()=>Input.GetKeyDown(KeyCode.Space))),       // １プレイヤーと、２プレイヤーの２回判定されてしまう

            new ModelOfInput.Player(
                playerIdObj: Commons.Player2,
                nearCenterStackPlace: Commons.LeftCenterStack,      // 2Pは左の台札にカードを置ける
                farCenterStackPlace: Commons.RightCenterStack,      // 2Pは右の台札にカードを置ける
                meaning: new ModelOfInput.Meaning(
                onMoveCardToCenterStackNearMe: ()=>Input.GetKeyDown(KeyCode.S),
                onMoveCardToFarCenterStack: ()=>Input.GetKeyDown(KeyCode.W),
                onPickupCardToForward: ()=>Input.GetKeyDown(KeyCode.D),
                onPickupCardToBackward: ()=>Input.GetKeyDown(KeyCode.A),
                onDrawing:()=>Input.GetKeyDown(KeyCode.Space))),        // １プレイヤーと、２プレイヤーの２回判定されてしまう
        };
    }
}