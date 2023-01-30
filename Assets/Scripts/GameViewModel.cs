namespace Assets.Scripts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    /// <summary>
    /// 西端: -62.0f
    /// 東端: 62.0f
    /// </summary>
    class GameViewModel
    {
        // - 初期化系

        internal void Init()
        {
            // ゲーム開始時、とりあえず、すべてのカードは、いったん右の台札という扱いにする
            const int right = 0;// 台札の右
            // const int left = 1;// 台札の左
            for (int i = 1; i < 14; i++)
            {
                // 右の台札
                this.goCenterStacksCards[right].Add(GameObject.Find($"Clubs {i}"));
                this.goCenterStacksCards[right].Add(GameObject.Find($"Diamonds {i}"));
                this.goCenterStacksCards[right].Add(GameObject.Find($"Hearts {i}"));
                this.goCenterStacksCards[right].Add(GameObject.Find($"Spades {i}"));
            }

            // 右の台札をシャッフル
            this.goCenterStacksCards[right] = this.goCenterStacksCards[right].OrderBy(i => Guid.NewGuid()).ToList();
        }

        // - プロパティー

        /// <summary>
        /// 底端
        /// 
        /// - `0.0f` は盤
        /// </summary>
        internal readonly float minY = 0.5f;

        internal readonly float[] handCardsZ = new[] { -28.0f, 42.0f };

        /// <summary>
        /// 手札
        /// 0: １プレイヤー（黒色）
        /// 1: ２プレイヤー（黒色）
        /// </summary>
        internal List<List<GameObject>> goPlayersPileCards = new() { new(), new() };

        // 手札（プレイヤー側で伏せて積んでる札）
        internal readonly float[] pileCardsX = new[] { 40.0f, -40.0f }; // 端っこは 62.0f, -62.0f
        internal readonly float[] pileCardsY = new[] { 0.5f, 0.5f };
        internal readonly float[] pileCardsZ = new[] { -6.5f, 16.0f };

        /// <summary>
        /// 場札（プレイヤー側でオープンしている札）
        /// 0: １プレイヤー（黒色）
        /// 1: ２プレイヤー（黒色）
        /// </summary>
        internal List<List<GameObject>> goPlayersHandCards = new() { new(), new() };

        /// <summary>
        /// 台札（画面中央に積んでいる札）
        /// 0: 右
        /// 1: 左
        /// </summary>
        internal List<List<GameObject>> goCenterStacksCards = new() { new(), new() };

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
        /// 台札の枚数
        /// </summary>
        /// <param name="place">右:0, 左:1</param>
        internal int GetCenterStackCardsLength(int place)
        {
            return this.goCenterStacksCards[place].Count;
        }
    }
}
