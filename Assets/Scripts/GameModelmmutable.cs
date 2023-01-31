namespace Assets.Scripts
{
    /// <summary>
    /// ゲーム・モデル
    /// 
    /// - Immutable
    /// </summary>
    class GameModel
    {
        GameModelBuffer gameModelBuffer;

        public GameModel(GameModelBuffer gameModel)
        {
            this.gameModelBuffer = gameModel;
        }

        /// <summary>
        /// プレイヤーが選択している場札は、先頭から何枚目
        /// 
        /// - 選択中の場札が無いなら、-1
        /// </summary>
        internal int GetIndexOfFocusedCardOfPlayer(int player)
        {
            return this.gameModelBuffer.IndexOfFocusedCardOfPlayers[player];
        }

    }
}
