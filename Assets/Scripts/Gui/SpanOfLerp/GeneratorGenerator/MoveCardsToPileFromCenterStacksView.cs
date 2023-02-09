namespace Assets.Scripts.Gui.SpanOfLerp.GeneratorGenerator
{
    using Assets.Scripts.Gui.SpanOfLerp.Generator;
    using Assets.Scripts.ThinkingEngine.Model;
    using Assets.Scripts.ThinkingEngine.Model.CommandArgs;
    using System;
    using SimulatorsOfTimeline = Assets.Scripts.Gui.SpanOfLerp.TimedGenerator;
    using SpanOfLeap = Assets.Scripts.Gui.SpanOfLerp;

    /// <summary>
    /// 右（または左）側の台札１枚を、手札へ移動する
    /// </summary>
    class MoveCardsToPileFromCenterStacksView : AbstractSpanGenerator
    {
        // - 生成

        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        public override ISpanGenerator Spawn()
        {
            return new MoveCardsToPileFromCenterStacksView();
        }

        // - プロパティ

        MoveCardsToPileFromCenterStacksModel GetModel(SimulatorsOfTimeline.TimedGenerator timedGenerator)
        {
            return (MoveCardsToPileFromCenterStacksModel)timedGenerator.TimedCommandArg.CommandArg;
        }

        // - メソッド

        /// <summary>
        /// ゲーム画面の同期を始めます
        /// 
        /// - 台札を、手札へ移動する
        /// - ゲーム開始時に使う
        /// </summary>
        /// <param name="place">右:0, 左:1</param>
        public override void CreateSpanToLerp(
            SimulatorsOfTimeline.TimedGenerator timedGenerator,
            GameModelBuffer gameModelBuffer,
            LazyArgs.SetValue<SpanOfLeap.Model> setViewMovement)
        {
            // 台札の一番上（一番後ろ）のカードを１枚抜く
            var numberOfCards = 1;
            var length = gameModelBuffer.IdOfCardsOfCenterStacks[GetModel(timedGenerator).Place].Count; // 台札の枚数
            if (1 <= length)
            {
                var startIndex = length - numberOfCards;
                var idOfCardOfCenterStack = gameModelBuffer.IdOfCardsOfCenterStacks[GetModel(timedGenerator).Place][startIndex]; // 台札の１番上のカード
                gameModelBuffer.RemoveCardAtOfCenterStack(GetModel(timedGenerator).Place, startIndex);

                // 黒いカードは１プレイヤー、赤いカードは２プレイヤー
                int player;
                var suit = idOfCardOfCenterStack.Suit();
                switch (suit)
                {
                    case IdOfCardSuits.Clubs:
                    case IdOfCardSuits.Spades:
                        player = 0;
                        break;

                    case IdOfCardSuits.Diamonds:
                    case IdOfCardSuits.Hearts:
                        player = 1;
                        break;

                    default:
                        throw new Exception();
                }

                // プレイヤーの手札を積み上げる
                gameModelBuffer.AddCardOfPlayersPile(player, idOfCardOfCenterStack);

                setViewMovement(PutCardToPile.Generate(
                    startSeconds: timedGenerator.StartSeconds,
                    duration: timedGenerator.TimedCommandArg.Duration,
                    player: player,
                    idOfPlayerPileCards: gameModelBuffer.IdOfCardsOfPlayersPile[player],
                    idOfPlayingCard: idOfCardOfCenterStack)); // 台札から手札へ移動するカード
            }
        }
    }
}
