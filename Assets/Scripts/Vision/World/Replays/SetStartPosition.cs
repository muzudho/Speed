﻿namespace Assets.Scripts.Vision.World.Replays
{
    using Assets.Scripts.ThinkingEngine.Models;
    using ModelOfGame = Assets.Scripts.ThinkingEngine.Models.Game;
    using Assets.Scripts.ThinkingEngine.Models.CommandArgs;
    using Assets.Scripts.Vision.World.SpanOfLerp.TimedGenerator;
    using GuiOfTimedCommandArgs = Assets.Scripts.Vision.World.TimedCommandArgs;
    using TimedGeneratorOfSpanOfLearp = Assets.Scripts.Vision.World.SpanOfLerp.TimedGenerator;
    using Assets.Scripts.ThinkingEngine;

    /// <summary>
    /// 開始局面まで
    /// </summary>
    static class SetStartPosition
    {
        internal static void DoIt(GameModelBuffer modelBuffer, ScheduleRegister scheduleRegister)
        {
            var model = new ModelOfGame.Default(modelBuffer);

            const int right = 0;// 台札の右
            const int left = 1;// 台札の左

            while (0 < model.GetLengthOfCenterStackCards(right))
            {
                // 即実行
                var timedCommandArg = new GuiOfTimedCommandArgs.Model(new MoveCardsToPileFromCenterStacksModel(
                        place: right
                        ));
                var timedGenerator = new TimedGeneratorOfSpanOfLearp.TimedGenerator(
                        startSeconds: 0.0f,
                        timedCommandArg: timedCommandArg,
                        spanGenerator: TimedGeneratorOfSpanOfLearp.Mapping.SpawnViewFromModel(timedCommandArg.GetType()));
                timedGenerator.SpanGenerator.CreateSpanToLerp(
                    timedGenerator,
                    modelBuffer,
                    setSpanToLerp: (movementViewModel) => movementViewModel.Lerp(1.0f));
            }

            // １，２プレイヤーについて、手札から５枚抜いて、場札として置く（画面上の場札の位置は調整される）
            {
                var playerObj = Commons.Player1;
                var spanModel = new MoveCardsToHandFromPileModel(
                        playerObj: playerObj,
                        numberOfCards: 5);
                scheduleRegister.AddWithinScheduler(playerObj, spanModel);
            }
            {
                var playerObj = Commons.Player2;
                var spanModel = new MoveCardsToHandFromPileModel(
                        playerObj: playerObj,
                        numberOfCards: 5);
                scheduleRegister.AddWithinScheduler(playerObj, spanModel);
            }

            // 間
            float interval = 0.85f;

            // 間
            {
                foreach (var playerObj in Commons.Players)
                {
                    scheduleRegister.AddScheduleSeconds(playerObj: playerObj, seconds: interval);
                }
            }

            // 登録：ピックアップ場札を、台札へ積み上げる
            {
                {
                    // １プレイヤーが、ピックアップ中の場札を抜いて、右の台札へ積み上げる
                    var playerObj = Commons.Player1;
                    var spanModel = new MoveCardToCenterStackFromHandModel(
                            playerObj: playerObj, // １プレイヤーが
                            place: right); // 右の
                    scheduleRegister.AddWithinScheduler(playerObj, spanModel);
                }
                {
                    // ２プレイヤーが、ピックアップ中の場札を抜いて、左の台札へ積み上げる
                    var playerObj = Commons.Player2;
                    var spanModel = new MoveCardToCenterStackFromHandModel(
                            playerObj: playerObj, // ２プレイヤーが
                            place: left); // 左の;
                    scheduleRegister.AddWithinScheduler(playerObj, spanModel);
                }
            }

            // 対局開始の合図
            {
                var spanModel = new SetGameActive(
                    isGameActive: true);

                {
                    var playerObj = Commons.Player1; // どっちでもいいが、とりあえず、プレイヤー１に　合図を出させる
                    scheduleRegister.AddWithinScheduler(playerObj, spanModel);
                }
                {
                    var playerObj = Commons.Player2; // プレイヤー２も、間を合わせる
                    scheduleRegister.AddScheduleSeconds(
                        playerObj: playerObj,
                        seconds: new GuiOfTimedCommandArgs.Model(spanModel).Duration);
                }
            }

        }
    }
}
