namespace Assets.Scripts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UIElements;

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
        internal int GetLengthOfCenterStackCards(int place)
        {
            return this.goCenterStacksCards[place].Count;
        }

        internal GameObject GetCardOfCenterStack(int place, int startIndex)
        {
            return this.goCenterStacksCards[place].ElementAt(startIndex);
        }

        /// <summary>
        /// 天辺の台札
        /// </summary>
        /// <param name="place"></param>
        /// <returns></returns>
        internal GameObject GetLastCardOfCenterStack(int place)
        {
            var length = this.GetLengthOfCenterStackCards(place);
            return this.GetCardOfCenterStack(place, length - 1); // 最後のカード
        }

        internal void RemoveCardAtOfCenterStack(int place, int startIndex)
        {
            this.goCenterStacksCards[place].RemoveAt(startIndex);
        }

        internal void AddCardOfCenterStack(int place, GameObject goCard)
        {
            this.goCenterStacksCards[place].Add(goCard);
        }

        /// <summary>
        /// 台札の次の天辺の位置
        /// </summary>
        /// <param name="place"></param>
        /// <returns></returns>
        internal (float, float) GetXZOfNextCenterStackCard(int place)
        {
            var length = this.GetLengthOfCenterStackCards(place);
            if (length < 1)
            {
                // 床上
                var nextTopX2 = this.centerStacksX[place];
                var nextTopZ2 = this.centerStacksZ[place];
                return (nextTopX2, nextTopZ2);
            }

            // 台札の次の天辺の位置
            var goLastCard = this.GetLastCardOfCenterStack(place); // 天辺（最後）のカード
            var nextTopX = (this.centerStacksX[place] - goLastCard.transform.position.x) / 2 + this.centerStacksX[place];
            var nextTopZ = (this.centerStacksZ[place] - goLastCard.transform.position.z) / 2 + this.centerStacksZ[place];
            return (nextTopX, nextTopZ);
        }

        /// <summary>
        /// 手札を追加
        /// </summary>
        /// <param name="player"></param>
        /// <param name="goCard"></param>
        internal void AddCardOfPlayersPile(int player, GameObject goCard)
        {
            this.goPlayersPileCards[player].Add(goCard);
        }

        /// <summary>
        /// 手札の枚数
        /// </summary>
        /// <returns></returns>
        internal int GetLengthOfPlayerPileCards(int player)
        {
            return this.goPlayersPileCards[player].Count;
        }

        internal List<GameObject> GetRangeCardsOfPlayerPile(int player, int startIndex, int numberOfCards)
        {
            return this.goPlayersPileCards[player].GetRange(startIndex, numberOfCards);
        }

        internal void RemoveRangeCardsOfPlayerPile(int player, int startIndex, int numberOfCards)
        {
            this.goPlayersPileCards[player].RemoveRange(startIndex, numberOfCards);
        }

        /// <summary>
        /// 場札を追加
        /// </summary>
        /// <param name="player"></param>
        /// <param name="goCards"></param>
        internal void AddRangeCardsOfPlayerHand(int player, List<GameObject> goCards)
        {
            this.goPlayersHandCards[player].AddRange(goCards);
        }

        /// <summary>
        /// 場札の枚数
        /// </summary>
        /// <returns></returns>
        internal int GetLengthOfPlayerHandCards(int player)
        {
            return this.goPlayersHandCards[player].Count;
        }

        internal List<GameObject> GetCardsOfPlayerHand(int player)
        {
            return this.goPlayersHandCards[player];
        }

        internal GameObject GetCardAtOfPlayerHand(int player, int handIndex)
        {
            return this.goPlayersHandCards[player].ElementAt(handIndex);
        }

        internal void RemoveCardAtOfPlayerHand(int player, int handIndex)
        {
            this.goPlayersHandCards[player].RemoveAt(handIndex);
        }

        /// <summary>
        /// 今回フォーカスするカードを持ち上げる
        /// </summary>
        /// <param name="player"></param>
        /// <param name="handIndesx"></param>
        internal void SetFocusCardOfPlayerHand(int player, int handIndesx)
        {
            var goCurrentCard = this.GetCardAtOfPlayerHand(player, handIndesx);
            this.SetFocusHand(goCurrentCard);
        }

        /// <summary>
        /// 前にフォーカスしていたカードを、盤に下ろす
        /// </summary>
        /// <param name="player"></param>
        /// <param name="handIndex"></param>
        internal void ResetFocusCardOfPlayerHand(int player, int handIndex)
        {
            var goPreviousCard = this.GetCardAtOfPlayerHand(player, handIndex);
            this.ResetFocusHand(goPreviousCard);
        }

        /// <summary>
        /// 場札カードを持ち上げる
        /// </summary>
        /// <param name="card"></param>
        void SetFocusHand(GameObject card)
        {
            var liftY = 5.0f; // 持ち上げる（パースペクティブがかかっていて、持ち上げすぎると北へ移動したように見える）
            var rotateY = -5; // -5°傾ける
            var rotateZ = -5; // -5°傾ける

            card.transform.position = new Vector3(card.transform.position.x, card.transform.position.y + liftY, card.transform.position.z);
            card.transform.rotation = Quaternion.Euler(card.transform.rotation.eulerAngles.x, card.transform.rotation.eulerAngles.y + rotateY, card.transform.eulerAngles.z + rotateZ);
        }

        /// <summary>
        /// 持ち上げたカードを場に戻す
        /// </summary>
        /// <param name="card"></param>
        void ResetFocusHand(GameObject card)
        {
            var liftY = 5.0f; // 持ち上げる（パースペクティブがかかっていて、持ち上げすぎると北へ移動したように見える）
            var rotateY = -5; // -5°傾ける
            var rotateZ = -5; // -5°傾ける

            // 逆をする
            liftY = -liftY;
            rotateY = -rotateY;
            rotateZ = -rotateZ;

            card.transform.position = new Vector3(card.transform.position.x, card.transform.position.y + liftY, card.transform.position.z);
            card.transform.rotation = Quaternion.Euler(card.transform.rotation.eulerAngles.x, card.transform.rotation.eulerAngles.y + rotateY, card.transform.eulerAngles.z + rotateZ);
        }

    }
}
