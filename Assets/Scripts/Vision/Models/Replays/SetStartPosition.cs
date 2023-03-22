namespace Assets.Scripts.Vision.Models.Replays
{
    using Assets.Scripts.ThinkingEngine;
    using Assets.Scripts.ThinkingEngine.Models;
    using ModelOfGame = Assets.Scripts.ThinkingEngine.Models.Game;
    using ModelOfScheduler = Assets.Scripts.Vision.Models.Scheduler;
    using ModelOfSchedulerO5thTask = Assets.Scripts.Vision.Models.Scheduler.O5thTask;
    using ModelOfSchedulerO6thCommandMapping = Assets.Scripts.Vision.Models.Scheduler.O6thCommandMapping;
    using ModelOfSchedulerO7thTimeline = Assets.Scripts.Vision.Models.Scheduler.O7thTimeline;
    using ModelOfThinkingEngineCommand = Assets.Scripts.ThinkingEngine.Models.Commands;

    /// <summary>
    /// 開始局面まで
    /// </summary>
    static class SetStartPosition
    {
        internal static void DoIt(GameModelBuffer modelBuffer, ModelOfSchedulerO7thTimeline.Model timeline)
        {
            var model = new ModelOfGame.Default(modelBuffer);

            while (0 < model.GetLengthOfCenterStackCards(Commons.RightCenterStack))
            {
                // 即実行
                var commandOfThinkingEngine = new ModelOfThinkingEngineCommand.MoveCardsToPileFromCenterStacks(
                        placeObj: Commons.RightCenterStack);
                var task = new ModelOfSchedulerO5thTask.Model(
                        startSeconds: 0.0f,
                        commandOfScheduler: ModelOfSchedulerO6thCommandMapping.Model.NewSourceCodeFromModel(commandOfThinkingEngine));
                task.CommandOfScheduler.GenerateSpan(
                    task,
                    modelBuffer,
                    setTimelineSpan: (movementViewModel) => movementViewModel.Lerp(1.0f));
            }

            // １，２プレイヤーについて、手札から５枚抜いて、場札として置く（画面上の場札の位置は調整される）
            {
                var playerObj = Commons.Player1;
                var parameter = new ModelOfThinkingEngineCommand.MoveCardsToHandFromPile(
                        playerObj: playerObj,
                        numberOfCards: 5);
                timeline.AddWithinScheduler(playerObj, parameter);
            }
            {
                var playerObj = Commons.Player2;
                var parameter = new ModelOfThinkingEngineCommand.MoveCardsToHandFromPile(
                        playerObj: playerObj,
                        numberOfCards: 5);
                timeline.AddWithinScheduler(playerObj, parameter);
            }

            // 間
            float interval = 0.85f;

            // 間
            {
                foreach (var playerObj in Commons.Players)
                {
                    timeline.AddScheduleSeconds(playerObj: playerObj, seconds: interval);
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
                    timeline.AddWithinScheduler(playerObj, spanModel);
                }
                {
                    // ２プレイヤーが、ピックアップ中の場札を抜いて、左の台札へ積み上げる
                    var playerObj = Commons.Player2;
                    var spanModel = new ModelOfThinkingEngineCommand.MoveCardToCenterStackFromHand(
                            playerObj: playerObj, // ２プレイヤーが
                            placeObj: Commons.LeftCenterStack); // 左の;
                    timeline.AddWithinScheduler(playerObj, spanModel);
                }
            }

            // 対局開始の合図
            {
                var command = new ModelOfThinkingEngineCommand.SetGameActive(
                    isGameActive: true);

                {
                    var playerObj = Commons.Player1; // どっちでもいいが、とりあえず、プレイヤー１に　合図を出させる
                    timeline.AddWithinScheduler(playerObj, command);
                }
                {
                    var playerObj = Commons.Player2; // プレイヤー２も、間を合わせる
                    timeline.AddScheduleSeconds(
                        playerObj: playerObj,
                        seconds: ModelOfScheduler.CommandDurationMapping.GetDurationBy(command.GetType()));
                }
            }

        }
    }
}
