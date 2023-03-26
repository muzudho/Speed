namespace Assets.Scripts.ThinkingEngine.Models.Game
{
    using Assets.Scripts.Vision.Models;
    using System.Collections.Generic;
    using ModelOfGameBuffer = Assets.Scripts.ThinkingEngine.Models.GameBuffer;

    /// <summary>
    /// ゲーム・モデル
    /// 
    /// - 読み取り専用。(Immutable)
    /// </summary>
    partial class Default
    {
        // - その他

        public Default(ModelOfGameBuffer.Model gameModelBuffer)
        {
            this.gameModelBuffer = gameModelBuffer;
        }

        // - フィールド

        ModelOfGameBuffer.Model gameModelBuffer;

        // - プロパティ

        internal GameSeconds ElapsedSeconds => gameModelBuffer.ElapsedTimeObj;

        /// <summary>
        /// 対局中か？
        /// </summary>
        /// <returns></returns>
        internal bool IsGameActive => this.gameModelBuffer.IsGameActive;

        // - メソッド

        /// <summary>
        /// TODO 本当は JSON か何かで出力して、 GameModelBuffer が JSON を読込むような感じにしたい
        /// </summary>
        /// <returns></returns>
        internal ModelOfGameBuffer.Model CreateGameModelBuffer()
        {
            // ディープ・コピー
            return new ModelOfGameBuffer.Model()
            {
                // ゲーム内経過時間
                ElapsedTimeObj = this.gameModelBuffer.ElapsedTimeObj,

                // プレイヤー別
                Players = new ModelOfGameBuffer.Player[2]
                {
                    // １プレイヤー
                    new(
                        // 台札
                        idOfCardsOfCenterStacks: new List<IdOfPlayingCards>(this.gameModelBuffer.Players[Commons.Player1.AsInt].IdOfCardsOfCenterStacks.ToArray()),
                        // 手札
                        idOfCardsOfPlayersPile :new List<IdOfPlayingCards>(this.gameModelBuffer.Players[Commons.Player1.AsInt].IdOfCardsOfPlayersPile.ToArray())
                        ),

                    // ２プレイヤー
                    new(
                        // 台札
                        idOfCardsOfCenterStacks: new List<IdOfPlayingCards>(this.gameModelBuffer.Players[Commons.Player2.AsInt].IdOfCardsOfCenterStacks.ToArray()),
                        // 手札
                        idOfCardsOfPlayersPile :new List<IdOfPlayingCards>(this.gameModelBuffer.Players[Commons.Player2.AsInt].IdOfCardsOfPlayersPile.ToArray())
                        ),
                },

                // 場札
                IdOfCardsOfPlayersHand = new List<List<IdOfPlayingCards>>()
                {
                    new List<IdOfPlayingCards>(this.gameModelBuffer.IdOfCardsOfPlayersHand[Commons.Player1.AsInt].ToArray()),
                    new List<IdOfPlayingCards>(this.gameModelBuffer.IdOfCardsOfPlayersHand[Commons.Player2.AsInt].ToArray()),
                },

                // ピックアップ場札
                IndexOfFocusedCardOfPlayersObj = new HandCardIndex[]
                {
                    this.gameModelBuffer.IndexOfFocusedCardOfPlayersObj[Commons.Player1.AsInt],
                    this.gameModelBuffer.IndexOfFocusedCardOfPlayersObj[Commons.Player2.AsInt],
                }
            };
        }
    }
}
