﻿namespace Assets.Scripts.ThinkingEngine.Models.GameBuffer
{
    using System.Collections.Generic;

    /// <summary>
    /// ゲーム・モデル・バッファー
    /// 
    /// - 台札別
    /// </summary>
    internal class CenterStack
    {
        // - その他

        /// <summary>
        /// 生成
        /// </summary>
        internal CenterStack(
            List<IdOfPlayingCards> idOfCards)
        {
            IdOfCards = idOfCards;
        }

        // - プロパティ

        /// <summary>
        /// 台札
        /// 
        /// - 画面中央に積んでいる札
        /// - 0: 右
        /// - 1: 左
        /// </summary>
        internal List<IdOfPlayingCards> IdOfCards { get; private set; }

        // - メソッド

        /// <summary>
        /// 台札の枚数
        /// </summary>
        /// <param name="idOfCard"></param>
        internal IdOfPlayingCards GetCard(int index)
        {
            return this.IdOfCards[index];
        }

        /// <summary>
        /// 台札の枚数
        /// </summary>
        /// <param name="idOfCard"></param>
        internal int GetLength()
        {
            return this.IdOfCards.Count;
        }

        /// <summary>
        /// 台札を追加
        /// </summary>
        /// <param name="idOfCard"></param>
        internal void AddCard(IdOfPlayingCards idOfCard)
        {
            this.IdOfCards.Add(idOfCard);
        }

        /// <summary>
        /// 台札を削除
        /// </summary>
        /// <param name="startIndexObj"></param>
        internal void RemoveCardAt(CenterStackCardIndex startIndexObj)
        {
            this.IdOfCards.RemoveAt(startIndexObj.AsInt);
        }
    }
}
