namespace Assets.Scripts.Vision.Models.Scheduler.O4thCommands
{
    using Assets.Scripts.Coding;
    using Assets.Scripts.ThinkingEngine;
    using Assets.Scripts.ThinkingEngine.Models;
    using System;
    using ModelOfSchedulerO1stTimelineSpan = Assets.Scripts.Vision.Models.Scheduler.O1stTimelineSpan;
    using ModelOfSchedulerO3rdViewCommand = Assets.Scripts.Vision.Models.Scheduler.O3rdViewCommand;
    using ModelOfThinkingEngineCommand = Assets.Scripts.ThinkingEngine.Models.Commands;

    /// <summary>
    /// 右（または左）側の台札１枚を、手札へ移動する
    /// </summary>
    class MoveCardsToPileFromCenterStacks : ItsAbstract
    {
        // - 生成

        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        public override IModel NewThis(ModelOfThinkingEngineCommand.IModel commandOfThinkingEngine)
        {
            return new MoveCardsToPileFromCenterStacks();
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
            ITask task,
            GameModelBuffer gameModelBuffer,
            LazyArgs.SetValue<ModelOfSchedulerO1stTimelineSpan.IModel> setTimelineSpan)
        {
            // 台札の一番上（一番後ろ）のカードを１枚抜く
            var numberOfCards = 1;
            var length = gameModelBuffer.IdOfCardsOfCenterStacks[GetCommandOfThinkingEngine(task).PlaceObj.AsInt].Count; // 台札の枚数
            if (1 <= length)
            {
                var startIndexObj = new CenterStackCardIndex(length - numberOfCards);
                var idOfCardOfCenterStack = gameModelBuffer.IdOfCardsOfCenterStacks[GetCommandOfThinkingEngine(task).PlaceObj.AsInt][startIndexObj.AsInt]; // 台札の１番上のカード
                gameModelBuffer.RemoveCardAtOfCenterStack(GetCommandOfThinkingEngine(task).PlaceObj, startIndexObj);

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

                setTimelineSpan(ModelOfSchedulerO3rdViewCommand.PutCardToPile.GenerateSpan(
                    startSeconds: task.StartSeconds,
                    duration: CommandDurationMapping.GetDurationBy(task.CommandOfScheduler.CommandOfThinkingEngine.GetType()),
                    playerObj: playerObj,
                    idOfPlayerPileCards: gameModelBuffer.IdOfCardsOfPlayersPile[playerObj.AsInt],
                    idOfPlayingCard: idOfCardOfCenterStack)); // 台札から手札へ移動するカード
            }
        }

        ModelOfThinkingEngineCommand.MoveCardsToPileFromCenterStacks GetCommandOfThinkingEngine(ITask task)
        {
            return (ModelOfThinkingEngineCommand.MoveCardsToPileFromCenterStacks)task.CommandOfScheduler.CommandOfThinkingEngine;
        }
    }
}
