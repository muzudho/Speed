﻿namespace Assets.Scripts.Gui.Models.Timeline.Spans
{
    /// <summary>
    /// ｎプレイヤーは、右（または左）隣のカードへ、ピックアップを移動します
    /// </summary>
    internal class MoveFocusToNextCardModel : ISpanModel
    {
        // - 生成

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="player"></param>
        /// <param name="direction"></param>
        internal MoveFocusToNextCardModel(int player, int direction)
        {
            this.Player = player;
            this.Direction = direction;
        }

        // - プロパティ

        internal int Player { get; private set; }

        internal int Direction { get; private set; }
    }
}
