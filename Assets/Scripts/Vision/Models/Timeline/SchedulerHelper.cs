namespace Assets.Scripts.Vision.Models.Timeline
{
    using Assets.Scripts.Coding;
    using Assets.Scripts.ThinkingEngine.Models;
    using ModelOfTimelineO1stElement = Assets.Scripts.Vision.Models.Timeline.O1stElements;
    using ModelOfTimelineO1stSpan = Assets.Scripts.Vision.Models.Timeline.O1stSpan;

    /// <summary>
    /// タイムラインのスケジューラーのストレージ
    /// </summary>
    internal static class SchedulerHelper
    {
        // - メソッド

        /// <summary>
        /// ゲーム画面の同期を始めます
        /// </summary>
        /// <param name="elapsedSeconds">ゲーム内消費時間（秒）</param>
        /// <param name="gameModelBuffer">ゲームの内部状態（編集可能）</param>
        /// <param name="gameViewModel">画面表示の状態（編集可能）</param>
        internal static void ConvertToSpansToLerp(
            ModelOfTimelineO1stElement.ScheduleRegister scheduleRegister,
            float elapsedSeconds,
            GameModelBuffer gameModelBuffer,
            LazyArgs.SetValue<ModelOfTimelineO1stSpan.IBasecaseSpan> setSpanToLerp)
        {
            // TODO ★ スレッド・セーフにしたい
            // キューに溜まっている分を全て消化
            int i = 0;
            while (i < scheduleRegister.GetCountItems())
            {
                var timeSpan = scheduleRegister.GetItemAt(i);

                // まだ
                if (elapsedSeconds < timeSpan.StartSeconds)
                {
                    i++;
                    continue;
                }

                // 起動
                // ----
                // Debug.Log($"[Assets.Scripts.Vision.World.Models.Timeline.Model OnEnter] タイム・スパン実行 span.StartSeconds:{timeSpan.StartSeconds} <= elapsedSeconds:{elapsedSeconds}");

                // スケジュールから除去
                scheduleRegister.RemoveAt(i);

                // ゲーム画面の同期を始めます
                timeSpan.SpanGenerator.CreateSpanToLerp(
                    timeSpan,
                    gameModelBuffer,
                    setSpanToLerp: setSpanToLerp);
            }
        }
    }
}
