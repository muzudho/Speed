namespace Assets.Scripts.Vision.Models.Replays
{
    using Assets.Scripts.ThinkingEngine;
    using Assets.Scripts.ThinkingEngine.Models;
    using ModelOfObservableGame = Assets.Scripts.ThinkingEngine.Models.Game.Observable.Model;
    using ModelOfScheduler = Assets.Scripts.Scheduler.AnalogCommands;
    using ModelOfThinkingEngineCommands = Assets.Scripts.ThinkingEngine.DigitalCommands;

    static class Demo
    {
        // - メソッド

        /// <summary>
        /// タイムライン作成
        /// 
        /// - デモ
        /// </summary>
        static void SetupDemo(ModelOfObservableGame gameModel, ModelOfScheduler.Model schedulerModel)
        {
            // 卓準備

            // 間
            GameSeconds intervalObj = new GameSeconds(0.85f);

            // 間
            foreach (var playerObj in Commons.Players)
            {
                schedulerModel.Timeline.AddScheduleSeconds(playerObj: playerObj, time: intervalObj);
            }

            // ゲーム・デモ開始

            // 登録：カード選択
            {
                for (int i = 0; i < 2; i++)
                {
                    // １プレイヤーの右隣のカードへフォーカスを移します
                    {
                        var playerObj = Commons.Player1;
                        var digitalCommand = new ModelOfThinkingEngineCommands.MoveFocusToNextCard(
                                playerObj: playerObj,
                                directionObj: Commons.PickRight);
                        schedulerModel.Timeline.AddWithinScheduler(playerObj, digitalCommand);
                    }

                    // ２プレイヤーの右隣のカードへフォーカスを移します
                    {
                        var playerObj = Commons.Player2;
                        var digitalCommand = new ModelOfThinkingEngineCommands.MoveFocusToNextCard(
                                playerObj: playerObj,
                                directionObj: Commons.PickRight);
                        schedulerModel.Timeline.AddWithinScheduler(playerObj, digitalCommand);
                    }

                    // 間
                    foreach (var playerObj in Commons.Players)
                    {
                        schedulerModel.Timeline.AddScheduleSeconds(playerObj: playerObj, time: intervalObj);
                    }
                }
            }

            // 登録：台札を積み上げる
            {
                {
                    // １プレイヤーが
                    var playerObj = Commons.Player1;
                    // 左の台札
                    var placeObj = Commons.LeftCenterStack;

                    if (CardMoveHelper.IsBoomerang(gameModel, playerObj, placeObj, out IdOfPlayingCards previousCard))
                    {
                        // ブーメラン
                    }
                    else
                    {
                        var digitalCommand = new ModelOfThinkingEngineCommands.MoveCardToCenterStackFromHand(
                                playerObj: playerObj,
                                placeObj: placeObj);
                        schedulerModel.Timeline.AddWithinScheduler(playerObj, digitalCommand);
                    }
                }
                {
                    // ２プレイヤーが
                    var playerObj = Commons.Player2;
                    // 右の台札
                    var placeObj = Commons.RightCenterStack;

                    if (CardMoveHelper.IsBoomerang(gameModel, playerObj, placeObj, out IdOfPlayingCards previousCard))
                    {
                        // ブーメラン
                    }
                    else
                    {
                        var digitalCommand = new ModelOfThinkingEngineCommands.MoveCardToCenterStackFromHand(
                                playerObj: playerObj,
                                placeObj: placeObj);
                        schedulerModel.Timeline.AddWithinScheduler(playerObj, digitalCommand);
                    }
                }
            }

            // 間
            foreach (var playerObj in Commons.Players)
            {
                schedulerModel.Timeline.AddScheduleSeconds(playerObj: playerObj, time: intervalObj);
            }

            // 登録：手札から１枚引く
            {
                {
                    // １プレイヤーは手札から１枚抜いて、場札として置く
                    var playerObj = Commons.Player1;

                    var digitalCommand = new ModelOfThinkingEngineCommands.MoveCardsToHandFromPile(
                        playerObj: playerObj,
                        numberOfCards: 1);
                    schedulerModel.Timeline.AddWithinScheduler(playerObj, digitalCommand);
                }
                {
                    // ２プレイヤーは手札から１枚抜いて、場札として置く
                    var playerObj = Commons.Player2;

                    var digitalCommand = new ModelOfThinkingEngineCommands.MoveCardsToHandFromPile(
                        playerObj: playerObj,
                        numberOfCards: 1);
                    schedulerModel.Timeline.AddWithinScheduler(playerObj, digitalCommand);
                }
            }
        }
    }
}
