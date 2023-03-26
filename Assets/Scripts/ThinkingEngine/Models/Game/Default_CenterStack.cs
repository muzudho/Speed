namespace Assets.Scripts.ThinkingEngine.Models.Game
{
    partial class Default
    {
        // - メソッド

        internal ReadonlyList<IdOfPlayingCards> GetCenterStack(CenterStackPlace placeObj)
        {
            return new ReadonlyList<IdOfPlayingCards>(this.gameModelBuffer.Players[placeObj.AsInt].IdOfCardsOfCenterStacks);
        }

        /// <summary>
        /// 右（または左）の天辺の台札
        /// </summary>
        /// <param name="placeObj">右:0, 左:1</param>
        /// <returns>無ければ None</returns>
        internal IdOfPlayingCards GetLastCardOfCenterStack(CenterStackPlace placeObj)
        {
            var length = this.GetLengthOfCenterStackCards(placeObj);
            var startIndex = length - 1;

            if (startIndex == -1 || this.gameModelBuffer.Players[placeObj.AsInt].IdOfCardsOfCenterStacks.Count <= startIndex)
            {
                return IdOfPlayingCards.None;
            }

            // Debug.Log($"[GameModel GetLastCardOfCenterStack] place:{place} stack-count:{this.gameModelBuffer.IdOfCardsOfCenterStacks[place].Count} startIndex:{startIndex}");
            return this.gameModelBuffer.Players[placeObj.AsInt].IdOfCardsOfCenterStacks[startIndex]; // 最後のカード
        }

        /// <summary>
        /// 右（または左）の台札の枚数
        /// </summary>
        /// <param name="placeObj">右:0, 左:1</param>
        internal int GetLengthOfCenterStackCards(CenterStackPlace placeObj)
        {
            return this.gameModelBuffer.Players[placeObj.AsInt].IdOfCardsOfCenterStacks.Count;
        }

        /// <summary>
        /// 台札の天辺
        /// </summary>
        /// <param name="placeObj"></param>
        /// <param name="getLengthOfCenterStackCards"></param>
        /// <param name="getLastCardOfCenterStack">天辺（最後）のカード</param>
        /// <returns></returns>
        internal IdOfPlayingCards GetTopOfCenterStack(CenterStackPlace placeObj)
        {
            var centerStack = this.gameModelBuffer.Players[placeObj.AsInt].IdOfCardsOfCenterStacks;

            var length = centerStack.Count;
            if (length < 1)
            {
                // 床上
                return IdOfPlayingCards.None;
            }

            // 台札の天辺
            return centerStack[length - 1];
        }
    }
}
