﻿namespace Assets.Scripts.Models.Timeline.Commands
{
    using Assets.Scripts.Models;
    using Assets.Scripts.Views;
    using System;

    /// <summary>
    /// ｎプレイヤーは、右（または左）隣のカードへ、ピックアップを移動します
    /// </summary>
    class MoveFocusToNextCard : ICommand
    {
        // - 生成

        internal MoveFocusToNextCard(int player, int direction, LazyArgs.SetValue<int> setIndexOfNextFocusedHandCard)
        {
            this.Player = player;
            this.Direction = direction;
            this.SetIndexOfNextFocusedHandCard = setIndexOfNextFocusedHandCard;
        }

        // - プロパティ

        int Player { get; set; }
        int Direction { get; set; }
        LazyArgs.SetValue<int> SetIndexOfNextFocusedHandCard { get; set; }

        // - メソッド

        /// <summary>
        /// ｎプレイヤーは、右（または左）隣のカードへ、ピックアップを移動します
        /// </summary>
        /// <param name="player"></param>
        /// <param name="direction">後ろ:0, 前:1</param>
        public void DoIt(GameModelBuffer gameModelBuffer, GameViewModel gameViewModel)
        {
            GameModel gameModel = new GameModel(gameModelBuffer);
            int indexOfFocusedHandCard = gameModelBuffer.IndexOfFocusedCardOfPlayers[Player];

            int current;
            var length = gameModelBuffer.IdOfCardsOfPlayersHand[Player].Count;

            if (length < 1)
            {
                // 場札が無いなら、何もピックアップされていません
                current = -1;
            }
            else
            {
                switch (Direction)
                {
                    // 後ろへ
                    case 0:
                        if (indexOfFocusedHandCard == -1 || length <= indexOfFocusedHandCard + 1)
                        {
                            // （ピックアップしているカードが無いとき）先頭の外から、先頭へ入ってくる
                            current = 0;
                        }
                        else
                        {
                            current = indexOfFocusedHandCard + 1;
                        }
                        break;

                    // 前へ
                    case 1:
                        if (indexOfFocusedHandCard == -1 || indexOfFocusedHandCard - 1 < 0)
                        {
                            // （ピックアップしているカードが無いとき）最後尾の外から、最後尾へ入ってくる
                            current = length - 1;
                        }
                        else
                        {
                            current = indexOfFocusedHandCard - 1;
                        }
                        break;

                    default:
                        throw new Exception();
                }
            }

            SetIndexOfNextFocusedHandCard(current);

            if (0 <= indexOfFocusedHandCard && indexOfFocusedHandCard < gameModelBuffer.IdOfCardsOfPlayersHand[Player].Count) // 範囲内なら
            {
                // 前にフォーカスしていたカードを、盤に下ろす
                gameViewModel.PutDownCardOfHand(gameModel, Player, indexOfFocusedHandCard);
            }

            if (0 <= current && current < gameModelBuffer.IdOfCardsOfPlayersHand[Player].Count) // 範囲内なら
            {
                // 今回フォーカスするカードを持ち上げる
                gameViewModel.PickupCardOfHand(gameModel, Player, current);
            }
        }
    }
}
