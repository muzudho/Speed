namespace Assets.Scripts.ThinkingEngine.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// ゲームの状態
    /// 
    /// - 編集可能
    /// </summary>
    public class GameModelBuffer
    {
        // - プロパティ

        /// <summary>
        /// 対局中か？
        /// </summary>
        public bool IsGameActive { get; set; }

        // ゲーム内経過時間
        internal float ElapsedSeconds { get; set; } = 0.0f;

        /// <summary>
        /// 台札
        /// 
        /// - 画面中央に積んでいる札
        /// - 0: 右
        /// - 1: 左
        /// </summary>
        internal List<List<IdOfPlayingCards>> IdOfCardsOfCenterStacks { get; set; } = new() { new(), new() };

        /// <summary>
        /// 手札
        /// 
        /// - プレイヤー側で積んでる札
        /// - 0: １プレイヤー（黒色）
        /// - 1: ２プレイヤー（黒色）
        /// </summary>
        internal List<List<IdOfPlayingCards>> IdOfCardsOfPlayersPile { get; set; } = new() { new(), new() };

        /// <summary>
        /// 場札
        /// 
        /// - プレイヤー側でオープンしている札
        /// - 0: １プレイヤー（黒色）
        /// - 1: ２プレイヤー（黒色）
        /// </summary>
        internal List<List<IdOfPlayingCards>> IdOfCardsOfPlayersHand { get; set; } = new() { new(), new() };

        /// <summary>
        /// ｎプレイヤーが選択している場札は、先頭から何枚目
        /// 
        /// - 選択中の場札が無いなら、-1
        /// </summary>
        internal HandCardIndex[] IndexOfFocusedCardOfPlayersObj { get; set; } = { Commons.HandCardIndexNoSelected, Commons.HandCardIndexNoSelected };

        // - メソッド

        /// <summary>
        /// 台札を削除
        /// </summary>
        /// <param name="place"></param>
        /// <param name="startIndexObj"></param>
        internal void RemoveCardAtOfCenterStack(CenterStackPlace place, CenterStackCardIndex startIndexObj)
        {
            this.IdOfCardsOfCenterStacks[place.AsInt].RemoveAt(startIndexObj.AsInt);
        }

        /// <summary>
        /// 台札を追加
        /// </summary>
        /// <param name="placeObj"></param>
        /// <param name="idOfCard"></param>
        internal void AddCardOfCenterStack(CenterStackPlace placeObj, IdOfPlayingCards idOfCard)
        {
            this.IdOfCardsOfCenterStacks[placeObj.AsInt].Add(idOfCard);
        }

        /// <summary>
        /// 手札を追加
        /// </summary>
        /// <param name="playerObj"></param>
        /// <param name="idOfCard"></param>
        internal void AddCardOfPlayersPile(Player playerObj, IdOfPlayingCards idOfCard)
        {
            this.IdOfCardsOfPlayersPile[playerObj.AsInt].Add(idOfCard);
        }

        /// <summary>
        /// 手札を削除
        /// </summary>
        /// <param name="playerObj"></param>
        /// <param name="startIndexObj"></param>
        /// <param name="numberOfCards"></param>
        internal void RemoveRangeCardsOfPlayerPile(Player playerObj, PlayerPileCardIndex startIndexObj, int numberOfCards)
        {
            this.IdOfCardsOfPlayersPile[playerObj.AsInt].RemoveRange(startIndexObj.AsInt, numberOfCards);
        }

        /// <summary>
        /// 場札を追加
        /// </summary>
        /// <param name="playerObj"></param>
        /// <param name="idOfCards"></param>
        internal void AddRangeCardsOfPlayerHand(Player playerObj, List<IdOfPlayingCards> idOfCards)
        {
            this.IdOfCardsOfPlayersHand[playerObj.AsInt].AddRange(idOfCards);
        }

        /// <summary>
        /// 場札を削除
        /// </summary>
        /// <param name="playerObj"></param>
        /// <param name="handIndexObj"></param>
        internal void RemoveCardAtOfPlayerHand(Player playerObj, HandCardIndex handIndexObj)
        {
            this.IdOfCardsOfPlayersHand[playerObj.AsInt].RemoveAt(handIndexObj.AsInt);
        }

        /// <summary>
        /// 手札から場札へ移動
        /// </summary>
        /// <param name="playerObj"></param>
        /// <param name="startIndexObj"></param>
        /// <param name="numberOfCards"></param>
        internal void MoveCardsToHandFromPile(Player playerObj, PlayerPileCardIndex startIndexObj, int numberOfCards)
        {
            var idOfCards = this.IdOfCardsOfPlayersPile[playerObj.AsInt].GetRange(startIndexObj.AsInt, numberOfCards);

            this.RemoveRangeCardsOfPlayerPile(playerObj, startIndexObj, numberOfCards);
            this.AddRangeCardsOfPlayerHand(playerObj, idOfCards);
        }
    }
}
