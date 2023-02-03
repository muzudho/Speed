namespace Assets.Scripts.Views
{
    using Assets.Scripts.Models;
    using Assets.Scripts.Models.Timeline;
    using Assets.Scripts.Simulators.Timeline;
    using Assets.Scripts.Views.Timeline;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// 画面表示関連
    /// 
    /// 西端: -62.0f
    /// 東端: 62.0f
    /// </summary>
    public class GameViewModel
    {
        // - プロパティー

        /// <summary>
        /// 底端
        /// 
        /// - `0.0f` は盤
        /// </summary>
        internal readonly float minY = 0.5f;

        internal readonly float[] handCardsZ = new[] { -28.0f, 42.0f };

        // 手札（プレイヤー側で伏せて積んでる札）
        internal readonly float[] pileCardsX = new[] { 40.0f, -40.0f }; // 端っこは 62.0f, -62.0f
        internal readonly float[] pileCardsY = new[] { 0.5f, 0.5f };
        internal readonly float[] pileCardsZ = new[] { -6.5f, 16.0f };

        // 台札
        internal float[] centerStacksX = { 15.0f, -15.0f };

        /// <summary>
        /// 台札のY座標
        /// 
        /// - 右が 0、左が 1
        /// - 0.0f は盤なので、それより上にある
        /// </summary>
        internal float[] centerStacksY = { 0.5f, 0.5f };
        internal float[] centerStacksZ = { 2.5f, 9.0f };

        // - メソッド

        /// <summary>
        /// 台札の次の天辺の位置
        /// </summary>
        /// <param name="place"></param>
        /// <returns></returns>
        internal (float, float) GetXZOfNextCenterStackCard(GameModel gameModel, int place)
        {
            var length = gameModel.GetLengthOfCenterStackCards(place);
            if (length < 1)
            {
                // 床上
                var nextTopX2 = this.centerStacksX[place];
                var nextTopZ2 = this.centerStacksZ[place];
                return (nextTopX2, nextTopZ2);
            }

            // 台札の次の天辺の位置
            var idOfLastCard = gameModel.GetLastCardOfCenterStack(place); // 天辺（最後）のカード
            var goLastCard = GameObjectStorage.Items[Specification.GetIdOfGameObject(idOfLastCard)];
            var nextTopX = (this.centerStacksX[place] - goLastCard.transform.position.x) / 2 + this.centerStacksX[place];
            var nextTopZ = (this.centerStacksZ[place] - goLastCard.transform.position.z) / 2 + this.centerStacksZ[place];
            return (nextTopX, nextTopZ);
        }
    }
}
