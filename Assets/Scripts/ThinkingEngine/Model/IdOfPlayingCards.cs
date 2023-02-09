namespace Assets.Scripts.ThinkingEngine.Model
{
    using System;

    /// <summary>
    /// トランプのカード
    /// 
    /// - ジョーカーを除く
    /// </summary>
    internal enum IdOfPlayingCards
    {
        None,

        Clubs1,
        Clubs2,
        Clubs3,
        Clubs4,
        Clubs5,
        Clubs6,
        Clubs7,
        Clubs8,
        Clubs9,
        Clubs10,
        Clubs11,
        Clubs12,
        Clubs13,

        Diamonds1,
        Diamonds2,
        Diamonds3,
        Diamonds4,
        Diamonds5,
        Diamonds6,
        Diamonds7,
        Diamonds8,
        Diamonds9,
        Diamonds10,
        Diamonds11,
        Diamonds12,
        Diamonds13,

        Hearts1,
        Hearts2,
        Hearts3,
        Hearts4,
        Hearts5,
        Hearts6,
        Hearts7,
        Hearts8,
        Hearts9,
        Hearts10,
        Hearts11,
        Hearts12,
        Hearts13,

        Spades1,
        Spades2,
        Spades3,
        Spades4,
        Spades5,
        Spades6,
        Spades7,
        Spades8,
        Spades9,
        Spades10,
        Spades11,
        Spades12,
        Spades13,
    }

    static class IdOfPlayingCardsExtensions
    {
        public static IdOfCardSuits Suit(this IdOfPlayingCards idOfCard)
        {
            switch (idOfCard)
            {
                case IdOfPlayingCards.Clubs1:
                case IdOfPlayingCards.Clubs2:
                case IdOfPlayingCards.Clubs3:
                case IdOfPlayingCards.Clubs4:
                case IdOfPlayingCards.Clubs5:
                case IdOfPlayingCards.Clubs6:
                case IdOfPlayingCards.Clubs7:
                case IdOfPlayingCards.Clubs8:
                case IdOfPlayingCards.Clubs9:
                case IdOfPlayingCards.Clubs10:
                case IdOfPlayingCards.Clubs11:
                case IdOfPlayingCards.Clubs12:
                case IdOfPlayingCards.Clubs13:
                    return IdOfCardSuits.Clubs;

                case IdOfPlayingCards.Diamonds1:
                case IdOfPlayingCards.Diamonds2:
                case IdOfPlayingCards.Diamonds3:
                case IdOfPlayingCards.Diamonds4:
                case IdOfPlayingCards.Diamonds5:
                case IdOfPlayingCards.Diamonds6:
                case IdOfPlayingCards.Diamonds7:
                case IdOfPlayingCards.Diamonds8:
                case IdOfPlayingCards.Diamonds9:
                case IdOfPlayingCards.Diamonds10:
                case IdOfPlayingCards.Diamonds11:
                case IdOfPlayingCards.Diamonds12:
                case IdOfPlayingCards.Diamonds13:
                    return IdOfCardSuits.Diamonds;

                case IdOfPlayingCards.Hearts1:
                case IdOfPlayingCards.Hearts2:
                case IdOfPlayingCards.Hearts3:
                case IdOfPlayingCards.Hearts4:
                case IdOfPlayingCards.Hearts5:
                case IdOfPlayingCards.Hearts6:
                case IdOfPlayingCards.Hearts7:
                case IdOfPlayingCards.Hearts8:
                case IdOfPlayingCards.Hearts9:
                case IdOfPlayingCards.Hearts10:
                case IdOfPlayingCards.Hearts11:
                case IdOfPlayingCards.Hearts12:
                case IdOfPlayingCards.Hearts13:
                    return IdOfCardSuits.Hearts;

                case IdOfPlayingCards.Spades1:
                case IdOfPlayingCards.Spades2:
                case IdOfPlayingCards.Spades3:
                case IdOfPlayingCards.Spades4:
                case IdOfPlayingCards.Spades5:
                case IdOfPlayingCards.Spades6:
                case IdOfPlayingCards.Spades7:
                case IdOfPlayingCards.Spades8:
                case IdOfPlayingCards.Spades9:
                case IdOfPlayingCards.Spades10:
                case IdOfPlayingCards.Spades11:
                case IdOfPlayingCards.Spades12:
                case IdOfPlayingCards.Spades13:
                    return IdOfCardSuits.Spades;

                default: throw new ArgumentOutOfRangeException("idOfCard");
            }
        }

        public static int Number(this IdOfPlayingCards idOfCard)
        {
            switch (idOfCard)
            {
                case IdOfPlayingCards.Clubs1:
                case IdOfPlayingCards.Diamonds1:
                case IdOfPlayingCards.Hearts1:
                case IdOfPlayingCards.Spades1:
                    return 1;

                case IdOfPlayingCards.Clubs2:
                case IdOfPlayingCards.Diamonds2:
                case IdOfPlayingCards.Hearts2:
                case IdOfPlayingCards.Spades2:
                    return 2;

                case IdOfPlayingCards.Clubs3:
                case IdOfPlayingCards.Diamonds3:
                case IdOfPlayingCards.Hearts3:
                case IdOfPlayingCards.Spades3:
                    return 3;

                case IdOfPlayingCards.Clubs4:
                case IdOfPlayingCards.Diamonds4:
                case IdOfPlayingCards.Hearts4:
                case IdOfPlayingCards.Spades4:
                    return 4;

                case IdOfPlayingCards.Clubs5:
                case IdOfPlayingCards.Diamonds5:
                case IdOfPlayingCards.Hearts5:
                case IdOfPlayingCards.Spades5:
                    return 5;

                case IdOfPlayingCards.Clubs6:
                case IdOfPlayingCards.Diamonds6:
                case IdOfPlayingCards.Hearts6:
                case IdOfPlayingCards.Spades6:
                    return 6;

                case IdOfPlayingCards.Clubs7:
                case IdOfPlayingCards.Diamonds7:
                case IdOfPlayingCards.Hearts7:
                case IdOfPlayingCards.Spades7:
                    return 7;

                case IdOfPlayingCards.Clubs8:
                case IdOfPlayingCards.Diamonds8:
                case IdOfPlayingCards.Hearts8:
                case IdOfPlayingCards.Spades8:
                    return 8;

                case IdOfPlayingCards.Clubs9:
                case IdOfPlayingCards.Diamonds9:
                case IdOfPlayingCards.Hearts9:
                case IdOfPlayingCards.Spades9:
                    return 9;

                case IdOfPlayingCards.Clubs10:
                case IdOfPlayingCards.Diamonds10:
                case IdOfPlayingCards.Hearts10:
                case IdOfPlayingCards.Spades10:
                    return 10;

                case IdOfPlayingCards.Clubs11:
                case IdOfPlayingCards.Diamonds11:
                case IdOfPlayingCards.Hearts11:
                case IdOfPlayingCards.Spades11:
                    return 11;

                case IdOfPlayingCards.Clubs12:
                case IdOfPlayingCards.Diamonds12:
                case IdOfPlayingCards.Hearts12:
                case IdOfPlayingCards.Spades12:
                    return 12;

                case IdOfPlayingCards.Clubs13:
                case IdOfPlayingCards.Diamonds13:
                case IdOfPlayingCards.Hearts13:
                case IdOfPlayingCards.Spades13:
                    return 13;

                default: throw new ArgumentOutOfRangeException("idOfCard");
            }
        }
    }
}
