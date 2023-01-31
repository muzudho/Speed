namespace Assets.Scripts.Models
{
    using System.Collections.Generic;

    class GameModelBuffer
    {
        // - プロパティ

        /// <summary>
        /// プレイヤーが選択している場札は、先頭から何枚目
        /// 
        /// - 選択中の場札が無いなら、-1
        /// </summary>
        internal int[] IndexOfFocusedCardOfPlayers { get; set; } = { -1, -1 };

        /// <summary>
        /// 手札
        /// 0: １プレイヤー（黒色）
        /// 1: ２プレイヤー（黒色）
        /// </summary>
        internal List<List<IdOfPlayingCards>> goPlayersPileCards { get; set; } = new() { new(), new() };

        /// <summary>
        /// 場札（プレイヤー側でオープンしている札）
        /// 0: １プレイヤー（黒色）
        /// 1: ２プレイヤー（黒色）
        /// </summary>
        internal List<List<IdOfPlayingCards>> goPlayersHandCards { get; set; } = new() { new(), new() };

        /// <summary>
        /// 台札（画面中央に積んでいる札）
        /// 0: 右
        /// 1: 左
        /// </summary>
        internal List<List<IdOfPlayingCards>> goCenterStacksCards { get; set; } = new() { new(), new() };

        internal void RemoveCardAtOfCenterStack(int place, int startIndex)
        {
            this.goCenterStacksCards[place].RemoveAt(startIndex);
        }

        internal void AddCardOfCenterStack(int place, IdOfPlayingCards idOfCard)
        {
            this.goCenterStacksCards[place].Add(idOfCard);
        }

        /// <summary>
        /// 手札を追加
        /// </summary>
        /// <param name="player"></param>
        /// <param name="idOfCard"></param>
        internal void AddCardOfPlayersPile(int player, IdOfPlayingCards idOfCard)
        {
            this.goPlayersPileCards[player].Add(idOfCard);
        }

        internal void RemoveRangeCardsOfPlayerPile(int player, int startIndex, int numberOfCards)
        {
            this.goPlayersPileCards[player].RemoveRange(startIndex, numberOfCards);
        }

        /// <summary>
        /// 場札を追加
        /// </summary>
        /// <param name="player"></param>
        /// <param name="idOfCards"></param>
        internal void AddRangeCardsOfPlayerHand(int player, List<IdOfPlayingCards> idOfCards)
        {
            this.goPlayersHandCards[player].AddRange(idOfCards);
        }

        internal void RemoveCardAtOfPlayerHand(int player, int handIndex)
        {
            this.goPlayersHandCards[player].RemoveAt(handIndex);
        }
    }
}
