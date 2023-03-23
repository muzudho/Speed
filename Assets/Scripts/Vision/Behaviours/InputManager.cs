namespace Assets.Scripts.Vision.Behaviours
{
    using Assets.Scripts.ThinkingEngine;
    using Assets.Scripts.ThinkingEngine.Models;
    using Assets.Scripts.Vision.Models;
    using UnityEngine;
    using ModelOfGame = Assets.Scripts.ThinkingEngine.Models.Game;
    using ModelOfInput = Assets.Scripts.Vision.Models.Input;
    using ModelOfScheduler = Assets.Scripts.Vision.Models.Scheduler;
    using ModelOfSchedulerO7thTimeline = Assets.Scripts.Vision.Models.Scheduler.O7thTimeline;
    using ModelOfThinkingEngineCommand = Assets.Scripts.ThinkingEngine.Models.Commands;

    /// <summary>
    /// 入力マネージャー
    /// </summary>
    public class InputManager : MonoBehaviour
    {
        // - フィールド

        ModelOfSchedulerO7thTimeline.Model timeline;

        /// <summary>
        /// コンピューター・プレイヤー用
        /// </summary>
        ModelOfGame.Default gameModel;

        /// <summary>
        /// ステールメート管理
        /// </summary>
        StalemateManager stalemateManager;

        /// <summary>
        /// タイムライン・スパンの実行開始時間（秒）
        /// </summary>
        GameSeconds[] spanTimesObj = new[] { GameSeconds.Zero, GameSeconds.Zero };

        /// <summary>
        /// コンピューター・プレイヤーか？
        /// 
        /// - コンピューターなら Computer インスタンス
        /// - コンピューターでなければヌル
        /// </summary>
        internal Computer[] Computers { get; set; } = new Computer[] { null, null, };
        // internal Computer[] Computers { get; set; } = new Computer[] { new Computer(0), new Computer(1), };

        ModelOfInput.ToMeaning inputToMeaning = new ModelOfInput.ToMeaning();

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
            // キー入力の解析：クリアー
            inputToMeaning.Clear();

            // もう入力できないなら真
            bool[] handled = { false, false };

            // キー入力を翻訳する
            foreach (var playerObj in Commons.Players)
            {
                // 前判定：もう入力できないなら真
                //
                // - スパム中
                // - 対局停止中
                handled[playerObj.AsInt] = 0.0f < spanTimesObj[playerObj.AsInt].AsFloat || !gameModel.IsGameActive;

                if (!handled[playerObj.AsInt])
                {
                    if (Computers[playerObj.AsInt] == null)
                    {
                        // キー入力の解析：人間の入力を受付
                        inputToMeaning.UpdateFromInput(playerObj);
                    }
                    else
                    {
                        // コンピューター・プレイヤーが思考して、操作を決める
                        Computers[playerObj.AsInt].Think(gameModel);

                        // キー入力の解析：コンピューターからの入力を受付
                        inputToMeaning.Overwrite(
                            playerObj: playerObj,
                            moveCardToCenterStackNearMe: Computers[playerObj.AsInt].MoveCardToCenterStackNearMe,
                            moveCardToFarCenterStack: Computers[playerObj.AsInt].MoveCardToFarCenterStack,
                            pickupCardToForward: Computers[playerObj.AsInt].PickupCardToForward,
                            pickupCardToBackward: Computers[playerObj.AsInt].PickupCardToBackward,
                            drawing: Computers[playerObj.AsInt].Drawing);
                    }
                }

                // スパン時間消化
                if (0.0f < spanTimesObj[playerObj.AsInt].AsFloat)
                {
                    // 負数になっても気にしない
                    spanTimesObj[playerObj.AsInt] = new GameSeconds(spanTimesObj[playerObj.AsInt].AsFloat - Time.deltaTime);
                }
            }

            // ステールメートしてるかどうかの判定
            // ==================================

            this.stalemateManager.CheckStalemate(this.gameModel);

            // 先に登録したコマンドの方が早く実行される

            // （ボタン押下が同時なら）右の台札は１プレイヤー優先
            // ==================================================

            // - １プレイヤー
            // - 自分の近い方の台札へ置く
            {
                var playerObj = Commons.Player1;
                if (!handled[playerObj.AsInt] &&
                    !this.stalemateManager.IsStalemate &&
                    inputToMeaning.MoveCardToCenterStackNearMe[playerObj.AsInt] &&
                    LegalMove.CanPutToCenterStack(this.gameModel, Commons.Player1, gameModel.GetIndexOfFocusedCardOfPlayer(Commons.Player1), Commons.RightCenterStack))  // 1Pは右の台札にカードを置ける
                {
                    // １プレイヤーが、ピックアップ中の場札を抜いて、（１プレイヤーから見て）右の台札へ積み上げる
                    var command = new ModelOfThinkingEngineCommand.MoveCardToCenterStackFromHand(
                        playerObj: playerObj,      // １プレイヤーが
                        placeObj: Commons.RightCenterStack); // 右の

                    spanTimesObj[playerObj.AsInt] = ModelOfScheduler.CommandDurationMapping.GetDurationBy(command.GetType());
                    timeline.AddCommand(
                        startObj: gameModel.ElapsedSeconds,
                        command: command);
                    handled[playerObj.AsInt] = true;
                }
            }

            // - ２プレイヤー
            // - 自分から遠い方の台札へ置く
            {
                var playerObj = Commons.Player2;
                if (!handled[playerObj.AsInt] &&
                    !this.stalemateManager.IsStalemate &&
                    inputToMeaning.MoveCardToFarCenterStack[playerObj.AsInt] &&
                    LegalMove.CanPutToCenterStack(this.gameModel, Commons.Player2, gameModel.GetIndexOfFocusedCardOfPlayer(Commons.Player2), Commons.RightCenterStack))  // 2Pは右の台札にカードを置ける
                {
                    // ２プレイヤーが、ピックアップ中の場札を抜いて、（１プレイヤーから見て）右の台札へ積み上げる
                    var command = new ModelOfThinkingEngineCommand.MoveCardToCenterStackFromHand(
                        playerObj: playerObj,      // ２プレイヤーが
                        placeObj: Commons.RightCenterStack); // 右の

                    spanTimesObj[playerObj.AsInt] = ModelOfScheduler.CommandDurationMapping.GetDurationBy(command.GetType());
                    timeline.AddCommand(
                        startObj: gameModel.ElapsedSeconds,
                        command: command);
                    handled[playerObj.AsInt] = true;
                }
            }

            // （ボタン押下が同時なら）左の台札は２プレイヤー優先
            // ==================================================

            // - ２プレイヤー
            // - 自分の近い方の台札へ置く
            {
                var playerObj = Commons.Player2;
                if (!handled[playerObj.AsInt] &&
                    !this.stalemateManager.IsStalemate &&
                    inputToMeaning.MoveCardToCenterStackNearMe[playerObj.AsInt] &&
                    LegalMove.CanPutToCenterStack(this.gameModel, Commons.Player2, gameModel.GetIndexOfFocusedCardOfPlayer(Commons.Player2), Commons.LeftCenterStack)) // 2Pは左の台札にカードを置ける
                {
                    // ２プレイヤーが、ピックアップ中の場札を抜いて、（１プレイヤーから見て）左の台札へ積み上げる
                    var command = new ModelOfThinkingEngineCommand.MoveCardToCenterStackFromHand(
                        playerObj: playerObj,      // ２プレイヤーが
                        placeObj: Commons.LeftCenterStack);  // 左の

                    spanTimesObj[playerObj.AsInt] = ModelOfScheduler.CommandDurationMapping.GetDurationBy(command.GetType());
                    timeline.AddCommand(
                        startObj: gameModel.ElapsedSeconds,
                        command: command);
                    handled[playerObj.AsInt] = true;
                }
            }

            // - １プレイヤー
            // - 自分から遠い方の台札へ置く
            {
                var playerObj = Commons.Player1;
                if (!handled[playerObj.AsInt] &&
                    !this.stalemateManager.IsStalemate &&
                    inputToMeaning.MoveCardToFarCenterStack[playerObj.AsInt] &&
                    LegalMove.CanPutToCenterStack(this.gameModel, Commons.Player1, gameModel.GetIndexOfFocusedCardOfPlayer(Commons.Player1), Commons.LeftCenterStack))    // 1Pは左の台札にカードを置ける
                {
                    // １プレイヤーが、ピックアップ中の場札を抜いて、（１プレイヤーから見て）左の台札へ積み上げる
                    var command = new ModelOfThinkingEngineCommand.MoveCardToCenterStackFromHand(
                        playerObj: playerObj,      // １プレイヤーが
                        placeObj: Commons.LeftCenterStack);  // 左の

                    spanTimesObj[playerObj.AsInt] = ModelOfScheduler.CommandDurationMapping.GetDurationBy(command.GetType());
                    timeline.AddCommand(
                        startObj: gameModel.ElapsedSeconds,
                        command: command);
                    handled[playerObj.AsInt] = true;
                }
            }

            // それ以外のキー入力は、同時でも勝敗に関係しない
            // ==============================================

            // １プレイヤー
            {
                var playerObj = Commons.Player1;

                if (handled[playerObj.AsInt])
                {

                }
                // 行動：
                //      １プレイヤーのピックアップしているカードから見て、（１プレイヤーから見て）
                //      左隣のカードをピックアップするように変えます
                else if (inputToMeaning.PickupCardToBackward[playerObj.AsInt])
                {
                    // 制約：
                    //      場札が２枚以上あるときに限る
                    if (2 <= this.gameModel.GetCardsOfPlayerHand(playerObj).Count)
                    {
                        var command = new ModelOfThinkingEngineCommand.MoveFocusToNextCard(
                            playerObj: playerObj,
                            directionObj: Commons.PickLeft);

                        spanTimesObj[playerObj.AsInt] = ModelOfScheduler.CommandDurationMapping.GetDurationBy(command.GetType());
                        timeline.AddCommand(
                            startObj: gameModel.ElapsedSeconds,
                            command: command);
                    }
                }
                // 行動：
                //      １プレイヤーのピックアップしているカードから見て、（１プレイヤーから見て）
                //      右隣のカードをピックアップするように変えます
                else if (inputToMeaning.PickupCardToForward[playerObj.AsInt])
                {
                    // 制約：
                    //      場札が２枚以上あるときに限る
                    if (2 <= this.gameModel.GetCardsOfPlayerHand(playerObj).Count)
                    {
                        var command = new ModelOfThinkingEngineCommand.MoveFocusToNextCard(
                            playerObj: playerObj,
                            directionObj: Commons.PickRight);

                        spanTimesObj[playerObj.AsInt] = ModelOfScheduler.CommandDurationMapping.GetDurationBy(command.GetType());
                        timeline.AddCommand(
                            startObj: gameModel.ElapsedSeconds,
                            command: command);
                    }
                }
            }

            // ２プレイヤー
            {
                var playerObj = Commons.Player2;

                if (handled[playerObj.AsInt])
                {

                }
                // 行動：
                //      ２プレイヤーのピックアップしているカードから見て、（２プレイヤーから見て）
                //      左隣のカードをピックアップするように変えます
                else if (inputToMeaning.PickupCardToBackward[playerObj.AsInt])
                {
                    // 制約：
                    //      場札が２枚以上あるときに限る
                    if (2 <= this.gameModel.GetCardsOfPlayerHand(playerObj).Count)
                    {
                        var command = new ModelOfThinkingEngineCommand.MoveFocusToNextCard(
                            playerObj: playerObj,
                            directionObj: Commons.PickLeft);

                        spanTimesObj[playerObj.AsInt] = ModelOfScheduler.CommandDurationMapping.GetDurationBy(command.GetType());
                        timeline.AddCommand(
                            startObj: gameModel.ElapsedSeconds,
                            command: command);
                    }
                }
                // 行動：
                //      ２プレイヤーのピックアップしているカードから見て、（２プレイヤーから見て）
                //      右隣のカードをピックアップするように変えます
                else if (inputToMeaning.PickupCardToForward[playerObj.AsInt])
                {
                    // 制約：
                    //      場札が２枚以上あるときに限る
                    if (2 <= this.gameModel.GetCardsOfPlayerHand(playerObj).Count)
                    {
                        var command = new ModelOfThinkingEngineCommand.MoveFocusToNextCard(
                            playerObj: playerObj,
                            directionObj: Commons.PickRight);

                        spanTimesObj[playerObj.AsInt] = ModelOfScheduler.CommandDurationMapping.GetDurationBy(command.GetType());
                        timeline.AddCommand(
                            startObj: gameModel.ElapsedSeconds,
                            command: command);
                    }
                }
            }

            // 場札の補充
            if (inputToMeaning.Drawing)
            {
                // 両プレイヤーは手札から１枚抜いて、場札として置く
                foreach (var playerObj in Commons.Players)
                {
                    // 場札を並べる
                    var command = new ModelOfThinkingEngineCommand.MoveCardsToHandFromPile(
                        playerObj: playerObj,
                        numberOfCards: 1);

                    spanTimesObj[playerObj.AsInt] = ModelOfScheduler.CommandDurationMapping.GetDurationBy(command.GetType());
                    timeline.AddCommand(
                        startObj: gameModel.ElapsedSeconds,
                        command: command);
                }
            }
        }
    }
}