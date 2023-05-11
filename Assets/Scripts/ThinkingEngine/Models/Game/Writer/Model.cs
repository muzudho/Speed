namespace Assets.Scripts.ThinkingEngine.Models.Game.Writer
{
    using Assets.Scripts.Vision.Models;
    using Assets.Scripts.Vision.Models.World;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ModelOfThinkingEngine = Assets.Scripts.ThinkingEngine.Models;
    using ModelOfGameBuffer = Assets.Scripts.ThinkingEngine.Models.Game.Buffer;

    /// <summary>
    /// ゲームの状態
    /// 
    /// - 編集可能
    /// </summary>
    public class Model
    {
        // - その他

        #region その他（生成）
        /// <summary>
        /// 生成
        /// </summary>
        internal Model(ModelOfGameBuffer.Model gameModelBuffer)
        {
            this.gameModelBuffer = gameModelBuffer;

            this.CenterStacks = new CenterStack[2]
            {
                new CenterStack(gameModelBuffer, Commons.RightCenterStack),
                new CenterStack(gameModelBuffer, Commons.LeftCenterStack),
            };

            this.Players = new Player[2]
            {
                new Player(gameModelBuffer, Commons.Player1),
                new Player(gameModelBuffer, Commons.Player2),
            };
        }
        #endregion

        // - フィールド

        ModelOfGameBuffer.Model gameModelBuffer;

        // - プロパティ

        #region プロパティ（対局中か？）
        /// <summary>
        /// 対局中か？
        /// </summary>
        public bool IsGameActive
        {
            set
            {
                this.gameModelBuffer.IsGameActive = value;
            }
        }
        #endregion

        #region プロパティ（ゲーム内経過時間）
        /// <summary>
        /// ゲーム内経過時間
        /// </summary>
        internal GameSeconds ElapsedTimeObj
        {
            set
            {
                this.gameModelBuffer.ElapsedTimeObj = value;
            }
        }
        #endregion

        #region プロパティ（台札別）
        /// <summary>
        /// ゲーム・モデル・バッファー
        /// 
        /// - 台札別
        /// </summary>
        CenterStack[] CenterStacks { get; set; }

        internal CenterStack GetCenterStack(CenterStackPlace place)
        {
            return this.CenterStacks[place.AsInt];
        }
        #endregion

        #region プロパティ（プレイヤー別）
        /// <summary>
        /// ゲーム・モデル・バッファー
        /// 
        /// - プレイヤー別
        /// </summary>
        Player[] Players { get; set; }

        internal Player GetPlayer(ModelOfThinkingEngine.Player player)
        {
            return this.Players[player.AsInt];
        }
        #endregion

        // - メソッド

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="cardsOfGame"></param>
        internal void CleanUp(out List<IdOfPlayingCards> cardsOfGame)
        {
            // ゲーム開始時、とりあえず、すべてのカードを集める
            cardsOfGame = new();
            foreach (var idOfGo in GameObjectStorage.CreatePlayingCards().Keys)
            {
                cardsOfGame.Add(ModelOfThinkingEngine.IdMapping.GetIdOfPlayingCard(idOfGo));
            }

            // 台札、場札、手札をクリアーする
            this.GetCenterStack(Commons.RightCenterStack).Clear();
            this.GetCenterStack(Commons.LeftCenterStack).Clear();
            this.GetPlayer(Commons.Player1).ClearCardsOfHand();
            this.GetPlayer(Commons.Player1).ClearCardsOfPile();
            this.GetPlayer(Commons.Player2).ClearCardsOfHand();
            this.GetPlayer(Commons.Player2).ClearCardsOfPile();

            // すべてのカードをシャッフル
            cardsOfGame = cardsOfGame.OrderBy(i => Guid.NewGuid()).ToList();
        }
    }
}
