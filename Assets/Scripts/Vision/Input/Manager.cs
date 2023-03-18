﻿namespace Assets.Scripts.Vision.Input
{
    using Assets.Scripts.ThinkingEngine;
    using Assets.Scripts.ThinkingEngine.Models;
    using ModelOfGame = Assets.Scripts.ThinkingEngine.Models.Game;
    using Assets.Scripts.ThinkingEngine.Models.CommandArgs;
    using Assets.Scripts.Vision.UserInterface;
    using Assets.Scripts.Vision.World;
    using Assets.Scripts.Vision.World.SpanOfLerp.TimedGenerator;
    using UnityEngine;
    using GuiOfTimedCommandArgs = Assets.Scripts.Vision.World.TimedCommandArgs;
    using VisionOfInput = Assets.Scripts.Vision.Input;
    using static UnityEditor.Experimental.GraphView.GraphView;

    /// <summary>
    /// 入力マネージャー
    /// </summary>
    public class Manager : MonoBehaviour
    {
        // - フィールド

        ScheduleRegister scheduleRegister;

        /// <summary>
        /// コンピューター・プレイヤー用
        /// </summary>
        ModelOfGame.Default gameModel;

        /// <summary>
        /// ステールメート後の再開用
        /// </summary>
        ReopeningManager reopeningManager;

        float[] spamSeconds = new[] { 0f, 0f };

        /// <summary>
        /// コンピューター・プレイヤーか？
        /// 
        /// - コンピューターなら Computer インスタンス
        /// - コンピューターでなければヌル
        /// </summary>
        internal Computer[] Computers { get; set; } = new Computer[] { null, null, };
        // internal Computer[] Computers { get; set; } = new Computer[] { new Computer(0), new Computer(1), };

        VisionOfInput.ToMeaning inputToMeaning = new VisionOfInput.ToMeaning();

        // - イベントハンドラ

        // Start is called before the first frame update
        void Start()
        {
            var gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
            scheduleRegister = gameManager.ScheduleRegister;
            gameModel = gameManager.Model;

            reopeningManager = GameObject.Find("Reopening Manager").GetComponent<ReopeningManager>();
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
            for (var player = 0; player < 2; player++)
            {
                // 前判定：もう入力できないなら真
                //
                // - スパム中
                // - 対局停止中
                handled[player] = 0 < spamSeconds[player] || !gameModel.IsGameActive;

                if (!handled[player])
                {
                    if (Computers[player] == null)
                    {
                        // キー入力の解析：人間の入力を受付
                        inputToMeaning.UpdateFromInput(player);
                    }
                    else
                    {
                        // コンピューター・プレイヤーが思考して、操作を決める
                        Computers[player].Think(gameModel);

                        // キー入力の解析：コンピューターからの入力を受付
                        inputToMeaning.Overwrite(
                            player: player,
                            moveCardToCenterStackNearMe: Computers[player].MoveCardToCenterStackNearMe,
                            moveCardToFarCenterStack: Computers[player].MoveCardToFarCenterStack,
                            pickupCardToForward: Computers[player].PickupCardToForward,
                            pickupCardToBackward: Computers[player].PickupCardToBackward,
                            drawing: Computers[player].Drawing);
                    }
                }

                // スパン時間消化
                if (0 < spamSeconds[player])
                {
                    // 負数になっても気にしない
                    spamSeconds[player] -= Time.deltaTime;
                }
            }

            const int right = 0;// 台札の右
            const int left = 1;// 台札の左

            // ステールメートしてるかどうかの判定
            // ==================================
            bool player1CanPutToRightCenterStack = LegalMove.CanPutToCenterStack(this.gameModel, Commons.Player1, right); // 1Pは右の台札にカードを置ける
            bool player1CanPutToLeftCenterStack = LegalMove.CanPutToCenterStack(this.gameModel, Commons.Player1, left);   // 1Pは左の台札にカードを置ける
            bool player2CanPutToRightCenterStack = LegalMove.CanPutToCenterStack(this.gameModel, Commons.Player2, right); // 2Pは右の台札にカードを置ける
            bool player2CanPutToLeftCenterStack = LegalMove.CanPutToCenterStack(this.gameModel, Commons.Player2, left);   // 2Pは左の台札にカードを置ける

            // ステールメートしているとき
            if (!player1CanPutToRightCenterStack &&
                !player1CanPutToLeftCenterStack &&
                !player2CanPutToRightCenterStack &&
                !player2CanPutToLeftCenterStack)
            {
                // TODO ★ カウントダウン・タイマーを表示。０になったら、ピックアップ中の場札を強制的に台札へ置く
                this.reopeningManager.DoIt();
            }

            // 先に登録したコマンドの方が早く実行される

            // （ボタン押下が同時なら）右の台札は１プレイヤー優先
            // ==================================================

            // - １プレイヤー
            // - 自分の近い方の台札へ置く
            {
                var playerObj = Commons.Player1;
                if (!handled[playerObj.AsInt] && inputToMeaning.MoveCardToCenterStackNearMe[playerObj.AsInt] && player1CanPutToRightCenterStack)  // 右の
                {
                    // １プレイヤーが、ピックアップ中の場札を抜いて、（１プレイヤーから見て）右の台札へ積み上げる
                    var timedCommandArg = new GuiOfTimedCommandArgs.Model(new MoveCardToCenterStackFromHandModel(
                        playerObj: playerObj,      // １プレイヤーが
                        place: right)); // 右の

                    spamSeconds[playerObj.AsInt] = timedCommandArg.Duration;
                    scheduleRegister.AddJustNow(timedCommandArg);
                    handled[playerObj.AsInt] = true;
                }
            }

            // - ２プレイヤー
            // - 自分から遠い方の台札へ置く
            {
                var playerObj = Commons.Player2;
                if (!handled[playerObj.AsInt] && inputToMeaning.MoveCardToFarCenterStack[playerObj.AsInt] && player2CanPutToRightCenterStack)  // 右の)
                {
                    // ２プレイヤーが、ピックアップ中の場札を抜いて、（１プレイヤーから見て）右の台札へ積み上げる
                    var timedCommandArg = new GuiOfTimedCommandArgs.Model(new MoveCardToCenterStackFromHandModel(
                        playerObj: playerObj,      // ２プレイヤーが
                        place: right)); // 右の

                    spamSeconds[playerObj.AsInt] = timedCommandArg.Duration;
                    scheduleRegister.AddJustNow(timedCommandArg);
                    handled[playerObj.AsInt] = true;
                }
            }

            // （ボタン押下が同時なら）左の台札は２プレイヤー優先
            // ==================================================

            // - ２プレイヤー
            // - 自分の近い方の台札へ置く
            {
                var playerObj = Commons.Player2;
                if (!handled[playerObj.AsInt] && inputToMeaning.MoveCardToCenterStackNearMe[playerObj.AsInt] && player2CanPutToLeftCenterStack)
                {
                    // ２プレイヤーが、ピックアップ中の場札を抜いて、（１プレイヤーから見て）左の台札へ積み上げる
                    var timedCommandArg = new GuiOfTimedCommandArgs.Model(new MoveCardToCenterStackFromHandModel(
                        playerObj: playerObj,      // ２プレイヤーが
                        place: left));  // 左の

                    spamSeconds[playerObj.AsInt] = timedCommandArg.Duration;
                    scheduleRegister.AddJustNow(timedCommandArg);
                    handled[playerObj.AsInt] = true;
                }
            }

            // - １プレイヤー
            // - 自分から遠い方の台札へ置く
            {
                var playerObj = Commons.Player1;
                if (!handled[playerObj.AsInt] && inputToMeaning.MoveCardToFarCenterStack[playerObj.AsInt] && player1CanPutToLeftCenterStack)
                {
                    // １プレイヤーが、ピックアップ中の場札を抜いて、（１プレイヤーから見て）左の台札へ積み上げる
                    var timedCommandArg = new GuiOfTimedCommandArgs.Model(new MoveCardToCenterStackFromHandModel(
                        playerObj: playerObj,      // １プレイヤーが
                        place: left));  // 左の

                    spamSeconds[playerObj.AsInt] = timedCommandArg.Duration;
                    scheduleRegister.AddJustNow(timedCommandArg);
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
                        var timedCommandArg = new GuiOfTimedCommandArgs.Model(new MoveFocusToNextCardModel(
                            playerObj: playerObj,
                            direction: 1));

                        spamSeconds[playerObj.AsInt] = timedCommandArg.Duration;
                        scheduleRegister.AddJustNow(timedCommandArg);
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
                        var timedCommandArg = new GuiOfTimedCommandArgs.Model(new MoveFocusToNextCardModel(
                            playerObj: playerObj,
                            direction: 0));

                        spamSeconds[playerObj.AsInt] = timedCommandArg.Duration;
                        scheduleRegister.AddJustNow(timedCommandArg);
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
                        var timedCommandArg = new GuiOfTimedCommandArgs.Model(new MoveFocusToNextCardModel(
                            playerObj: playerObj,
                            direction: 1));

                        spamSeconds[playerObj.AsInt] = timedCommandArg.Duration;
                        scheduleRegister.AddJustNow(timedCommandArg);
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
                        var timedCommandArg = new GuiOfTimedCommandArgs.Model(new MoveFocusToNextCardModel(
                            playerObj: playerObj,
                            direction: 0));

                        spamSeconds[playerObj.AsInt] = timedCommandArg.Duration;
                        scheduleRegister.AddJustNow(timedCommandArg);
                    }
                }
            }

            // デバッグ用
            if (inputToMeaning.Drawing)
            {
                // 両プレイヤーは手札から１枚抜いて、場札として置く
                foreach (var playerObj in Commons.Players)
                {
                    // 場札を並べる
                    var timedCommandArg = new GuiOfTimedCommandArgs.Model(new MoveCardsToHandFromPileModel(
                        playerObj: playerObj,
                    numberOfCards: 1));

                    spamSeconds[playerObj.AsInt] = timedCommandArg.Duration;
                    scheduleRegister.AddJustNow(timedCommandArg);
                }
            }
        }
    }
}