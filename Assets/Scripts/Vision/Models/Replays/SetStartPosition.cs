namespace Assets.Scripts.Vision.Models.Replays
{
    using Assets.Scripts.ThinkingEngine;
    using Assets.Scripts.ThinkingEngine.Models;
    using ModelOfGameBuffer = Assets.Scripts.ThinkingEngine.Models.Game.Buffer;
    using ModelOfGameWriter = Assets.Scripts.ThinkingEngine.Models.Game.Writer;
    using ModelOfInput = Assets.Scripts.Vision.Models.Input;
    using ModelOfObservableGame = Assets.Scripts.ThinkingEngine.Models.Game.Observable.Model;
    using ModelOfScheduler = Assets.Scripts.Scheduler.AnalogCommands;
    using ModelOfSchedulerO6thCommandMapping = Assets.Scripts.Scheduler.AnalogCommands.O6thDACommandMapping;
    using ModelOfThinkingEngineDigitalCommands = Assets.Scripts.ThinkingEngine.DigitalCommands;

    /// <summary>
    /// 開始局面まで
    /// </summary>
    static class SetStartPosition
    {
        internal static void DoIt(
            ModelOfGameBuffer.Model gameModelBuffer,
            ModelOfGameWriter.Model gameModelWriter,
            ModelOfInput.Init inputModel,
            ModelOfScheduler.Model schedulerModel)
        {
            var gameModel = new ModelOfObservableGame(gameModelBuffer);

            // とりあえず右の台札
            while (0 < gameModel.GetCenterStack(Commons.RightCenterStack).GetLengthOfCards())
            {
                // 即実行
                // ======

                //
                // 台札のカードを、手札へ分配
                //
                // デジタル・コマンド（思考エンジン内部用）　→　アナログ・コマンド（画面用）に変換
                //
                var digitalCommand = new ModelOfThinkingEngineDigitalCommands.MoveCardsToPileFromCenterStacks(
                        placeObj: Commons.RightCenterStack);
                var analogCommand = ModelOfSchedulerO6thCommandMapping.Model.WrapCommand(
                    startObj: GameSeconds.Zero,
                    digitalCommand: digitalCommand);

                // タイムスパン作成・登録
                analogCommand.GenerateSpan(
                    gameModelBuffer: gameModelBuffer,
                    gameModelWriter: gameModelWriter,
                    inputModel: inputModel,
                    schedulerModel: schedulerModel,
                    setTimespan: (timespan) => timespan.Lerp(1.0f));
            }

            // １，２プレイヤーについて、手札から５枚抜いて、場札として置く（画面上の場札の位置は調整される）
            {
                var playerObj = Commons.Player1;
                var digitalCommand = new ModelOfThinkingEngineDigitalCommands.MoveCardsToHandFromPile(
                        playerObj: playerObj,
                        numberOfCards: 5);
                schedulerModel.Timeline.AddWithinScheduler(playerObj, digitalCommand);
            }
            {
                var playerObj = Commons.Player2;
                var digitalCommand = new ModelOfThinkingEngineDigitalCommands.MoveCardsToHandFromPile(
                        playerObj: playerObj,
                        numberOfCards: 5);
                schedulerModel.Timeline.AddWithinScheduler(playerObj, digitalCommand);
            }

            // 間
            GameSeconds intervalObj = new GameSeconds(0.85f);

            // 間
            {
                foreach (var playerObj in Commons.Players)
                {
                    schedulerModel.Timeline.AddScheduleSeconds(playerObj: playerObj, time: intervalObj);
                }
            }

            // 登録：ピックアップ場札を、台札へ積み上げる
            {
                {
                    // １プレイヤーが
                    var playerObj = Commons.Player1;
                    // 右の台札へ
                    var placeObj = Commons.RightCenterStack;

                    if (CardMoveHelper.IsBoomerang(gameModel, playerObj, placeObj, out IdOfPlayingCards previousCard))
                    {
                        // ブーメラン
                    }
                    else
                    {
                        // １プレイヤーが、ピックアップ中の場札を抜いて、右の台札へ積み上げる
                        var digitalCommand = new ModelOfThinkingEngineDigitalCommands.MoveCardToCenterStackFromHand(
                                playerObj: playerObj,
                                placeObj: placeObj);
                        schedulerModel.Timeline.AddWithinScheduler(playerObj, digitalCommand);
                    }
                }
                {
                    // ２プレイヤーが
                    var playerObj = Commons.Player2;
                    // 左の台札へ
                    var placeObj = Commons.LeftCenterStack;

                    if (CardMoveHelper.IsBoomerang(gameModel, playerObj, placeObj, out IdOfPlayingCards previousCard))
                    {
                        // ブーメラン
                    }
                    else
                    {
                        // ２プレイヤーが、ピックアップ中の場札を抜いて、左の台札へ積み上げる
                        var digitalCommand = new ModelOfThinkingEngineDigitalCommands.MoveCardToCenterStackFromHand(
                            playerObj: playerObj,
                            placeObj: placeObj);
                        schedulerModel.Timeline.AddWithinScheduler(playerObj, digitalCommand);
                    }
                }
            }

            // 対局開始の合図
            {
                var digitalCommand = new ModelOfThinkingEngineDigitalCommands.SetGameActive(
                    isGameActive: true);

                {
                    var playerObj = Commons.Player1; // どっちでもいいが、とりあえず、プレイヤー１に　合図を出させる
                    schedulerModel.Timeline.AddWithinScheduler(playerObj, digitalCommand);
                }
                {
                    var playerObj = Commons.Player2; // プレイヤー２も、間を合わせる
                    schedulerModel.Timeline.AddScheduleSeconds(
                        playerObj: playerObj,
                        time: ModelOfScheduler.DurationMapping.GetDurationBy(digitalCommand.GetType()));
                }
            }

        }
    }
}
