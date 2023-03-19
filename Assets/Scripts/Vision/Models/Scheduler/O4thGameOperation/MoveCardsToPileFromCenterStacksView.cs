namespace Assets.Scripts.Vision.Models.Scheduler.O4thGameOperation
{
    using Assets.Scripts.Coding;
    using Assets.Scripts.ThinkingEngine;
    using Assets.Scripts.ThinkingEngine.Models;
    using Assets.Scripts.ThinkingEngine.Models.CommandArgs;
    using System;
    using ModelOfSchedulerO1stTimelineSpan = Assets.Scripts.Vision.Models.Scheduler.O1stTimelineSpan;
    using ModelOfSchedulerO3rdSpanGenerator = Assets.Scripts.Vision.Models.Scheduler.O3rdSpanGenerator;

    /// <summary>
    /// 右（または左）側の台札１枚を、手札へ移動する
    /// </summary>
    class MoveCardsToPileFromCenterStacksView : ItsAbstract
    {
        // - 生成

        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        public override IModel NewThis()
        {
            return new MoveCardsToPileFromCenterStacksView();
        }

        // - プロパティ

        MoveCardsToPileFromCenterStacksModel GetModel(IGameOperationSpan timedGenerator)
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
        public override void CreateSpan(
            IGameOperationSpan timedGenerator,
            GameModelBuffer gameModelBuffer,
            LazyArgs.SetValue<ModelOfSchedulerO1stTimelineSpan.IModel> setViewMovement)
        {
            // 台札の一番上（一番後ろ）のカードを１枚抜く
            var numberOfCards = 1;
            var length = gameModelBuffer.IdOfCardsOfCenterStacks[GetModel(timedGenerator).PlaceObj.AsInt].Count; // 台札の枚数
            if (1 <= length)
            {
                var startIndexObj = new CenterStackCardIndex(length - numberOfCards);
                var idOfCardOfCenterStack = gameModelBuffer.IdOfCardsOfCenterStacks[GetModel(timedGenerator).PlaceObj.AsInt][startIndexObj.AsInt]; // 台札の１番上のカード
                gameModelBuffer.RemoveCardAtOfCenterStack(GetModel(timedGenerator).PlaceObj, startIndexObj);

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

                setViewMovement(ModelOfSchedulerO3rdSpanGenerator.PutCardToPile.GenerateSpan(
                    startSeconds: timedGenerator.StartSeconds,
                    duration: timedGenerator.TimedCommandArg.Duration,
                    playerObj: playerObj,
                    idOfPlayerPileCards: gameModelBuffer.IdOfCardsOfPlayersPile[playerObj.AsInt],
                    idOfPlayingCard: idOfCardOfCenterStack)); // 台札から手札へ移動するカード
            }
        }
    }
}
