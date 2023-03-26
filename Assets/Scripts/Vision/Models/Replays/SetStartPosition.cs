namespace Assets.Scripts.Vision.Models.Replays
{
    using Assets.Scripts.ThinkingEngine;
    using ModelOfGame = Assets.Scripts.ThinkingEngine.Models.Game;
    using ModelOfGameBuffer = Assets.Scripts.ThinkingEngine.Models.GameBuffer;
    using ModelOfInput = Assets.Scripts.Vision.Models.Input;
    using ModelOfScheduler = Assets.Scripts.Vision.Models.Scheduler;
    using ModelOfSchedulerO6thCommandMapping = Assets.Scripts.Vision.Models.Scheduler.O6thCommandMapping;
    using ModelOfThinkingEngineCommand = Assets.Scripts.ThinkingEngine.Models.Commands;

    /// <summary>
    /// 開始局面まで
    /// </summary>
    static class SetStartPosition
    {
        internal static void DoIt(
            ModelOfGameBuffer.Model modelBuffer,
            ModelOfInput.Init inputModel,
            ModelOfScheduler.Model schedulerModel)
        {
            var model = new ModelOfGame.Default(modelBuffer);

            // とりあえず右の台札
            while (0 < model.GetCenterStack(Commons.RightCenterStack).GetLengthOfCards())
            {
                // 即実行
                // ======

                // コマンド作成（思考エンジン用）
                var commandOfThinkingEngine = new ModelOfThinkingEngineCommand.MoveCardsToPileFromCenterStacks(
                        placeObj: Commons.RightCenterStack);

                // コマンド作成（画面用）
                var commandOfScheduler = ModelOfSchedulerO6thCommandMapping.Model.WrapCommand(
                    startObj: GameSeconds.Zero,
                    command: commandOfThinkingEngine);

                // タイムスパン作成・登録
                commandOfScheduler.GenerateSpan(
                    gameModelBuffer: modelBuffer,
                    inputModel: inputModel,
                    schedulerModel: schedulerModel,
                    setTimespan: (timespan) => timespan.Lerp(1.0f));
            }

            // １，２プレイヤーについて、手札から５枚抜いて、場札として置く（画面上の場札の位置は調整される）
            {
                var playerObj = Commons.Player1;
                var parameter = new ModelOfThinkingEngineCommand.MoveCardsToHandFromPile(
                        playerObj: playerObj,
                        numberOfCards: 5);
                schedulerModel.Timeline.AddWithinScheduler(playerObj, parameter);
            }
            {
                var playerObj = Commons.Player2;
                var parameter = new ModelOfThinkingEngineCommand.MoveCardsToHandFromPile(
                        playerObj: playerObj,
                        numberOfCards: 5);
                schedulerModel.Timeline.AddWithinScheduler(playerObj, parameter);
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
                    // １プレイヤーが、ピックアップ中の場札を抜いて、右の台札へ積み上げる
                    var playerObj = Commons.Player1;
                    var spanModel = new ModelOfThinkingEngineCommand.MoveCardToCenterStackFromHand(
                            playerObj: playerObj, // １プレイヤーが
                            placeObj: Commons.RightCenterStack); // 右の
                    schedulerModel.Timeline.AddWithinScheduler(playerObj, spanModel);
                }
                {
                    // ２プレイヤーが、ピックアップ中の場札を抜いて、左の台札へ積み上げる
                    var playerObj = Commons.Player2;
                    var spanModel = new ModelOfThinkingEngineCommand.MoveCardToCenterStackFromHand(
                            playerObj: playerObj, // ２プレイヤーが
                            placeObj: Commons.LeftCenterStack); // 左の;
                    schedulerModel.Timeline.AddWithinScheduler(playerObj, spanModel);
                }
            }

            // 対局開始の合図
            {
                var command = new ModelOfThinkingEngineCommand.SetGameActive(
                    isGameActive: true);

                {
                    var playerObj = Commons.Player1; // どっちでもいいが、とりあえず、プレイヤー１に　合図を出させる
                    schedulerModel.Timeline.AddWithinScheduler(playerObj, command);
                }
                {
                    var playerObj = Commons.Player2; // プレイヤー２も、間を合わせる
                    schedulerModel.Timeline.AddScheduleSeconds(
                        playerObj: playerObj,
                        time: ModelOfScheduler.CommandDurationMapping.GetDurationBy(command.GetType()));
                }
            }

        }
    }
}
