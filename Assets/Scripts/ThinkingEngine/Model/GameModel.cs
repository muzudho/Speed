namespace Assets.Scripts.ThinkingEngine.Model
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// ゲーム・モデル
    /// 
    /// - 読み取り専用。(Immutable)
    /// </summary>
    class GameModel
    {
        // - その他

        public GameModel(GameModelBuffer gameModelBuffer)
        {
            this.gameModelBuffer = gameModelBuffer;
        }

        // - フィールド

        GameModelBuffer gameModelBuffer;

        // - プロパティ

        internal float ElapsedSeconds => gameModelBuffer.ElapsedSeconds;

        // - メソッド

        internal ReadonlyList<IdOfPlayingCards> GetCenterStack(int place)
        {
            return new ReadonlyList<IdOfPlayingCards>(this.gameModelBuffer.IdOfCardsOfCenterStacks[place]);
        }

        /// <summary>
        /// 右（または左）の天辺の台札
        /// </summary>
        /// <param name="place">右:0, 左:1</param>
        /// <returns>無ければ None</returns>
        internal IdOfPlayingCards GetLastCardOfCenterStack(int place)
        {
            var length = this.GetLengthOfCenterStackCards(place);
            var startIndex = length - 1;

            if (startIndex==-1 || this.gameModelBuffer.IdOfCardsOfCenterStacks[place].Count <= startIndex)
            {
                return IdOfPlayingCards.None;
            }

            Debug.Log($"[GameModel GetLastCardOfCenterStack] place:{place} stack-count:{this.gameModelBuffer.IdOfCardsOfCenterStacks[place].Count} startIndex:{startIndex}");
            return this.gameModelBuffer.IdOfCardsOfCenterStacks[place][startIndex]; // 最後のカード
        }

        /// <summary>
        /// ｎプレイヤーが選択している場札は、先頭から何枚目
        /// 
        /// - 選択中の場札が無いなら、-1
        /// </summary>
        /// <param name="player">プレイヤー</param>
        internal int GetIndexOfFocusedCardOfPlayer(int player)
        {
            return this.gameModelBuffer.IndexOfFocusedCardOfPlayers[player];
        }

        /// <summary>
        /// 右（または左）の台札の枚数
        /// </summary>
        /// <param name="place">右:0, 左:1</param>
        internal int GetLengthOfCenterStackCards(int place)
        {
            return this.gameModelBuffer.IdOfCardsOfCenterStacks[place].Count;
        }

        /// <summary>
        /// 台札の天辺
        /// </summary>
        /// <param name="place"></param>
        /// <param name="getLengthOfCenterStackCards"></param>
        /// <param name="getLastCardOfCenterStack">天辺（最後）のカード</param>
        /// <returns></returns>
        internal IdOfPlayingCards GetTopOfCenterStack(int place)
        {
            var centerStack = this.gameModelBuffer.IdOfCardsOfCenterStacks[place];

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
        /// ｎプレイヤーの、場札の枚数
        /// </summary>
        /// <param name="player">プレイヤー</param>
        /// <returns></returns>
        internal int GetLengthOfPlayerHandCards(int player)
        {
            return this.gameModelBuffer.IdOfCardsOfPlayersHand[player].Count;
        }

        /// <summary>
        /// ｎプレイヤーの、場札をリストで取得
        /// </summary>
        /// <param name="player">プレイヤー</param>
        /// <returns></returns>
        internal List<IdOfPlayingCards> GetCardsOfPlayerHand(int player)
        {
            return this.gameModelBuffer.IdOfCardsOfPlayersHand[player];
        }

        /// <summary>
        /// ｎプレイヤーの、ｍ枚目の場札を取得
        /// </summary>
        /// <param name="player"></param>
        /// <param name="handIndex"></param>
        /// <returns></returns>
        internal IdOfPlayingCards GetCardAtOfPlayerHand(int player, int handIndex)
        {
            return this.gameModelBuffer.IdOfCardsOfPlayersHand[player][handIndex];
        }

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
