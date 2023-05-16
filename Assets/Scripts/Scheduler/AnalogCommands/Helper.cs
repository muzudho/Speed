namespace Assets.Scripts.Scheduler.AnalogCommands
{
    using Assets.Scripts.Coding;
    using ModelOfAnalogCommands = Assets.Scripts.Scheduler.AnalogCommands;
    using ModelOfAnalogCommands1stTimelineSpan = Assets.Scripts.Scheduler.AnalogCommands.O1stTimelineSpan;
    using ModelOfGameBuffer = Assets.Scripts.ThinkingEngine.Models.Game.Buffer;
    using ModelOfGameWriter = Assets.Scripts.ThinkingEngine.Models.Game.Writer;
    using ModelOfInput = Assets.Scripts.Vision.Models.Input;

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
        /// <param name="gameModelBuffer">ゲームの内部状態（編集可能）</param>
        /// <param name="setTimespan"></param>
        internal static void ConvertToSpans(
            ModelOfGameBuffer.Model gameModelBuffer,
            ModelOfGameWriter.Model gameModelWriter,
            ModelOfInput.Init inputModel,
            ModelOfAnalogCommands.Model schedulerModel,
            LazyArgs.SetValue<ModelOfAnalogCommands1stTimelineSpan.IModel> setTimespan)
        {
            // TODO ★ スレッド・セーフにしたい
            // キューに溜まっている分を全て消化
            int i = 0;
            while (i < schedulerModel.Timeline.GetCountCommands())
            {
                var commandOfScheduler = schedulerModel.Timeline.GetCommandAt(i);

                // まだ
                if (gameModelBuffer.ElapsedTimeObj.AsFloat < commandOfScheduler.TimeRangeObj.StartObj.AsFloat)
                {
                    i++;
                    continue;
                }

                // 起動
                // ----
                // Debug.Log($"[Assets.Scripts.Vision.World.Models.Timeline.Model OnEnter] タイム・スパン実行 span.StartSeconds:{timeSpan.StartSeconds} <= gameModelBuffer.ElapsedTimeObj:{gameModelBuffer.ElapsedTimeObj}");

                // スケジュールから除去
                schedulerModel.Timeline.RemoveAt(i);

                // タイムスパン作成・登録
                commandOfScheduler.GenerateSpan(
                    gameModelBuffer,
                    gameModelWriter,
                    inputModel,
                    schedulerModel,
                    setTimespan: setTimespan);
            }
        }
    }
}
