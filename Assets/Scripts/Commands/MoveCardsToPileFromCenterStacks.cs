namespace Assets.Scripts.Commands
{
    using Assets.Scripts.Models;
    using Assets.Scripts.Views;
    using System;

    static class MoveCardsToPileFromCenterStacks
    {
        /// <summary>
        /// 台札を、手札へ移動する
        /// 
        /// - ゲーム開始時に使う
        /// </summary>
        /// <param name="place">右:0, 左:1</param>
        internal static void DoIt(GameModelBuffer gameModelBuffer, GameViewModel gameViewModel, int place)
        {
            // 台札の一番上（一番後ろ）のカードを１枚抜く
            var numberOfCards = 1;
            var length = gameModelBuffer.IdOfCardsOfCenterStacks[place].Count; // 台札の枚数
            if (1 <= length)
            {
                var startIndex = length - numberOfCards;
                var idOfCard = gameModelBuffer.IdOfCardsOfCenterStacks[place][startIndex];
                gameModelBuffer.RemoveCardAtOfCenterStack(place, startIndex);

                // 黒いカードは１プレイヤー、赤いカードは２プレイヤー
                int player;
                float angleY;
                var goCard = ViewStorage.PlayingCards[idOfCard];
                if (goCard.name.StartsWith("Clubs") || goCard.name.StartsWith("Spades"))
                {
                    player = 0;
                    angleY = 180.0f;
                }
                else if (goCard.name.StartsWith("Diamonds") || goCard.name.StartsWith("Hearts"))
                {
                    player = 1;
                    angleY = 0.0f;
                }
                else
                {
                    throw new Exception();
                }

                // プレイヤーの手札を積み上げる
                gameModelBuffer.AddCardOfPlayersPile(player, idOfCard);
                gameViewModel.SetPosRot(idOfCard, gameViewModel.pileCardsX[player], gameViewModel.pileCardsY[player], gameViewModel.pileCardsZ[player], angleY: angleY, angleZ: 180.0f);
                gameViewModel.pileCardsY[player] += 0.2f;
            }
        }
    }
}
