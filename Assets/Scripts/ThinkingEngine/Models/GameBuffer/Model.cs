namespace Assets.Scripts.ThinkingEngine.Models.GameBuffer
{
    using Assets.Scripts.Vision.Models;
    using System.Collections.Generic;
    using ModelOfThinkingEngine = Assets.Scripts.ThinkingEngine.Models;

    /// <summary>
    /// ゲームの状態
    /// 
    /// - 編集可能
    /// </summary>
    public class Model
    {
        // - その他

        /// <summary>
        /// 生成
        /// </summary>
        internal Model(
            CenterStack[] centerStacks,
            Player[] players)
        {
            this.CenterStacks = centerStacks;
            this.Players = players;
        }

        // - プロパティ

        /// <summary>
        /// 対局中か？
        /// </summary>
        public bool IsGameActive { get; set; }

        // ゲーム内経過時間
        internal GameSeconds ElapsedTimeObj { get; set; } = GameSeconds.Zero;

        #region プロパティ（台札別）
        /// <summary>
        /// ゲーム・モデル・バッファー
        /// 
        /// - 台札別
        /// </summary>
        CenterStack[] CenterStacks { get; set; }

        internal CenterStack GetCenterStack(CenterStackPlace place)
        {
            return this.CenterStacks[place.AsInt];
        }
        #endregion

        #region プロパティ（プレイヤー別）
        /// <summary>
        /// ゲーム・モデル・バッファー
        /// 
        /// - プレイヤー別
        /// </summary>
        Player[] Players { get; set; }

        internal Player GetPlayer(ModelOfThinkingEngine.Player player)
        {
            return this.Players[player.AsInt];
        }
        #endregion

        /// <summary>
        /// ｎプレイヤーが選択している場札は、先頭から何枚目
        /// 
        /// - 選択中の場札が無いなら、-1
        /// </summary>
        internal HandCardIndex[] IndexOfFocusedCardOfPlayersObj { get; set; } = { Commons.HandCardIndexNoSelected, Commons.HandCardIndexNoSelected };

        // - メソッド

        #region メソッド（手札関連）
        /// <summary>
        /// 手札を追加
        /// </summary>
        /// <param name="playerObj"></param>
        /// <param name="idOfCard"></param>
        internal void AddCardOfPlayersPile(ModelOfThinkingEngine.Player playerObj, IdOfPlayingCards idOfCard)
        {
            this.Players[playerObj.AsInt].IdOfCardsOfPlayersPile.Add(idOfCard);
        }

        /// <summary>
        /// 手札を削除
        /// </summary>
        /// <param name="playerObj"></param>
        /// <param name="startIndexObj"></param>
        /// <param name="numberOfCards"></param>
        internal void RemoveRangeCardsOfPlayerPile(ModelOfThinkingEngine.Player playerObj, PlayerPileCardIndex startIndexObj, int numberOfCards)
        {
            this.Players[playerObj.AsInt].IdOfCardsOfPlayersPile.RemoveRange(startIndexObj.AsInt, numberOfCards);
        }
        #endregion

        #region メソッド（場札関連）
        /// <summary>
        /// 場札を追加
        /// </summary>
        /// <param name="playerObj"></param>
        /// <param name="idOfCards"></param>
        internal void AddRangeCardsOfPlayerHand(ModelOfThinkingEngine.Player playerObj, List<IdOfPlayingCards> idOfCards)
        {
            this.Players[playerObj.AsInt].IdOfCardsOfPlayersHand.AddRange(idOfCards);
        }

        /// <summary>
        /// 場札を削除
        /// </summary>
        /// <param name="playerObj"></param>
        /// <param name="handIndexObj"></param>
        internal void RemoveCardAtOfPlayerHand(ModelOfThinkingEngine.Player playerObj, HandCardIndex handIndexObj)
        {
            this.Players[playerObj.AsInt].IdOfCardsOfPlayersHand.RemoveAt(handIndexObj.AsInt);
        }
        #endregion

        /// <summary>
        /// 手札から場札へ移動
        /// </summary>
        /// <param name="playerObj"></param>
        /// <param name="startIndexObj"></param>
        /// <param name="numberOfCards"></param>
        internal void MoveCardsToHandFromPile(ModelOfThinkingEngine.Player playerObj, PlayerPileCardIndex startIndexObj, int numberOfCards)
        {
            var idOfCards = this.Players[playerObj.AsInt].IdOfCardsOfPlayersPile.GetRange(startIndexObj.AsInt, numberOfCards);

            this.RemoveRangeCardsOfPlayerPile(playerObj, startIndexObj, numberOfCards);
            this.AddRangeCardsOfPlayerHand(playerObj, idOfCards);
        }
    }
}
