using Assets.Scripts.Gui.SpanOfLerp.TimedGenerator;
using Assets.Scripts.ThinkingEngine;
using Assets.Scripts.ThinkingEngine.CommandArgs;
using UnityEngine;
using GuiOfTimedCommandArgs = Assets.Scripts.Gui.TimedCommandArgs;

public class InputManager : MonoBehaviour
{
    // - フィールド

    ScheduleRegister scheduleRegister;

    float[] spamSeconds = new[] { 0f, 0f };

    /// <summary>
    /// コンピューター・プレイヤーか？
    /// </summary>
    bool[] ComputerPlayer { get; set; } = new bool[] { false, false, };

    // - イベントハンドラ

    // Start is called before the first frame update
    void Start()
    {
        scheduleRegister = GameObject.Find("Game Manager").GetComponent<GameManager>().ScheduleRegister;
    }

    /// <summary>
    /// Update is called once per frame
    /// 
    /// - 入力は、すぐに実行は、しません
    /// - 入力は、コマンドに変換して、タイムラインへ登録します
    /// </summary>
    void Update()
    {
        // キー入力の解析
        bool[] moveCardToCenterStackNearMe = new[] { false, false };
        bool[] moveCardToFarCenterStack = new[] { false, false };
        bool[] pickupCardToForward = new[] { false, false };
        bool[] pickupCardToBackward = new[] { false, false };


        // もう入力できないなら真
        bool[] handled = { false, false };

        for (var player = 0; player < 2; player++)
        {
            // 前判定
            // もう入力できないなら真
            handled[player] = 0 < spamSeconds[player];

            if (!handled[player] && !ComputerPlayer[player])
            {
                // 人間の入力を受付
                if (player == 0)
                {
                    moveCardToCenterStackNearMe[player] = Input.GetKeyDown(KeyCode.DownArrow);
                    moveCardToFarCenterStack[player] = Input.GetKeyDown(KeyCode.UpArrow);
                    pickupCardToForward[player] = Input.GetKeyDown(KeyCode.RightArrow);
                    pickupCardToBackward[player] = Input.GetKeyDown(KeyCode.LeftArrow);
                }
                else
                {
                    moveCardToCenterStackNearMe[player] = Input.GetKeyDown(KeyCode.S);
                    moveCardToFarCenterStack[player] = Input.GetKeyDown(KeyCode.W);
                    pickupCardToForward[player] = Input.GetKeyDown(KeyCode.D);
                    pickupCardToBackward[player] = Input.GetKeyDown(KeyCode.A);
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

        // 先に登録したコマンドの方が早く実行される

        // （ボタン押下が同時なら）右の台札は１プレイヤー優先
        // ==================================================

        // １プレイヤー
        {
            var player = 0;
            if (!handled[player] && moveCardToCenterStackNearMe[player] && LegalMove.CanPutToCenterStack(
                gameModel: scheduleRegister.GameModel,
                player: player,
                place: right))  // 右の
            {
                // １プレイヤーが、ピックアップ中の場札を抜いて、（１プレイヤーから見て）右の台札へ積み上げる
                var timedCommandArg = new GuiOfTimedCommandArgs.Model(new MoveCardToCenterStackFromHandModel(
                    player: player,      // １プレイヤーが
                    place: right)); // 右の

                spamSeconds[player] = timedCommandArg.Duration;
                scheduleRegister.AddJustNow(timedCommandArg);
                handled[player] = true;
            }
        }

        // ２プレイヤー
        {
            var player = 1;
            if (!handled[player] && moveCardToFarCenterStack[player] && LegalMove.CanPutToCenterStack(
                gameModel: scheduleRegister.GameModel,
                player: player,
                place: right))  // 右の)
            {
                // ２プレイヤーが、ピックアップ中の場札を抜いて、（１プレイヤーから見て）右の台札へ積み上げる
                var timedCommandArg = new GuiOfTimedCommandArgs.Model(new MoveCardToCenterStackFromHandModel(
                    player: player,      // ２プレイヤーが
                    place: right)); // 右の

                spamSeconds[player] = timedCommandArg.Duration;
                scheduleRegister.AddJustNow(timedCommandArg);
                handled[player] = true;
            }
        }

        // （ボタン押下が同時なら）左の台札は２プレイヤー優先
        // ==================================================

        // ２プレイヤー
        {
            var player = 1;
            if (!handled[player] && moveCardToCenterStackNearMe[player] && LegalMove.CanPutToCenterStack(
                gameModel: scheduleRegister.GameModel,
                player: player,
                place: left))
            {
                // ２プレイヤーが、ピックアップ中の場札を抜いて、（１プレイヤーから見て）左の台札へ積み上げる
                var timedCommandArg = new GuiOfTimedCommandArgs.Model(new MoveCardToCenterStackFromHandModel(
                    player: player,      // ２プレイヤーが
                    place: left));  // 左の

                spamSeconds[player] = timedCommandArg.Duration;
                scheduleRegister.AddJustNow(timedCommandArg);
                handled[player] = true;
            }
        }

        // １プレイヤー
        {
            var player = 0;
            if (!handled[player] && moveCardToFarCenterStack[player] && LegalMove.CanPutToCenterStack(
                gameModel: scheduleRegister.GameModel,
                player: player,
                place: left))
            {
                // １プレイヤーが、ピックアップ中の場札を抜いて、（１プレイヤーから見て）左の台札へ積み上げる
                var timedCommandArg = new GuiOfTimedCommandArgs.Model(new MoveCardToCenterStackFromHandModel(
                    player: player,      // １プレイヤーが
                    place: left));  // 左の

                spamSeconds[player] = timedCommandArg.Duration;
                scheduleRegister.AddJustNow(timedCommandArg);
                handled[player] = true;
            }
        }

        // それ以外のキー入力は、同時でも勝敗に関係しない
        // ==============================================

        // １プレイヤー
        {
            var player = 0;

            if (handled[player])
            {

            }
            else if (pickupCardToBackward[player])
            {
                // １プレイヤーのピックアップしているカードから見て、（１プレイヤーから見て）左隣のカードをピックアップするように変えます
                var timedCommandArg = new GuiOfTimedCommandArgs.Model(new MoveFocusToNextCardModel(
                    player: player,
                    direction: 1));

                spamSeconds[player] = timedCommandArg.Duration;
                scheduleRegister.AddJustNow(timedCommandArg);
            }
            else if (pickupCardToForward[player])
            {
                // １プレイヤーのピックアップしているカードから見て、（１プレイヤーから見て）右隣のカードをピックアップするように変えます
                var timedCommandArg = new GuiOfTimedCommandArgs.Model(new MoveFocusToNextCardModel(
                    player: player,
                    direction: 0));

                spamSeconds[player] = timedCommandArg.Duration;
                scheduleRegister.AddJustNow(timedCommandArg);
            }
        }

        // ２プレイヤー
        {
            var player = 1;

            if (handled[player])
            {

            }
            else if (pickupCardToBackward[player])
            {
                // ２プレイヤーのピックアップしているカードから見て、（２プレイヤーから見て）左隣のカードをピックアップするように変えます
                var timedCommandArg = new GuiOfTimedCommandArgs.Model(new MoveFocusToNextCardModel(
                    player: player,
                    direction: 1));

                spamSeconds[player] = timedCommandArg.Duration;
                scheduleRegister.AddJustNow(timedCommandArg);
            }
            else if (pickupCardToForward[player])
            {
                // ２プレイヤーのピックアップしているカードから見て、（２プレイヤーから見て）右隣のカードをピックアップするように変えます
                var timedCommandArg = new GuiOfTimedCommandArgs.Model(new MoveFocusToNextCardModel(
                    player: player,
                    direction: 0));

                spamSeconds[player] = timedCommandArg.Duration;
                scheduleRegister.AddJustNow(timedCommandArg);
            }
        }

        // デバッグ用
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 両プレイヤーは手札から１枚抜いて、場札として置く
            for (var player = 0; player < 2; player++)
            {
                // 場札を並べる
                var timedCommandArg = new GuiOfTimedCommandArgs.Model(new MoveCardsToHandFromPileModel(
                    player: player,
                    numberOfCards: 1));

                spamSeconds[player] = timedCommandArg.Duration;
                scheduleRegister.AddJustNow(timedCommandArg);
            }
        }
    }
}
