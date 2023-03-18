namespace Assets.Scripts.Vision.World.Replays
{
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
            for (int player = 0; player < 2; player++)
            {
                scheduleRegister.AddScheduleSeconds(player: player, seconds: interval);
            }

            // ゲーム・デモ開始

            // 登録：カード選択
            {
                for (int i = 0; i < 2; i++)
                {
                    // １プレイヤーの右隣のカードへフォーカスを移します
                    {
                        var player = 0;
                        var spanModel = new MoveFocusToNextCardModel(
                                player: player,
                                direction: 0);
                        scheduleRegister.AddWithinScheduler(player, spanModel);
                    }

                    // ２プレイヤーの右隣のカードへフォーカスを移します
                    {
                        var player = 1;
                        var spanModel = new MoveFocusToNextCardModel(
                                player: player,
                                direction: 0);
                        scheduleRegister.AddWithinScheduler(player, spanModel);
                    }

                    // 間
                    for (int player = 0; player < 2; player++)
                    {
                        scheduleRegister.AddScheduleSeconds(player: player, seconds: interval);
                    }
                }
            }

            // 登録：台札を積み上げる
            {
                {
                    var player = 0;
                    var spanModel = new MoveCardToCenterStackFromHandModel(
                            player: player, // １プレイヤーが
                            place: 1); // 左の台札
                    scheduleRegister.AddWithinScheduler(player, spanModel);
                }
                {
                    var player = 1;
                    var spanModel = new MoveCardToCenterStackFromHandModel(
                            player: player, // ２プレイヤーが
                            place: 0); // 右の台札
                    scheduleRegister.AddWithinScheduler(player, spanModel);
                }
            }

            // 間
            for (int player = 0; player < 2; player++)
            {
                scheduleRegister.AddScheduleSeconds(player: player, seconds: interval);
            }

            // 登録：手札から１枚引く
            {
                {
                    // １プレイヤーは手札から１枚抜いて、場札として置く
                    var player = 0;
                    var spanModel = new MoveCardsToHandFromPileModel(
                            player: player,
                            numberOfCards: 1);
                    scheduleRegister.AddWithinScheduler(player, spanModel);
                }
                {
                    // ２プレイヤーは手札から１枚抜いて、場札として置く
                    var player = 1;
                    var spanModel = new MoveCardsToHandFromPileModel(
                            player: 1,
                            numberOfCards: 1);
                    scheduleRegister.AddWithinScheduler(player, spanModel);
                }
            }
        }
    }
}
