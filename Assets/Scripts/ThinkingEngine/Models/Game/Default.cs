namespace Assets.Scripts.ThinkingEngine.Models.Game
{
    using System.Collections.Generic;

    /// <summary>
    /// ゲーム・モデル
    /// 
    /// - 読み取り専用。(Immutable)
    /// </summary>
    partial class Default
    {
        // - その他

        public Default(GameModelBuffer gameModelBuffer)
        {
            this.gameModelBuffer = gameModelBuffer;
        }

        // - フィールド

        GameModelBuffer gameModelBuffer;

        // - プロパティ

        internal float ElapsedSeconds => gameModelBuffer.ElapsedSeconds;

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
        internal GameModelBuffer CreateGameModelBuffer()
        {
            // ディープ・コピー
            return new GameModelBuffer()
            {
                // ゲーム内経過時間
                ElapsedSeconds = this.gameModelBuffer.ElapsedSeconds,

                // 台札
                IdOfCardsOfCenterStacks = new List<List<IdOfPlayingCards>>()
                {
                    new List<IdOfPlayingCards>(this.gameModelBuffer.IdOfCardsOfCenterStacks[0].ToArray()),
                    new List<IdOfPlayingCards>(this.gameModelBuffer.IdOfCardsOfCenterStacks[1].ToArray()),
                },

                // 手札
                IdOfCardsOfPlayersPile = new List<List<IdOfPlayingCards>>()
                {
                    new List<IdOfPlayingCards>(this.gameModelBuffer.IdOfCardsOfPlayersPile[0].ToArray()),
                    new List<IdOfPlayingCards>(this.gameModelBuffer.IdOfCardsOfPlayersPile[1].ToArray()),
                },

                // 場札
                IdOfCardsOfPlayersHand = new List<List<IdOfPlayingCards>>()
                {
                    new List<IdOfPlayingCards>(this.gameModelBuffer.IdOfCardsOfPlayersHand[0].ToArray()),
                    new List<IdOfPlayingCards>(this.gameModelBuffer.IdOfCardsOfPlayersHand[1].ToArray()),
                },

                // ピックアップ場札
                IndexOfFocusedCardOfPlayers = new int[]
                {
                    this.gameModelBuffer.IndexOfFocusedCardOfPlayers[0],
                    this.gameModelBuffer.IndexOfFocusedCardOfPlayers[1],
                }
            };
        }
    }
}
