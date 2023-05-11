namespace Assets.Scripts.ThinkingEngine.Models.Game.Writer
{
    using ModelOfGameBuffer = Assets.Scripts.ThinkingEngine.Models.Game.Buffer;

    /// <summary>
    /// ゲーム・モデル
    /// 
    /// - 台札別
    /// </summary>
    internal class CenterStack
    {
        // - その他

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="placeObj">右:0, 左:1</param>
        internal CenterStack(
            ModelOfGameBuffer.Model gameModelBuffer,
            CenterStackPlace placeObj)
        {
            this.gameModelBuffer = gameModelBuffer;
            this.placeObj = placeObj;
        }

        // - フィールド

        readonly ModelOfGameBuffer.Model gameModelBuffer;

        readonly CenterStackPlace placeObj;

        // - メソッド

        internal ReadonlyList<IdOfPlayingCards> GetCards()
        {
            return new ReadonlyList<IdOfPlayingCards>(this.gameModelBuffer.GetCenterStack(this.placeObj).IdOfCards);
        }

        internal IdOfPlayingCards GetCard(int index)
        {
            return this.gameModelBuffer.GetCenterStack(this.placeObj).IdOfCards[index];
        }

        internal int GetLength()
        {
            return this.gameModelBuffer.GetCenterStack(this.placeObj).GetLength();
        }

        /// <summary>
        /// 右（または左）の天辺の台札
        /// </summary>
        /// <returns>無ければ None</returns>
        internal IdOfPlayingCards GetLastCard()
        {
            var length = this.GetLengthOfCards();
            var startIndex = length - 1;

            if (startIndex == -1 || this.gameModelBuffer.GetCenterStack(this.placeObj).IdOfCards.Count <= startIndex)
            {
                return IdOfPlayingCards.None;
            }

            // Debug.Log($"[GameModel GetLastCardOfCenterStack] place:{place} stack-count:{this.gameModelBuffer.IdOfCardsOfCenterStacks[place].Count} startIndex:{startIndex}");
            return this.gameModelBuffer.GetCenterStack(this.placeObj).IdOfCards[startIndex]; // 最後のカード
        }

        /// <summary>
        /// 右（または左）の台札の枚数
        /// </summary>
        internal int GetLengthOfCards()
        {
            return this.gameModelBuffer.GetCenterStack(this.placeObj).IdOfCards.Count;
        }

        /// <summary>
        /// 台札の天辺
        /// </summary>
        /// <returns></returns>
        internal IdOfPlayingCards GetTopCard()
        {
            var centerStack = this.gameModelBuffer.GetCenterStack(this.placeObj).IdOfCards;

            var length = centerStack.Count;
            if (length < 1)
            {
                // 床上
                return IdOfPlayingCards.None;
            }

            // 台札の天辺
            return centerStack[length - 1];
        }

        /// <summary>
        /// 台札を追加
        /// </summary>
        /// <param name="idOfCard"></param>
        internal void AddCard(IdOfPlayingCards idOfCard)
        {
            this.gameModelBuffer.GetCenterStack(this.placeObj).IdOfCards.Add(idOfCard);
        }

        /// <summary>
        /// 台札を削除
        /// </summary>
        /// <param name="startIndexObj"></param>
        internal void RemoveCardAt(CenterStackCardIndex startIndexObj)
        {
            this.gameModelBuffer.GetCenterStack(this.placeObj).IdOfCards.RemoveAt(startIndexObj.AsInt);
        }

        /// <summary>
        /// 台札を空にする
        /// </summary>
        internal void Clear()
        {
            this.gameModelBuffer.GetCenterStack(this.placeObj).IdOfCards.Clear();
        }
    }
}
