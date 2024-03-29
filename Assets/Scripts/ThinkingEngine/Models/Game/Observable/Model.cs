﻿namespace Assets.Scripts.ThinkingEngine.Models.Game.Observable
{
    using Assets.Scripts.Vision.Models;
    using System.Collections.Generic;
    using ModelOfThinkingEngine = Assets.Scripts.ThinkingEngine.Models;
    using ModelOfGameBuffer = Assets.Scripts.ThinkingEngine.Models.Game.Buffer;

    /// <summary>
    /// ゲーム・モデル
    /// 
    /// - 読み取り専用。(Immutable)
    /// </summary>
    class Model
    {
        // - その他

        public Model(ModelOfGameBuffer.Model gameModelBuffer)
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

        // - フィールド

        ModelOfGameBuffer.Model gameModelBuffer;

        // - プロパティ

        internal GameSeconds ElapsedSeconds => gameModelBuffer.ElapsedTimeObj;

        /// <summary>
        /// 対局中か？
        /// </summary>
        /// <returns></returns>
        internal bool IsGameActive => this.gameModelBuffer.IsGameActive;

        #region プロパティ（台札別）
        internal CenterStack[] CenterStacks { get; private set; }

        /// <summary>
        /// 台札取得
        /// </summary>
        /// <param name="playerObj"></param>
        /// <returns></returns>
        public CenterStack GetCenterStack(ModelOfThinkingEngine.CenterStackPlace placeObj)
        {
            return this.CenterStacks[placeObj.AsInt];
        }
        #endregion

        #region プロパティ（プレイヤー別）
        internal Player[] Players { get; private set; }

        /// <summary>
        /// プレイヤー取得
        /// </summary>
        /// <param name="playerObj"></param>
        /// <returns></returns>
        public Player GetPlayer(ModelOfThinkingEngine.Player playerObj)
        {
            return this.Players[playerObj.AsInt];
        }
        #endregion

        // - メソッド

        /// <summary>
        /// 手札の枚数を取得
        /// </summary>
        /// <returns></returns>
        internal int GetLengthOfPile(ModelOfThinkingEngine.Player playerObj)
        {
            return this.gameModelBuffer.GetPlayer(playerObj).IdOfCardsOfPile.Count;
        }

        /// <summary>
        /// 場札の枚数を取得
        /// </summary>
        /// <returns></returns>
        internal int GetLengthOfHand(ModelOfThinkingEngine.Player playerObj)
        {
            return this.gameModelBuffer.GetPlayer(playerObj).IdOfCardsOfHand.Count;
        }

        /// <summary>
        /// 台札の枚数を取得
        /// </summary>
        /// <returns></returns>
        internal int GetLengthOfCenterStack(ModelOfThinkingEngine.CenterStackPlace placeObj)
        {
            return this.gameModelBuffer.GetCenterStack(placeObj).IdOfCards.Count;
        }

        /// <summary>
        /// 選択されている場札
        /// </summary>
        /// <param name="playerObj"></param>
        /// <returns></returns>
        internal FocusedHandCard GetFocusedHandCardObj(ModelOfThinkingEngine.Player playerObj)
        {
            return this.gameModelBuffer.GetPlayer(playerObj).FocusedHandCardObj;
        }

        /// <summary>
        /// 場札を取得
        /// </summary>
        /// <param name="placeObj"></param>
        /// <param name="indexObj"></param>
        /// <returns></returns>
        internal IdOfPlayingCards GetCardOfHand(ModelOfThinkingEngine.Player playerObj, ModelOfThinkingEngine.HandCardIndex indexObj)
        {
            return this.gameModelBuffer.GetPlayer(playerObj).GetCardAtOfHand(indexObj);
        }

        /// <summary>
        /// 台札を取得
        /// </summary>
        /// <param name="placeObj"></param>
        /// <param name="indexObj"></param>
        /// <returns></returns>
        internal IdOfPlayingCards GetCardOfCenterStack(ModelOfThinkingEngine.CenterStackPlace placeObj, ModelOfThinkingEngine.CenterStackCardIndex indexObj)
        {
            return this.gameModelBuffer.GetCenterStack(placeObj).GetCard(indexObj.AsInt);
        }

        /// <summary>
        /// TODO 本当は JSON か何かで出力して、 GameModelBuffer が JSON を読込むような感じにしたい
        /// </summary>
        /// <returns></returns>
        internal ModelOfGameBuffer.Model CreateGameModelBuffer()
        {
            // ディープ・コピー
            return new ModelOfGameBuffer.Model(
                // 台札別
                centerStacks: new ModelOfGameBuffer.CenterStack[2]
                {
                    // 右
                    new(
                        // 台札
                        idOfCards: new List<IdOfPlayingCards>(this.gameModelBuffer.GetCenterStack(Commons.RightCenterStack).IdOfCards.ToArray())
                        ),

                    // 左
                    new (
                        // 台札
                        idOfCards: new List<IdOfPlayingCards>(this.gameModelBuffer.GetCenterStack(Commons.LeftCenterStack).IdOfCards.ToArray())
                        ),
                },
                // プレイヤー別
                players: new ModelOfGameBuffer.Player[2]
                {
                    // １プレイヤー
                    new(
                        // 手札
                        idOfCardsOfPile: new List<IdOfPlayingCards>(this.gameModelBuffer.GetPlayer(Commons.Player1).IdOfCardsOfPile.ToArray()),
                        // 場札
                        idOfCardsOfHand: new List<IdOfPlayingCards>(this.gameModelBuffer.GetPlayer(Commons.Player1).IdOfCardsOfHand.ToArray()),
                        // ピックアップ場札
                        focusedHandCardObj: this.gameModelBuffer.GetPlayer(Commons.Player1).FocusedHandCardObj
                        ),

                    // ２プレイヤー
                    new(
                        // 手札
                        idOfCardsOfPile: new List<IdOfPlayingCards>(this.gameModelBuffer.GetPlayer(Commons.Player2).IdOfCardsOfPile.ToArray()),
                        // 場札
                        idOfCardsOfHand: new List<IdOfPlayingCards>(this.gameModelBuffer.GetPlayer(Commons.Player2).IdOfCardsOfHand.ToArray()),
                        // ピックアップ場札
                        focusedHandCardObj: this.gameModelBuffer.GetPlayer(Commons.Player2).FocusedHandCardObj
                        ),
                }
                )
            {
                // ゲーム内経過時間
                ElapsedTimeObj = this.gameModelBuffer.ElapsedTimeObj,
            };
        }
    }
}
