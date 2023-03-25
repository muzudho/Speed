namespace Assets.Scripts.Vision.Models.Replays
{
    using Assets.Scripts.ThinkingEngine;
    using ModelOfScheduler = Assets.Scripts.Vision.Models.Scheduler;
    using ModelOfThinkingEngineCommand = Assets.Scripts.ThinkingEngine.Models.Commands;

    static class Demo
    {
        // - メソッド

        /// <summary>
        /// タイムライン作成
        /// 
        /// - デモ
        /// </summary>
        static void SetupDemo(ModelOfScheduler.Model schedulerModel)
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
                        var spanModel = new ModelOfThinkingEngineCommand.MoveFocusToNextCard(
                                playerObj: playerObj,
                                directionObj: Commons.PickRight);
                        schedulerModel.Timeline.AddWithinScheduler(playerObj, spanModel);
                    }

                    // ２プレイヤーの右隣のカードへフォーカスを移します
                    {
                        var playerObj = Commons.Player2;
                        var spanModel = new ModelOfThinkingEngineCommand.MoveFocusToNextCard(
                                playerObj: playerObj,
                                directionObj: Commons.PickRight);
                        schedulerModel.Timeline.AddWithinScheduler(playerObj, spanModel);
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
                    var playerObj = Commons.Player1;
                    var spanModel = new ModelOfThinkingEngineCommand.MoveCardToCenterStackFromHand(
                            playerObj: playerObj, // １プレイヤーが
                            placeObj: Commons.LeftCenterStack); // 左の台札
                    schedulerModel.Timeline.AddWithinScheduler(playerObj, spanModel);
                }
                {
                    var playerObj = Commons.Player2;
                    var spanModel = new ModelOfThinkingEngineCommand.MoveCardToCenterStackFromHand(
                            playerObj: playerObj, // ２プレイヤーが
                            placeObj: Commons.RightCenterStack); // 右の台札
                    schedulerModel.Timeline.AddWithinScheduler(playerObj, spanModel);
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
                    var parameter = new ModelOfThinkingEngineCommand.MoveCardsToHandFromPile(
                            playerObj: playerObj,
                            numberOfCards: 1);
                    schedulerModel.Timeline.AddWithinScheduler(playerObj, parameter);
                }
                {
                    // ２プレイヤーは手札から１枚抜いて、場札として置く
                    var playerObj = Commons.Player2;
                    var parameter = new ModelOfThinkingEngineCommand.MoveCardsToHandFromPile(
                            playerObj: playerObj,
                            numberOfCards: 1);
                    schedulerModel.Timeline.AddWithinScheduler(playerObj, parameter);
                }
            }
        }
    }
}
