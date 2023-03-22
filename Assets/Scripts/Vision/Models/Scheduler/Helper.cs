namespace Assets.Scripts.Vision.Models.Scheduler
{
    using Assets.Scripts.Coding;
    using Assets.Scripts.ThinkingEngine.Models;
    using ModelOfSchedulerO1stTimelineSpan = Assets.Scripts.Vision.Models.Scheduler.O1stTimelineSpan;
    using ModelOfSchedulerO7thTimeline = Assets.Scripts.Vision.Models.Scheduler.O7thTimeline;

    /// <summary>
    /// スケジューラーのヘルパー
    /// </summary>
    internal static class Helper
    {
        // - メソッド

        /// <summary>
        /// ゲーム画面の同期を始めます
        /// </summary>
        /// <param name="timeline"></param>
        /// <param name="elapsedSeconds">ゲーム内消費時間（秒）</param>
        /// <param name="gameModelBuffer">ゲームの内部状態（編集可能）</param>
        /// <param name="setSpanToLerp"></param>
        internal static void ConvertToSpans(
            ModelOfSchedulerO7thTimeline.Model timeline,
            float elapsedSeconds,
            GameModelBuffer gameModelBuffer,
            LazyArgs.SetValue<ModelOfSchedulerO1stTimelineSpan.IModel> setSpanToLerp)
        {
            // TODO ★ スレッド・セーフにしたい
            // キューに溜まっている分を全て消化
            int i = 0;
            while (i < timeline.GetCountTasks())
            {
                var gameOperationSpan = timeline.GetTaskAt(i);

                // まだ
                if (elapsedSeconds < gameOperationSpan.StartSeconds)
                {
                    i++;
                    continue;
                }

                // 起動
                // ----
                // Debug.Log($"[Assets.Scripts.Vision.World.Models.Timeline.Model OnEnter] タイム・スパン実行 span.StartSeconds:{timeSpan.StartSeconds} <= elapsedSeconds:{elapsedSeconds}");

                // スケジュールから除去
                timeline.RemoveAt(i);

                // ゲーム画面の同期を始めます
                gameOperationSpan.GameOperation.Build(
                    gameOperationSpan,
                    gameModelBuffer,
                    setSpanToLerp: setSpanToLerp);
            }
        }
    }
}
