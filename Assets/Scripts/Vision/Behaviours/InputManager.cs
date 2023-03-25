namespace Assets.Scripts.Vision.Behaviours
{
    using Assets.Scripts.ThinkingEngine;
    using Assets.Scripts.ThinkingEngine.Models;
    using UnityEngine;
    using ModelOfGame = Assets.Scripts.ThinkingEngine.Models.Game;
    using ModelOfInput = Assets.Scripts.Vision.Models.Input;
    using ModelOfInputOfPlayer = Assets.Scripts.Vision.Models.Input.Players;
    using ModelOfScheduler = Assets.Scripts.Vision.Models.Scheduler;
    using ModelOfSchedulerO7thTimeline = Assets.Scripts.Vision.Models.Scheduler.O7thTimeline;
    using ModelOfThinkingEngineCommand = Assets.Scripts.ThinkingEngine.Models.Commands;

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
        /// タイムライン
        /// </summary>
        ModelOfSchedulerO7thTimeline.Model timeline;

        // - プロパティ

        /// <summary>
        /// プレイヤーの入力
        /// 
        /// - プレイヤー別
        /// </summary>
        internal readonly ModelOfInput.Player[] InputOfPlayers = new ModelOfInput.Player[]
        {
            new ModelOfInput.Player(
                playerIdObj: Commons.Player1,
                nearCenterStackPlace: Commons.RightCenterStack,     // 1Pは右の台札にカードを置ける
                farCenterStackPlace: Commons.LeftCenterStack,       // 1Pは左の台札にカードを置ける
                meaning: new ModelOfInputOfPlayer.Meaning(
                onMoveCardToCenterStackNearMe: ()=>Input.GetKeyDown(KeyCode.DownArrow),
                onMoveCardToFarCenterStack: ()=>Input.GetKeyDown(KeyCode.UpArrow),
                onPickupCardToForward: ()=>Input.GetKeyDown(KeyCode.RightArrow),
                onPickupCardToBackward: ()=>Input.GetKeyDown(KeyCode.LeftArrow),
                onDrawing: ()=>Input.GetKeyDown(KeyCode.Space))),       // １プレイヤーと、２プレイヤーの２回判定されてしまう

            new ModelOfInput.Player(
                playerIdObj: Commons.Player2,
                nearCenterStackPlace: Commons.LeftCenterStack,      // 2Pは左の台札にカードを置ける
                farCenterStackPlace: Commons.RightCenterStack,      // 2Pは右の台札にカードを置ける
                meaning: new ModelOfInputOfPlayer.Meaning(
                onMoveCardToCenterStackNearMe: ()=>Input.GetKeyDown(KeyCode.S),
                onMoveCardToFarCenterStack: ()=>Input.GetKeyDown(KeyCode.W),
                onPickupCardToForward: ()=>Input.GetKeyDown(KeyCode.D),
                onPickupCardToBackward: ()=>Input.GetKeyDown(KeyCode.A),
                onDrawing:()=>Input.GetKeyDown(KeyCode.Space))),        // １プレイヤーと、２プレイヤーの２回判定されてしまう
        };

        // - イベントハンドラ

        // Start is called before the first frame update
        void Start()
        {
            var gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
            timeline = gameManager.Timeline;
            gameModel = gameManager.Model;

            this.stalemateManager = GameObject.Find("Stalemate Manager").GetComponent<StalemateManager>();
            this.stalemateManager.Init(this.timeline);
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
                this.InputOfPlayers[playerObj.AsInt].Handled = false;

                // キー入力を翻訳する
                this.InputOfPlayers[playerObj.AsInt].Translate(gameModel);
            }

            // ステールメートしてるかどうかの判定
            // ==================================

            this.stalemateManager.CheckStalemate(this.gameModel);

            // 先に登録したコマンドの方が早く実行される

            // （ボタン押下が同時なら）右の台札は１プレイヤー優先
            // ==================================================

            // - １プレイヤー
            // - 自分の近い方の台札へ置く
            this.InputOfPlayers[Commons.Player1.AsInt].MoveCardToCenterStackFromHand(
                NearFar.Near,
                this.gameModel,
                this.stalemateManager,
                this.timeline);

            // - ２プレイヤー
            // - 自分から遠い方の台札へ置く
            this.InputOfPlayers[Commons.Player2.AsInt].MoveCardToCenterStackFromHand(
                NearFar.Far,
                this.gameModel,
                this.stalemateManager,
                this.timeline);

            // （ボタン押下が同時なら）左の台札は２プレイヤー優先
            // ==================================================

            // - ２プレイヤー
            // - 自分の近い方の台札へ置く
            this.InputOfPlayers[Commons.Player2.AsInt].MoveCardToCenterStackFromHand(
                NearFar.Near,
                this.gameModel,
                this.stalemateManager,
                this.timeline);

            // - １プレイヤー
            // - 自分から遠い方の台札へ置く
            this.InputOfPlayers[Commons.Player1.AsInt].MoveCardToCenterStackFromHand(
                NearFar.Far,
                this.gameModel,
                this.stalemateManager,
                this.timeline);

            // それ以外のキー入力は、同時でも勝敗に関係しない
            // ==============================================

            // １プレイヤー
            {
                var playerObj = Commons.Player1;

                if (this.InputOfPlayers[playerObj.AsInt].Handled)
                {

                }
                // 行動：
                //      １プレイヤーのピックアップしているカードから見て、（１プレイヤーから見て）
                //      左隣のカードをピックアップするように変えます
                else if (this.InputOfPlayers[playerObj.AsInt].Meaning.PickupCardToBackward)
                {
                    this.InputOfPlayers[playerObj.AsInt].PickupCardToBackward(
                        this.gameModel,
                        this.stalemateManager,
                        this.timeline);
                }
                // 行動：
                //      １プレイヤーのピックアップしているカードから見て、（１プレイヤーから見て）
                //      右隣のカードをピックアップするように変えます
                else if (this.InputOfPlayers[playerObj.AsInt].Meaning.PickupCardToForward)
                {
                    this.InputOfPlayers[playerObj.AsInt].PickupCardToForward(
                        this.gameModel,
                        this.stalemateManager,
                        this.timeline);
                }
            }

            // ２プレイヤー
            {
                var playerObj = Commons.Player2;

                if (this.InputOfPlayers[playerObj.AsInt].Handled)
                {

                }
                // 行動：
                //      ２プレイヤーのピックアップしているカードから見て、（２プレイヤーから見て）
                //      左隣のカードをピックアップするように変えます
                else if (this.InputOfPlayers[playerObj.AsInt].Meaning.PickupCardToBackward)
                {
                    this.InputOfPlayers[playerObj.AsInt].PickupCardToBackward(
                        this.gameModel,
                        this.stalemateManager,
                        this.timeline);
                }
                // 行動：
                //      ２プレイヤーのピックアップしているカードから見て、（２プレイヤーから見て）
                //      右隣のカードをピックアップするように変えます
                else if (this.InputOfPlayers[playerObj.AsInt].Meaning.PickupCardToForward)
                {
                    this.InputOfPlayers[playerObj.AsInt].PickupCardToForward(
                        this.gameModel,
                        this.stalemateManager,
                        this.timeline);
                }
            }

            // 場札の補充
            if (this.InputOfPlayers[Commons.Player1.AsInt].Meaning.Drawing ||
                this.InputOfPlayers[Commons.Player2.AsInt].Meaning.Drawing)
            {
                // 両プレイヤーは手札から１枚抜いて、場札として置く
                foreach (var playerObj in Commons.Players)
                {
                    this.InputOfPlayers[playerObj.AsInt].DrawingHandCard(
                        this.gameModel,
                        this.timeline);
                }
            }
        }
    }
}