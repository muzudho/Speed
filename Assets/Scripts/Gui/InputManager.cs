using Assets.Scripts.ThinkingEngine.CommandArgs;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    /// <summary>
    /// Update is called once per frame
    /// 
    /// - 入力は、すぐに実行は、しません
    /// - 入力は、コマンドに変換して、タイムラインへ登録します
    /// </summary>
    void Update()
    {
        const int right = 0;// 台札の右
        const int left = 1;// 台札の左
        bool handled1player = false;
        bool handled2player = false;

        // 先に登録したコマンドの方が早く実行される

        // （ボタン押下が同時なら）右の台札は１プレイヤー優先
        // ==================================================

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // １プレイヤーが、ピックアップ中の場札を抜いて、（１プレイヤーから見て）右の台札へ積み上げる
            var player = 0;
            var spanModel = new MoveCardToCenterStackFromHandModel(
                    player: player, // １プレイヤーが
                    place: right); // 右の
            gameManager.ScheduleRegister.AddJustNow(
                gameManager.ElapsedSeconds,
                spanModel);
            handled1player = true;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            // ２プレイヤーが、ピックアップ中の場札を抜いて、（１プレイヤーから見て）右の台札へ積み上げる
            var player = 1;
            var spanModel = new MoveCardToCenterStackFromHandModel(
                    player: player, // ２プレイヤーが
                    place: right); // 右の
            gameManager.ScheduleRegister.AddJustNow(
                gameManager.ElapsedSeconds,
                spanModel);
            handled2player = true;
        }

        // （ボタン押下が同時なら）左の台札は２プレイヤー優先
        // ==================================================

        // ２プレイヤー
        if (Input.GetKeyDown(KeyCode.S))
        {
            // ２プレイヤーが、ピックアップ中の場札を抜いて、（１プレイヤーから見て）左の台札へ積み上げる
            var player = 1;
            var spanModel = new MoveCardToCenterStackFromHandModel(
                    player: player, // ２プレイヤーが
                    place: left); // 左の
            gameManager.ScheduleRegister.AddJustNow(
                gameManager.ElapsedSeconds,
                spanModel);
            handled2player = true;
        }

        // １プレイヤー
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // １プレイヤーが、ピックアップ中の場札を抜いて、（１プレイヤーから見て）左の台札へ積み上げる
            var player = 0;
            var spanModel = new MoveCardToCenterStackFromHandModel(
                    player: player, // １プレイヤーが
                    place: left); // 左の
            gameManager.ScheduleRegister.AddJustNow(
                gameManager.ElapsedSeconds,
                spanModel);
            handled1player = true;
        }

        // それ以外のキー入力は、同時でも勝敗に関係しない
        // ==============================================

        // １プレイヤー
        if (handled1player)
        {

        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // １プレイヤーのピックアップしているカードから見て、（１プレイヤーから見て）左隣のカードをピックアップするように変えます
            var player = 0;
            var spanModel = new MoveFocusToNextCardModel(
                    player: player,
                    direction: 1);
            gameManager.ScheduleRegister.AddJustNow(
                gameManager.ElapsedSeconds,
                spanModel);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // １プレイヤーのピックアップしているカードから見て、（１プレイヤーから見て）右隣のカードをピックアップするように変えます
            var player = 0;
            var spanModel = new MoveFocusToNextCardModel(
                    player: player,
                    direction: 0);
            gameManager.ScheduleRegister.AddJustNow(
                gameManager.ElapsedSeconds,
                spanModel);
        }

        // ２プレイヤー
        if (handled2player)
        {

        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            // ２プレイヤーのピックアップしているカードから見て、（２プレイヤーから見て）左隣のカードをピックアップするように変えます
            var player = 1;
            var spanModel = new MoveFocusToNextCardModel(
                    player: player,
                    direction: 1);
            gameManager.ScheduleRegister.AddJustNow(
                gameManager.ElapsedSeconds,
                spanModel);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            // ２プレイヤーのピックアップしているカードから見て、（２プレイヤーから見て）右隣のカードをピックアップするように変えます
            var player = 1;
            var spanModel = new MoveFocusToNextCardModel(
                    player: player,
                    direction: 0);
            gameManager.ScheduleRegister.AddJustNow(
                gameManager.ElapsedSeconds,
                spanModel);
        }

        // デバッグ用
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 両プレイヤーは手札から１枚抜いて、場札として置く
            for (var player = 0; player < 2; player++)
            {
                // 場札を並べる
                var spanModel = new MoveCardsToHandFromPileModel(
                        player: player,
                        numberOfCards: 1);
                gameManager.ScheduleRegister.AddJustNow(
                    gameManager.ElapsedSeconds,
                    spanModel);
            }
        }
    }
}
