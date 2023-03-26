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
        // - プロパティ

        /// <summary>
        /// 対局中か？
        /// </summary>
        public bool IsGameActive { get; set; }

        // ゲーム内経過時間
        internal GameSeconds ElapsedTimeObj { get; set; } = GameSeconds.Zero;

        /// <summary>
        /// ゲーム・モデル・バッファー
        /// 
        /// - プレイヤー別
        /// </summary>
        internal Player[] Players { get; set; } = new Player[2]
        {
            // １プレイヤー
            new(
                idOfCardsOfCenterStacks: new List<IdOfPlayingCards>(),
                idOfCardsOfPlayersPile: new List<IdOfPlayingCards>()
                ),

            // ２プレイヤー
            new(
                idOfCardsOfCenterStacks: new List<IdOfPlayingCards>(),
                idOfCardsOfPlayersPile: new List<IdOfPlayingCards>()
                ),
        };

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

        #region メソッド（台札関連）
        /// <summary>
        /// 台札の枚数
        /// </summary>
        /// <param name="placeObj"></param>
        /// <param name="idOfCard"></param>
        internal IdOfPlayingCards GetCardOfCenterStack(CenterStackPlace placeObj, int index)
        {
            return this.Players[placeObj.AsInt].IdOfCardsOfCenterStacks[index];
        }

        /// <summary>
        /// 台札の枚数
        /// </summary>
        /// <param name="placeObj"></param>
        /// <param name="idOfCard"></param>
        internal int GetLengthOfCenterStack(CenterStackPlace placeObj)
        {
            return this.Players[placeObj.AsInt].IdOfCardsOfCenterStacks.Count;
        }

        /// <summary>
        /// 台札を追加
        /// </summary>
        /// <param name="placeObj"></param>
        /// <param name="idOfCard"></param>
        internal void AddCardOfCenterStack(CenterStackPlace placeObj, IdOfPlayingCards idOfCard)
        {
            // TODO スレッド・セーフだろうか？
            this.Players[placeObj.AsInt].IdOfCardsOfCenterStacks.Add(idOfCard);
        }

        /// <summary>
        /// 台札を削除
        /// </summary>
        /// <param name="place"></param>
        /// <param name="startIndexObj"></param>
        internal void RemoveCardAtOfCenterStack(CenterStackPlace place, CenterStackCardIndex startIndexObj)
        {
            this.Players[place.AsInt].IdOfCardsOfCenterStacks.RemoveAt(startIndexObj.AsInt);
        }
        #endregion

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
            this.IdOfCardsOfPlayersHand[playerObj.AsInt].AddRange(idOfCards);
        }

        /// <summary>
        /// 場札を削除
        /// </summary>
        /// <param name="playerObj"></param>
        /// <param name="handIndexObj"></param>
        internal void RemoveCardAtOfPlayerHand(ModelOfThinkingEngine.Player playerObj, HandCardIndex handIndexObj)
        {
            this.IdOfCardsOfPlayersHand[playerObj.AsInt].RemoveAt(handIndexObj.AsInt);
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
