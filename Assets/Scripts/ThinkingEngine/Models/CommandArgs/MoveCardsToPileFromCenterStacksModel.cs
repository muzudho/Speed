﻿namespace Assets.Scripts.ThinkingEngine.Models.CommandArgs
{
    /// <summary>
    /// 右（または左）側の台札１枚を、手札へ移動する
    /// </summary>
    internal class MoveCardsToPileFromCenterStacksModel : ICommandArg
    {
        // - 生成

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="placeObj"></param>
        internal MoveCardsToPileFromCenterStacksModel(CenterStackPlace placeObj)
        {
            this.PlaceObj = placeObj;
        }

        // - プロパティ

        internal CenterStackPlace PlaceObj { get; private set; }
    }
}
