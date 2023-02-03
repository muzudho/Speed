namespace Assets.Scripts.Models.Timeline
{
    using Assets.Scripts.Views.Timeline;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// タイムライン・モデル
    /// 
    /// - 編集可能（Mutable）
    /// </summary>
    internal class Model
    {
        // - プロパティ

        /// <summary>
        /// スケジュールに登録されている残りの項目
        /// 
        /// - 実行されると、 `ongoingItems` へ移動する
        /// </summary>

        List<ISpanView> scheduledItemModels = new();

        internal List<ISpanView> ScheduledItems
        {
            get
            {
                return this.scheduledItemModels;
            }
        }

        // - メソッド

        /// <summary>
        /// 追加
        /// </summary>
        /// <param name="spanModel">タイム・スパン</param>
        internal void Add(ISpanView spanModel)
        {
            this.ScheduledItems.Add(spanModel);
        }

        internal ISpanView GetItemAt(int index)
        {
            return this.ScheduledItems[index];
        }

        internal int GetCountItems()
        {
            return this.ScheduledItems.Count;
        }

        internal void RemoveAt(int index)
        {
            this.ScheduledItems.RemoveAt(index);
        }

        internal void DebugWrite()
        {
            Debug.Log($"[Assets.Scripts.Models.Timeline.Model DebugWrite] timedItems.Count:{scheduledItemModels.Count}");
        }
    }
}
