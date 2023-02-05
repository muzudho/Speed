﻿namespace Assets.Scripts.Simulators
{
    using Assets.Scripts.ThikningEngine.CommandArgs;
    using Assets.Scripts.Views.Timeline;
    using Assets.Scripts.Views.Timeline.Spans;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// モデルには時間、空間の概念がないので、
    /// モデルに時間、空間を一意に紐づける働きをします
    /// </summary>
    internal static class Specification
    {
        // - その他

        /// <summary>
        /// 静的初期化
        /// </summary>
        static Specification()
        {
            // 隣の場札をピックアップする秒
            float durationOfMoveFocusToNextCard = 0.15f;

            DurationOfModels.Add(typeof(MoveCardsToHandFromPileModel).GetHashCode(), 0.15f + durationOfMoveFocusToNextCard);
            DurationOfModels.Add(typeof(MoveCardsToPileFromCenterStacksModel).GetHashCode(), 0.3f);
            DurationOfModels.Add(typeof(MoveCardToCenterStackFromHandModel).GetHashCode(), 0.15f + durationOfMoveFocusToNextCard);
            DurationOfModels.Add(typeof(MoveFocusToNextCardModel).GetHashCode(), durationOfMoveFocusToNextCard);

            ViewsFromModel.Add(typeof(MoveCardsToHandFromPileModel).GetHashCode(), new MoveCardsToHandFromPileView());
            ViewsFromModel.Add(typeof(MoveCardsToPileFromCenterStacksModel).GetHashCode(), new MoveCardsToPileFromCenterStacksView());
            ViewsFromModel.Add(typeof(MoveCardToCenterStackFromHandModel).GetHashCode(), new MoveCardToCenterStackFromHandView());
            ViewsFromModel.Add(typeof(MoveFocusToNextCardModel).GetHashCode(), new MoveFocusToNextCardView());
        }

        // - プロパティ

        internal static Dictionary<int, float> DurationOfModels = new();

        internal static Dictionary<int, ISpanView> ViewsFromModel = new();

        // - メソッド

        internal static float GetDurationBy(Type type)
        {
            return DurationOfModels[type.GetHashCode()];
        }

        internal static ISpanView SpawnViewFromModel(Type type)
        {
            return ViewsFromModel[type.GetHashCode()].Spawn();
        }
    }
}
