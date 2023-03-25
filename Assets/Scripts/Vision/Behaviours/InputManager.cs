namespace Assets.Scripts.Vision.Behaviours
{
    using Assets.Scripts.ThinkingEngine;
    using Assets.Scripts.ThinkingEngine.Models;
    using UnityEngine;
    using ModelOfGame = Assets.Scripts.ThinkingEngine.Models.Game;
    using ModelOfInput = Assets.Scripts.Vision.Models.Input;
    using ModelOfScheduler = Assets.Scripts.Vision.Models.Scheduler;

    /// <summary>
    /// 入力マネージャー
    /// </summary>
    public class InputManager : MonoBehaviour
    {
        // - フィールド

        /// <summary>
        /// コンピューター・プレイヤー用
        /// </summary>
        ModelOfGame.Default gameModel;

        /// <summary>
        /// ステールメート管理
        /// </summary>
        StalemateManager stalemateManager;

        /// <summary>
        /// スケジューラー・モデル
        /// </summary>
        ModelOfScheduler.Model schedulerModel;

        // - プロパティ

        /// <summary>
        /// 入力モデル
        /// </summary>
        internal readonly ModelOfInput.Init Model = new ModelOfInput.Init();

        // - イベントハンドラ

        // Start is called before the first frame update
        void Start()
        {
            var gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
            gameModel = gameManager.Model;

            var schedulerManager = GameObject.Find("Scheduler Manager").GetComponent<SchedulerManager>();
            this.schedulerModel = schedulerManager.Model;

            this.stalemateManager = GameObject.Find("Stalemate Manager").GetComponent<StalemateManager>();
            this.stalemateManager.Init(this.schedulerModel);
        }

        /// <summary>
        /// Update is called once per frame
        /// 
        /// - 入力は、すぐに実行は、しません
        /// - 入力は、コマンドに変換して、タイムラインへ登録します
        /// </summary>
        void Update()
        {
            // 初期化
            foreach (var playerObj in Commons.Players)
            {
                // もう入力できないなら真
                this.Model.Players[playerObj.AsInt].Rights.ClearHandle();

                // キー入力を翻訳する
                this.Model.Players[playerObj.AsInt].Translate(gameModel);
            }

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
                    this.Model.Players[playerObj.AsInt].PickupCardToBackward(
                        this.gameModel,
                        this.stalemateManager,
                        this.schedulerModel);
                }
                // 行動：
                //      １プレイヤーのピックアップしているカードから見て、（１プレイヤーから見て）
                //      右隣のカードをピックアップするように変えます
                else if (this.Model.Players[playerObj.AsInt].Meaning.PickupCardToForward)
                {
                    this.Model.Players[playerObj.AsInt].PickupCardToForward(
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
                    this.Model.Players[playerObj.AsInt].PickupCardToBackward(
                        this.gameModel,
                        this.stalemateManager,
                        this.schedulerModel);
                }
                // 行動：
                //      ２プレイヤーのピックアップしているカードから見て、（２プレイヤーから見て）
                //      右隣のカードをピックアップするように変えます
                else if (this.Model.Players[playerObj.AsInt].Meaning.PickupCardToForward)
                {
                    this.Model.Players[playerObj.AsInt].PickupCardToForward(
                        this.gameModel,
                        this.stalemateManager,
                        this.schedulerModel);
                }
            }

            // 場札の補充
            if (this.Model.Players[Commons.Player1.AsInt].Meaning.Drawing ||
                this.Model.Players[Commons.Player2.AsInt].Meaning.Drawing)
            {
                // 両プレイヤーは手札から１枚抜いて、場札として置く
                foreach (var playerObj in Commons.Players)
                {
                    this.Model.Players[playerObj.AsInt].DrawingHandCard(
                        this.gameModel,
                        this.schedulerModel);
                }
            }
        }
    }
}