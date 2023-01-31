namespace Assets.Scripts
{
    class GameModelBuffer
    {
        /// <summary>
        /// プレイヤーが選択している場札は、先頭から何枚目
        /// 
        /// - 選択中の場札が無いなら、-1
        /// </summary>
        internal int[] IndexOfFocusedCardOfPlayers { get; set; } = { -1, -1 };
    }
}
