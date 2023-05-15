namespace Assets.Scripts.Vision.Behaviours
{
    using Assets.Scripts.ThinkingEngine;
    using Assets.Scripts.ThinkingEngine.Models;
    using UnityEngine;
    using ModelOfObservableGame = Assets.Scripts.ThinkingEngine.Models.Game.Observable;
    using ModelOfGameBuffer = Assets.Scripts.ThinkingEngine.Models.Game.Buffer;
    using ModelOfInput = Assets.Scripts.Vision.Models.Input;
    using ModelOfScheduler = Assets.Scripts.Vision.Models.Scheduler;
    using ScriptOfThinkingEngine = Assets.Scripts.ThinkingEngine;
    using ManagerOfUserInterface = Assets.Scripts.Vision.Behaviours.CanvasManager;
    using System;

    /// <summary>
    /// 入力マネージャー
    /// </summary>
    public class InputManager : MonoBehaviour
    {
        // - フィールド

        /// <summary>
        /// ゲーム・モデル
        /// </summary>
        ModelOfObservableGame.Model gameModel;

        /// <summary>
        /// ゲーム・モデル・バッファー
        /// </summary>
        ModelOfGameBuffer.Model gameModelBuffer;

        /// <summary>
        /// ステールメート管理
        /// </summary>
        StalemateManager stalemateManager;

        /// <summary>
        /// スケジューラー・モデル
        /// </summary>
        ModelOfScheduler.Model schedulerModel;

        /// <summary>
        /// ユーザー・インターフェース・マネージャー
        /// </summary>
        ManagerOfUserInterface canvasManager;

        // - プロパティ

        /// <summary>
        /// 入力モデル
        /// </summary>
        internal readonly ModelOfInput.Init Model = new ModelOfInput.Init();

        // - メソッド

        #region メソッド（初期化）
        /// <summary>
        /// 初期化
        /// </summary>
        internal void CleanUp()
        {
            this.Model.CleanUp();
        }
        #endregion

        // - イベントハンドラ

        // Start is called before the first frame update
        void Start()
        {
            this.Model.CleanUp();

            var gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
            gameModel = gameManager.ObservableModel;
            gameModelBuffer = gameManager.ModelBuffer;

            var schedulerManager = GameObject.Find("Scheduler Manager").GetComponent<SchedulerManager>();
            this.schedulerModel = schedulerManager.Model;

            this.stalemateManager = GameObject.Find("Stalemate Manager").GetComponent<StalemateManager>();
            this.stalemateManager.Init(this.schedulerModel);

            this.canvasManager = GameObject.Find("Canvas Manager").GetComponent<CanvasManager>();
        }

        /// <summary>
        /// Update is called once per frame
        /// 
        /// - 入力は、すぐに実行は、しません
        /// - 入力は、コマンドに変換して、タイムラインへ登録します
        /// </summary>
        void Update()
        {
            // ゲームが始まっていなければ無視
            if (!gameModelBuffer.IsGameActive)
            {
                return;
            }

            // 初期化
            foreach (var playerObj in Commons.Players)
            {
                // もう入力できないなら真
                this.Model.Players[playerObj.AsInt].Rights.ClearHandleBeforeTick();

                // キー入力を翻訳する
                this.Model.Players[playerObj.AsInt].Translate(gameModel);
            }

            // 場札を使い切ったプレイヤーがいればゲーム終了
            // ============================================
            InputManager.CheckGameResult(
                thisModel: this.Model,
                gameModel: this.gameModel,
                onGameFinished: () =>
                {
                    gameModelBuffer.IsGameActive = false;
                },
                onDraw: ()=>
                {
                    this.canvasManager.Draw();
                },
                onWon1P: ()=>
                {
                    this.canvasManager.Won1P();
                },
                onWon2P:()=>
                {
                    this.canvasManager.Won2P();
                });

            // ステールメートしてるかどうかの判定
            // ==================================

            this.stalemateManager.CheckStalemate(this.gameModel);

            // 先に登録したコマンドの方が早く実行される

            // （ボタン押下が同時なら）右の台札は１プレイヤー優先
            // ==================================================

            // - １プレイヤー
            // - 自分の近い方の台札へ置く
            this.Model.Players[Commons.Player1.AsInt].MoveCardToCenterStackFromHand(
                NearFar.Near,
                this.gameModel,
                this.stalemateManager,
                this.schedulerModel);

            // - ２プレイヤー
            // - 自分から遠い方の台札へ置く
            this.Model.Players[Commons.Player2.AsInt].MoveCardToCenterStackFromHand(
                NearFar.Far,
                this.gameModel,
                this.stalemateManager,
                this.schedulerModel);

            // （ボタン押下が同時なら）左の台札は２プレイヤー優先
            // ==================================================

            // - ２プレイヤー
            // - 自分の近い方の台札へ置く
            this.Model.Players[Commons.Player2.AsInt].MoveCardToCenterStackFromHand(
                NearFar.Near,
                this.gameModel,
                this.stalemateManager,
                this.schedulerModel);

            // - １プレイヤー
            // - 自分から遠い方の台札へ置く
            this.Model.Players[Commons.Player1.AsInt].MoveCardToCenterStackFromHand(
                NearFar.Far,
                this.gameModel,
                this.stalemateManager,
                this.schedulerModel);

            // それ以外のキー入力は、同時でも勝敗に関係しない
            // ==============================================

            // １プレイヤー
            {
                var playerObj = Commons.Player1;

                if (this.Model.Players[playerObj.AsInt].Rights.IsHandled())
                {
                    // 今は入力できません
                }
                // 行動：
                //      １プレイヤーのピックアップしているカードから見て、（１プレイヤーから見て）
                //      左隣のカードをピックアップするように変えます
                else if (this.Model.Players[playerObj.AsInt].Meaning.PickupCardToBackward)
                {
                    this.Model.Players[playerObj.AsInt].PickupCardToNext(
                        ScriptOfThinkingEngine.Commons.PickLeft,
                        this.gameModel,
                        this.stalemateManager,
                        this.schedulerModel);
                }
                // 行動：
                //      １プレイヤーのピックアップしているカードから見て、（１プレイヤーから見て）
                //      右隣のカードをピックアップするように変えます
                else if (this.Model.Players[playerObj.AsInt].Meaning.PickupCardToForward)
                {
                    this.Model.Players[playerObj.AsInt].PickupCardToNext(
                        ScriptOfThinkingEngine.Commons.PickRight,
                        this.gameModel,
                        this.stalemateManager,
                        this.schedulerModel);
                }
            }

            // ２プレイヤー
            {
                var playerObj = Commons.Player2;

                if (this.Model.Players[playerObj.AsInt].Rights.IsHandled())
                {
                    // 今は入力できません
                }
                // 行動：
                //      ２プレイヤーのピックアップしているカードから見て、（２プレイヤーから見て）
                //      左隣のカードをピックアップするように変えます
                else if (this.Model.Players[playerObj.AsInt].Meaning.PickupCardToBackward)
                {
                    this.Model.Players[playerObj.AsInt].PickupCardToNext(
                        ScriptOfThinkingEngine.Commons.PickLeft,
                        this.gameModel,
                        this.stalemateManager,
                        this.schedulerModel);
                }
                // 行動：
                //      ２プレイヤーのピックアップしているカードから見て、（２プレイヤーから見て）
                //      右隣のカードをピックアップするように変えます
                else if (this.Model.Players[playerObj.AsInt].Meaning.PickupCardToForward)
                {
                    this.Model.Players[playerObj.AsInt].PickupCardToNext(
                        ScriptOfThinkingEngine.Commons.PickRight,
                        this.gameModel,
                        this.stalemateManager,
                        this.schedulerModel);
                }
            }

            // 場札の補充（手札から１枚抜いて、場札として置く）
            {
                {
                    var playerObj = Commons.Player1;

                    if (this.Model.Players[playerObj.AsInt].Meaning.Drawing)
                    {
                        this.Model.Players[playerObj.AsInt].DrawingHandCardFromPileCard(
                            this.gameModel,
                            this.schedulerModel);
                    }
                }

                {
                    var playerObj = Commons.Player2;

                    if (this.Model.Players[playerObj.AsInt].Meaning.Drawing)
                    {
                        this.Model.Players[playerObj.AsInt].DrawingHandCardFromPileCard(
                            this.gameModel,
                            this.schedulerModel);
                    }
                }
            }
        }

        /// <summary>
        /// ゲームの結果調べ
        /// </summary>
        static void CheckGameResult(
            ModelOfInput.Init thisModel,
            ModelOfObservableGame.Model gameModel,
            Action onGameFinished,
            Action onDraw,
            Action onWon1P,
            Action onWon2P)
        {
            // 場札を使い切っているか？
            bool[] resultArray = new bool[2];

            foreach (var playerObj in Commons.Players)
            {
                // - 場札を使い切っている
                // - カードを投げている最中ではない
                resultArray[playerObj.AsInt] = gameModel.GetPlayer(playerObj).GetLengthOfHandCards() < 1 && !thisModel.GetPlayer(playerObj).Rights.IsThrowingCardIntoCenterStack;
            }

            // いずれかが場札を使い切っていれば、ゲーム終了
            if (resultArray[Commons.Player1.AsInt] || resultArray[Commons.Player2.AsInt])
            {
                // ゲーム終了
                onGameFinished();

                // 両方が場札を使い切っていれば、引き分け
                if (resultArray[Commons.Player1.AsInt] && resultArray[Commons.Player2.AsInt])
                {
                    onDraw();
                }
                // １プレイヤーが場札を使い切っていれば、１プレイヤーの勝ち
                else if (resultArray[Commons.Player1.AsInt])
                {
                    onWon1P();
                }
                // それ以外は２プレイヤーの勝ち
                else
                {
                    onWon2P();
                }
            }
        }
    }
}