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
            return new ModelOfGameBuffer.Model(
                // 台札別
                centerStacks: new ModelOfGameBuffer.CenterStack[2]
                {
                    // 右
                    new(
                        // 台札
                        idOfCards: new List<IdOfPlayingCards>(this.gameModelBuffer.GetCenterStack(Commons.RightCenterStack).IdOfCards.ToArray())
                        ),

                    // 左
                    new (
                        // 台札
                        idOfCards: new List<IdOfPlayingCards>(this.gameModelBuffer.GetCenterStack(Commons.LeftCenterStack).IdOfCards.ToArray())
                        ),
                },
                // プレイヤー別
                players: new ModelOfGameBuffer.Player[2]
                {
                    // １プレイヤー
                    new(
                        // 手札
                        idOfCardsOfPlayersPile: new List<IdOfPlayingCards>(this.gameModelBuffer.GetPlayer(Commons.Player1).IdOfCardsOfPlayersPile.ToArray()),
                        // 場札
                        idOfCardsOfPlayersHand: new List<IdOfPlayingCards>(this.gameModelBuffer.GetPlayer(Commons.Player1).IdOfCardsOfPlayersHand.ToArray())
                        ),

                    // ２プレイヤー
                    new(
                        // 手札
                        idOfCardsOfPlayersPile: new List<IdOfPlayingCards>(this.gameModelBuffer.GetPlayer(Commons.Player2).IdOfCardsOfPlayersPile.ToArray()),
                        // 場札
                        idOfCardsOfPlayersHand: new List<IdOfPlayingCards>(this.gameModelBuffer.GetPlayer(Commons.Player2).IdOfCardsOfPlayersHand.ToArray())
                        ),
                }
                )
            {
                // ゲーム内経過時間
                ElapsedTimeObj = this.gameModelBuffer.ElapsedTimeObj,

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
