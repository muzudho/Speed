﻿namespace Assets.Scripts.Vision.Models.Input
{
    /// <summary>
    /// 入力の権利
    /// </summary>
    internal class Rights
    {
        // - プロパティ

        /// <summary>
        /// 非ゲーム中
        /// </summary>
        internal bool IsGameInactive { get; set; }

        /// <summary>
        /// 場札を、台札に投げている途中
        /// </summary>
        internal bool IsThrowingCardIntoCenterStack { get; set; }

        /// <summary>
        /// コマンド実行の残り時間（秒）
        /// </summary>
        internal GameSeconds TimeOfRestObj { get; set; } = GameSeconds.Zero;

        // - メソッド

        internal void ClearHandle()
        {
            this.IsGameInactive = false;
            this.IsThrowingCardIntoCenterStack = false;
        }

        /// <summary>
        /// もう入力できないなら真
        /// </summary>
        /// <returns></returns>
        internal bool IsHandled()
        {
            return                                      //
                this.IsGameInactive ||                  // 非ゲーム中
                this.IsThrowingCardIntoCenterStack ||   // カードを台札へ投げている途中
                (0.0f < this.TimeOfRestObj.AsFloat);    // モーション中
        }
    }
}
