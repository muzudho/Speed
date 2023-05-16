namespace Assets.Scripts.Scheduler.AnalogCommands.O4thComplex
{
    using Assets.Scripts.Coding;
    using Assets.Scripts.ThinkingEngine.Models;
    using Assets.Scripts.Vision.Models;
    using System;
    using System.Collections.Generic;
    using ModelOfAnalogCommand1stTimelineSpan = Assets.Scripts.Scheduler.AnalogCommands.O1stTimelineSpan;
    using ModelOfAnalogCommand3rdSimplex = Assets.Scripts.Scheduler.AnalogCommands.O3rdSimplex;
    using ModelOfAnalogCommands = Assets.Scripts.Scheduler.AnalogCommands;
    using ModelOfDigitalCommands = Assets.Scripts.ThinkingEngine.DigitalCommands;
    using ModelOfGameBuffer = Assets.Scripts.ThinkingEngine.Models.Game.Buffer;
    using ModelOfGameWriter = Assets.Scripts.ThinkingEngine.Models.Game.Writer;
    using ModelOfInput = Assets.Scripts.Vision.Models.Input;
    using ModelOfThinkingEngineCommons = Assets.Scripts.ThinkingEngine.Commons;

    /// <summary>
    /// 右（または左）側の台札１枚を、手札へ移動する
    /// </summary>
    class MoveCardsToPileFromCenterStacks : ItsAbstract
    {
        // - その他

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="startObj"></param>
        /// <param name="digitalCommand"></param>
        public MoveCardsToPileFromCenterStacks(
            GameSeconds startObj,
            ModelOfDigitalCommands.IModel digitalCommand)
            : base(startObj, digitalCommand)
        {
        }

        // - メソッド

        /// <summary>
        /// ゲーム画面の同期を始めます
        /// 
        /// - 台札を、手札へ移動する
        /// - ゲーム開始時に使う
        /// </summary>
        /// <param name="place">右:0, 左:1</param>
        public override List<ModelOfAnalogCommand1stTimelineSpan.IModel> GenerateSpan(
            ModelOfGameBuffer.Model gameModelBuffer,
            ModelOfGameWriter.Model gameModelWriter,
            ModelOfInput.Init inputModel,
            ModelOfAnalogCommands.Model schedulerModel,
            LazyArgs.SetValue<ModelOfAnalogCommand1stTimelineSpan.IModel> setTimespan)
        {
            var result = new List<ModelOfAnalogCommand1stTimelineSpan.IModel>();

            var digitalCommand = (ModelOfDigitalCommands.MoveCardsToPileFromCenterStacks)this.DigitalCommand;

            // 台札の一番上（一番後ろ）のカードを１枚抜く
            var numberOfCards = 1;
            var length = gameModelBuffer.GetCenterStack(digitalCommand.PlaceObj).IdOfCards.Count; // 台札の枚数
            if (1 <= length)
            {
                var startIndexObj = new CenterStackCardIndex(length - numberOfCards);
                var idOfCardOfCenterStack = gameModelBuffer.GetCenterStack(digitalCommand.PlaceObj).IdOfCards[startIndexObj.AsInt]; // 台札の１番上のカード
                gameModelWriter.GetCenterStack(digitalCommand.PlaceObj).RemoveCardAt(startIndexObj);

                // 黒いカードは１プレイヤー、赤いカードは２プレイヤー
                Player playerObj;
                var suit = idOfCardOfCenterStack.Suit();
                switch (suit)
                {
                    case IdOfCardSuits.Clubs:
                    case IdOfCardSuits.Spades:
                        playerObj = ModelOfThinkingEngineCommons.Player1;
                        break;

                    case IdOfCardSuits.Diamonds:
                    case IdOfCardSuits.Hearts:
                        playerObj = ModelOfThinkingEngineCommons.Player2;
                        break;

                    default:
                        throw new Exception();
                }

                // プレイヤーの手札を積み上げる
                gameModelWriter.GetPlayer(playerObj).AddCardOfPile(idOfCardOfCenterStack);

                setTimespan(ModelOfAnalogCommand3rdSimplex.PutCardToPile.CreateTimespan(
                    timeRange: this.TimeRangeObj,
                    playerObj: playerObj,
                    idOfPlayerPileCards: gameModelBuffer.GetPlayer(playerObj).IdOfCardsOfPile,
                    idOfPlayingCard: idOfCardOfCenterStack)); // 台札から手札へ移動するカード
            }

            return result;
        }
    }
}

