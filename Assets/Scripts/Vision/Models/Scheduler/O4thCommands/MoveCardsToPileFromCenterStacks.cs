namespace Assets.Scripts.Vision.Models.Scheduler.O4thCommands
{
    using Assets.Scripts.Coding;
    using Assets.Scripts.ThinkingEngine;
    using Assets.Scripts.ThinkingEngine.Models;
    using System;
    using ModelOfGameBuffer = Assets.Scripts.ThinkingEngine.Models.GameBuffer;
    using ModelOfInput = Assets.Scripts.Vision.Models.Input;
    using ModelOfScheduler = Assets.Scripts.Vision.Models.Scheduler;
    using ModelOfSchedulerO1stTimelineSpan = Assets.Scripts.Vision.Models.Scheduler.O1stTimelineSpan;
    using ModelOfSchedulerO3rdViewCommand = Assets.Scripts.Vision.Models.Scheduler.O3rdViewCommand;
    using ModelOfThinkingEngineCommand = Assets.Scripts.ThinkingEngine.Models.Commands;

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
        /// <param name="command"></param>
        public MoveCardsToPileFromCenterStacks(
            GameSeconds startObj,
            ModelOfThinkingEngineCommand.IModel command)
            : base(startObj, command)
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
        public override void GenerateSpan(
            ModelOfGameBuffer.Model gameModelBuffer,
            ModelOfInput.Init inputModel,
            ModelOfScheduler.Model schedulerModel,
            LazyArgs.SetValue<ModelOfSchedulerO1stTimelineSpan.IModel> setTimespan)
        {
            var command = (ModelOfThinkingEngineCommand.MoveCardsToPileFromCenterStacks)this.CommandOfThinkingEngine;

            // 台札の一番上（一番後ろ）のカードを１枚抜く
            var numberOfCards = 1;
            var length = gameModelBuffer.IdOfCardsOfCenterStacks[command.PlaceObj.AsInt].Count; // 台札の枚数
            if (1 <= length)
            {
                var startIndexObj = new CenterStackCardIndex(length - numberOfCards);
                var idOfCardOfCenterStack = gameModelBuffer.IdOfCardsOfCenterStacks[command.PlaceObj.AsInt][startIndexObj.AsInt]; // 台札の１番上のカード
                gameModelBuffer.RemoveCardAtOfCenterStack(command.PlaceObj, startIndexObj);

                // 黒いカードは１プレイヤー、赤いカードは２プレイヤー
                Player playerObj;
                var suit = idOfCardOfCenterStack.Suit();
                switch (suit)
                {
                    case IdOfCardSuits.Clubs:
                    case IdOfCardSuits.Spades:
                        playerObj = Commons.Player1;
                        break;

                    case IdOfCardSuits.Diamonds:
                    case IdOfCardSuits.Hearts:
                        playerObj = Commons.Player2;
                        break;

                    default:
                        throw new Exception();
                }

                // プレイヤーの手札を積み上げる
                gameModelBuffer.AddCardOfPlayersPile(playerObj, idOfCardOfCenterStack);

                setTimespan(ModelOfSchedulerO3rdViewCommand.PutCardToPile.GenerateSpan(
                    timeRange: this.TimeRangeObj,
                    playerObj: playerObj,
                    idOfPlayerPileCards: gameModelBuffer.IdOfCardsOfPlayersPile[playerObj.AsInt],
                    idOfPlayingCard: idOfCardOfCenterStack)); // 台札から手札へ移動するカード
            }
        }
    }
}
