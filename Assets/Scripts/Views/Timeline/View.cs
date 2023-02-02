namespace Assets.Scripts.Views.Timeline
{
    using ModelsOfTimeline = Assets.Scripts.Models.Timeline;
    using Assets.Scripts.Models;
    using Assets.Scripts.Views;
    using System.Collections.Generic;
    using UnityEngine;
    using Assets.Scripts.Models.Timeline;
    using Unity.VisualScripting;

    /// <summary>
    /// タイムライン・ビュー
    /// </summary>
    internal class View
    {
        // - その他（生成）

        public View(ModelsOfTimeline.Model model)
        {
            this.Model = model;
        }

        // - プロパティ

        internal ModelsOfTimeline.Model Model { get; private set; }

        /// <summary>
        /// 補間を実行中の項目
        /// 
        /// - 持続時間が切れると、消えていく
        /// </summary>
        List<CardMovementView> ongoingCardMovementViews = new();

        // - メソッド

        /// <summary>
        /// モーションの補間
        /// </summary>
        /// <param name="elapsedSeconds">ゲーム内消費時間（秒）</param>
        internal void Lerp(float elapsedSeconds, List<CardMovementModel> launchedCardMovementModels)
        {
            foreach (var cardMovementModel in launchedCardMovementModels)
            {
                ongoingCardMovementViews.Add(new CardMovementView(cardMovementModel));
            }

            Debug.Log($"[Assets.Scripts.Views.Timeline.View Lerp] リープ ongoingCardMovementViews count:{ongoingCardMovementViews.Count}");

            //// TODO ★ スレッド・セーフにしたい
            //// キューに溜まっている分を全て消化
            //int i = 0;
            //while (i < this.ScheduledItems.Count)
            //{

            //}


            if (0 < ongoingCardMovementViews.Count)
            {
                // 削除も行うので、逆順で
                for (int i = ongoingCardMovementViews.Count - 1; 0 <= i; i--)
                {
                    var ongoingCardMovementView = ongoingCardMovementViews[i];

                    // 期限切れ
                    if (ongoingCardMovementView.Model.EndSeconds <= elapsedSeconds)
                    {
                        // TODO ★★ 動作が完了する前に、次の動作を行うと、カードがどんどん沈んでいく、といったことが起こる。連打スパム対策が必要
                        // 動作完了
                        ongoingCardMovementView.Lerp(1.0f);


                        // 削除
                        ongoingCardMovementViews.RemoveAt(i);
                        continue;
                    }

                    // TODO 持続中のコマンドの補間
                    float progress = (elapsedSeconds - ongoingCardMovementView.Model.StartSeconds) / ongoingCardMovementView.Model.Duration;

                    ongoingCardMovementView.Lerp(progress);
                }
            }
        }

        internal void DebugWrite()
        {
            Debug.Log($"[Assets.Scripts.Views.Timeline.View DebugWrite] ongoingItems.Count:{ongoingCardMovementViews.Count}");
        }

    }
}
