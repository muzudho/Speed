using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Commands
{
    static class MoveFocusToNextCard
    {
        /// <summary>
        /// 隣のカードへフォーカスを移します
        /// </summary>
        /// <param name="player"></param>
        /// <param name="direction">後ろ:0, 前:1</param>
        internal static void DoIt(GameModelBuffer gameModelBuffer, GameViewModel gameViewModel, int player, int direction, int indexOfFocusedHandCard, LazyArgs.SetValue<int> setIndexOfNextFocusedHandCard)
        {
            GameModel gameModel = new GameModel(gameModelBuffer);

            int current;
            var length = gameModelBuffer.IdOfCardsOfPlayersHand[player].Count;

            if (length < 1)
            {
                // 場札が無いなら、何もピックアップされていません
                current = -1;
            }
            else
            {
                switch (direction)
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

            setIndexOfNextFocusedHandCard(current);

            if (0 <= indexOfFocusedHandCard && indexOfFocusedHandCard < gameModelBuffer.IdOfCardsOfPlayersHand[player].Count) // 範囲内なら
            {
                // 前にフォーカスしていたカードを、盤に下ろす
                gameViewModel.ResetFocusCardOfPlayerHand(gameModel, player, indexOfFocusedHandCard);
            }

            if (0 <= current && current < gameModelBuffer.IdOfCardsOfPlayersHand[player].Count) // 範囲内なら
            {
                // 今回フォーカスするカードを持ち上げる
                gameViewModel.SetFocusCardOfPlayerHand(gameModel, player, current);
            }
        }
    }
}
