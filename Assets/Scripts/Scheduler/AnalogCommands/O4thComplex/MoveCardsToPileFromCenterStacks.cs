namespace Assets.Scripts.Scheduler.AnalogCommands.O4thComplex
{
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

        // 静的フィールド

        // 台札の一番上（一番後ろ）のカードを１枚抜く
        static readonly int numberOfCards = 1;

        // フィールド

        int lengthOfTargetCenterStack;
        IdOfPlayingCards idOfCardOfTargetCenterStack;
        List<IdOfPlayingCards> idOfPlayerPileCards;

        // - メソッド

        /// <summary>
        /// 準備
        /// </summary>
        public override void Setup(ModelOfGameBuffer.Model gameModelBuffer)
        {
            var digitalCommand = (ModelOfDigitalCommands.MoveCardsToPileFromCenterStacks)this.DigitalCommand;

            // 台札の枚数
            this.lengthOfTargetCenterStack = gameModelBuffer.GetCenterStack(digitalCommand.PlaceObj).IdOfCards.Count;

            if (1 <= this.lengthOfTargetCenterStack)
            {
                var startIndexObj = new CenterStackCardIndex(this.lengthOfTargetCenterStack - numberOfCards);

                // 台札の１番上のカード
                this.idOfCardOfTargetCenterStack = gameModelBuffer.GetCenterStack(digitalCommand.PlaceObj).IdOfCards[startIndexObj.AsInt];
            }

            {
                // 黒いカードは１プレイヤー、赤いカードは２プレイヤー
                Player playerObj = GetPlayerByCardColor();

                this.idOfPlayerPileCards = gameModelBuffer.GetPlayer(playerObj).IdOfCardsOfPile;
            }
        }

        /// <summary>
        /// ゲーム画面の同期を始めます
        /// 
        /// - 台札を、手札へ移動する
        /// - ゲーム開始時に使う
        /// </summary>
        /// <param name="place">右:0, 左:1</param>
        public override List<ModelOfAnalogCommand1stTimelineSpan.IModel> CreateTimespanList(
            ModelOfGameBuffer.Model _gameModelBuffer,
            ModelOfGameWriter.Model gameModelWriter,
            ModelOfInput.Init inputModel,
            ModelOfAnalogCommands.Model schedulerModel)
        {
            var result = new List<ModelOfAnalogCommand1stTimelineSpan.IModel>();

            var digitalCommand = (ModelOfDigitalCommands.MoveCardsToPileFromCenterStacks)this.DigitalCommand;

            if (1 <= this.lengthOfTargetCenterStack)
            {
                var startIndexObj = new CenterStackCardIndex(this.lengthOfTargetCenterStack - numberOfCards);

                gameModelWriter.GetCenterStack(digitalCommand.PlaceObj).RemoveCardAt(startIndexObj);

                // 黒いカードは１プレイヤー、赤いカードは２プレイヤー
                Player playerObj = GetPlayerByCardColor();

                // プレイヤーの手札を積み上げる
                gameModelWriter.GetPlayer(playerObj).AddCardOfPile(this.idOfCardOfTargetCenterStack);

                result.Add(ModelOfAnalogCommand3rdSimplex.PutCardToPile.CreateTimespan(
                    timeRange: this.TimeRangeObj,
                    playerObj: playerObj,
                    idOfPlayerPileCards: this.idOfPlayerPileCards,
                    idOfPlayingCard: this.idOfCardOfTargetCenterStack)); // 台札から手札へ移動するカード
            }

            return result;
        }

        /// <summary>
        /// 黒いカードは１プレイヤー、赤いカードは２プレイヤー
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        Player GetPlayerByCardColor()
        {
            var suit = this.idOfCardOfTargetCenterStack.Suit();
            switch (suit)
            {
                case IdOfCardSuits.Clubs:
                case IdOfCardSuits.Spades:
                    return ModelOfThinkingEngineCommons.Player1;

                case IdOfCardSuits.Diamonds:
                case IdOfCardSuits.Hearts:
                    return ModelOfThinkingEngineCommons.Player2;

                default:
                    throw new Exception();
            }
        }
    }
}

