namespace Assets.Scripts.Vision.Replays
{
    using Assets.Scripts.ThinkingEngine;
    using Assets.Scripts.ThinkingEngine.Models.CommandArgs;
    using Assets.Scripts.Vision.World.SpanOfLerp.TimedGenerator;

    static class Demo
    {
        // - メソッド

        /// <summary>
        /// タイムライン作成
        /// 
        /// - デモ
        /// </summary>
        static void SetupDemo(ScheduleRegister scheduleRegister)
        {
            // 卓準備

            // 間
            float interval = 0.85f;

            // 間
            foreach (var playerObj in Commons.Players)
            {
                scheduleRegister.AddScheduleSeconds(playerObj: playerObj, seconds: interval);
            }

            // ゲーム・デモ開始

            // 登録：カード選択
            {
                for (int i = 0; i < 2; i++)
                {
                    // １プレイヤーの右隣のカードへフォーカスを移します
                    {
                        var playerObj = Commons.Player1;
                        var spanModel = new MoveFocusToNextCardModel(
                                playerObj: playerObj,
                                directionObj: Commons.PickRight);
                        scheduleRegister.AddWithinScheduler(playerObj, spanModel);
                    }

                    // ２プレイヤーの右隣のカードへフォーカスを移します
                    {
                        var playerObj = Commons.Player2;
                        var spanModel = new MoveFocusToNextCardModel(
                                playerObj: playerObj,
                                directionObj: Commons.PickRight);
                        scheduleRegister.AddWithinScheduler(playerObj, spanModel);
                    }

                    // 間
                    foreach (var playerObj in Commons.Players)
                    {
                        scheduleRegister.AddScheduleSeconds(playerObj: playerObj, seconds: interval);
                    }
                }
            }

            // 登録：台札を積み上げる
            {
                {
                    var playerObj = Commons.Player1;
                    var spanModel = new MoveCardToCenterStackFromHandModel(
                            playerObj: playerObj, // １プレイヤーが
                            placeObj: Commons.LeftCenterStack); // 左の台札
                    scheduleRegister.AddWithinScheduler(playerObj, spanModel);
                }
                {
                    var playerObj = Commons.Player2;
                    var spanModel = new MoveCardToCenterStackFromHandModel(
                            playerObj: playerObj, // ２プレイヤーが
                            placeObj: Commons.RightCenterStack); // 右の台札
                    scheduleRegister.AddWithinScheduler(playerObj, spanModel);
                }
            }

            // 間
            foreach (var playerObj in Commons.Players)
            {
                scheduleRegister.AddScheduleSeconds(playerObj: playerObj, seconds: interval);
            }

            // 登録：手札から１枚引く
            {
                {
                    // １プレイヤーは手札から１枚抜いて、場札として置く
                    var playerObj = Commons.Player1;
                    var spanModel = new MoveCardsToHandFromPileModel(
                            playerObj: playerObj,
                            numberOfCards: 1);
                    scheduleRegister.AddWithinScheduler(playerObj, spanModel);
                }
                {
                    // ２プレイヤーは手札から１枚抜いて、場札として置く
                    var playerObj = Commons.Player2;
                    var spanModel = new MoveCardsToHandFromPileModel(
                            playerObj: playerObj,
                            numberOfCards: 1);
                    scheduleRegister.AddWithinScheduler(playerObj, spanModel);
                }
            }
        }
    }
}
