using Assets.Scripts.Models;
using Assets.Scripts.Views;
using System;
using System.Linq;
using UnityEngine;
using Commands = Assets.Scripts.Models.Timeline.Commands;
using ModelsOfTimeline = Assets.Scripts.Models.Timeline;

/// <summary>
/// ゲーム・マネージャー
/// 
/// - スピードは、日本と海外で　ルールとプレイング・スタイルに違いがあるので、用語に統一感はない
/// </summary>
public class GameManager : MonoBehaviour
{
    ModelsOfTimeline.Model commandStorage;
    GameModelBuffer gameModelBuffer;
    GameModel gameModel;
    GameViewModel gameViewModel;

    // ゲーム内単位時間
    float unitSeconds = 1.0f / 60.0f;
    // ゲーム内経過時間
    float elapsedSeconds = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        // 全てのカードのゲーム・オブジェクトを、IDに紐づける
        GameObjectStorage.Add(IdOfPlayingCards.Clubs1, GameObject.Find($"Clubs 1"));
        GameObjectStorage.Add(IdOfPlayingCards.Clubs2, GameObject.Find($"Clubs 2"));
        GameObjectStorage.Add(IdOfPlayingCards.Clubs3, GameObject.Find($"Clubs 3"));
        GameObjectStorage.Add(IdOfPlayingCards.Clubs4, GameObject.Find($"Clubs 4"));
        GameObjectStorage.Add(IdOfPlayingCards.Clubs5, GameObject.Find($"Clubs 5"));
        GameObjectStorage.Add(IdOfPlayingCards.Clubs6, GameObject.Find($"Clubs 6"));
        GameObjectStorage.Add(IdOfPlayingCards.Clubs7, GameObject.Find($"Clubs 7"));
        GameObjectStorage.Add(IdOfPlayingCards.Clubs8, GameObject.Find($"Clubs 8"));
        GameObjectStorage.Add(IdOfPlayingCards.Clubs9, GameObject.Find($"Clubs 9"));
        GameObjectStorage.Add(IdOfPlayingCards.Clubs10, GameObject.Find($"Clubs 10"));
        GameObjectStorage.Add(IdOfPlayingCards.Clubs11, GameObject.Find($"Clubs 11"));
        GameObjectStorage.Add(IdOfPlayingCards.Clubs12, GameObject.Find($"Clubs 12"));
        GameObjectStorage.Add(IdOfPlayingCards.Clubs13, GameObject.Find($"Clubs 13"));

        GameObjectStorage.Add(IdOfPlayingCards.Diamonds1, GameObject.Find($"Diamonds 1"));
        GameObjectStorage.Add(IdOfPlayingCards.Diamonds2, GameObject.Find($"Diamonds 2"));
        GameObjectStorage.Add(IdOfPlayingCards.Diamonds3, GameObject.Find($"Diamonds 3"));
        GameObjectStorage.Add(IdOfPlayingCards.Diamonds4, GameObject.Find($"Diamonds 4"));
        GameObjectStorage.Add(IdOfPlayingCards.Diamonds5, GameObject.Find($"Diamonds 5"));
        GameObjectStorage.Add(IdOfPlayingCards.Diamonds6, GameObject.Find($"Diamonds 6"));
        GameObjectStorage.Add(IdOfPlayingCards.Diamonds7, GameObject.Find($"Diamonds 7"));
        GameObjectStorage.Add(IdOfPlayingCards.Diamonds8, GameObject.Find($"Diamonds 8"));
        GameObjectStorage.Add(IdOfPlayingCards.Diamonds9, GameObject.Find($"Diamonds 9"));
        GameObjectStorage.Add(IdOfPlayingCards.Diamonds10, GameObject.Find($"Diamonds 10"));
        GameObjectStorage.Add(IdOfPlayingCards.Diamonds11, GameObject.Find($"Diamonds 11"));
        GameObjectStorage.Add(IdOfPlayingCards.Diamonds12, GameObject.Find($"Diamonds 12"));
        GameObjectStorage.Add(IdOfPlayingCards.Diamonds13, GameObject.Find($"Diamonds 13"));

        GameObjectStorage.Add(IdOfPlayingCards.Hearts1, GameObject.Find($"Hearts 1"));
        GameObjectStorage.Add(IdOfPlayingCards.Hearts2, GameObject.Find($"Hearts 2"));
        GameObjectStorage.Add(IdOfPlayingCards.Hearts3, GameObject.Find($"Hearts 3"));
        GameObjectStorage.Add(IdOfPlayingCards.Hearts4, GameObject.Find($"Hearts 4"));
        GameObjectStorage.Add(IdOfPlayingCards.Hearts5, GameObject.Find($"Hearts 5"));
        GameObjectStorage.Add(IdOfPlayingCards.Hearts6, GameObject.Find($"Hearts 6"));
        GameObjectStorage.Add(IdOfPlayingCards.Hearts7, GameObject.Find($"Hearts 7"));
        GameObjectStorage.Add(IdOfPlayingCards.Hearts8, GameObject.Find($"Hearts 8"));
        GameObjectStorage.Add(IdOfPlayingCards.Hearts9, GameObject.Find($"Hearts 9"));
        GameObjectStorage.Add(IdOfPlayingCards.Hearts10, GameObject.Find($"Hearts 10"));
        GameObjectStorage.Add(IdOfPlayingCards.Hearts11, GameObject.Find($"Hearts 11"));
        GameObjectStorage.Add(IdOfPlayingCards.Hearts12, GameObject.Find($"Hearts 12"));
        GameObjectStorage.Add(IdOfPlayingCards.Hearts13, GameObject.Find($"Hearts 13"));

        GameObjectStorage.Add(IdOfPlayingCards.Spades1, GameObject.Find($"Spades 1"));
        GameObjectStorage.Add(IdOfPlayingCards.Spades2, GameObject.Find($"Spades 2"));
        GameObjectStorage.Add(IdOfPlayingCards.Spades3, GameObject.Find($"Spades 3"));
        GameObjectStorage.Add(IdOfPlayingCards.Spades4, GameObject.Find($"Spades 4"));
        GameObjectStorage.Add(IdOfPlayingCards.Spades5, GameObject.Find($"Spades 5"));
        GameObjectStorage.Add(IdOfPlayingCards.Spades6, GameObject.Find($"Spades 6"));
        GameObjectStorage.Add(IdOfPlayingCards.Spades7, GameObject.Find($"Spades 7"));
        GameObjectStorage.Add(IdOfPlayingCards.Spades8, GameObject.Find($"Spades 8"));
        GameObjectStorage.Add(IdOfPlayingCards.Spades9, GameObject.Find($"Spades 9"));
        GameObjectStorage.Add(IdOfPlayingCards.Spades10, GameObject.Find($"Spades 10"));
        GameObjectStorage.Add(IdOfPlayingCards.Spades11, GameObject.Find($"Spades 11"));
        GameObjectStorage.Add(IdOfPlayingCards.Spades12, GameObject.Find($"Spades 12"));
        GameObjectStorage.Add(IdOfPlayingCards.Spades13, GameObject.Find($"Spades 13"));

        commandStorage = new ModelsOfTimeline.Model();
        gameModelBuffer = new GameModelBuffer();
        gameModel = new GameModel(gameModelBuffer);
        gameViewModel = new GameViewModel();

        // ゲーム開始時、とりあえず、すべてのカードは、いったん右の台札という扱いにする
        const int right = 0;// 台札の右
                            // const int left = 1;// 台札の左
        foreach (var idOfCard in GameObjectStorage.PlayingCards.Keys)
        {
            // 右の台札
            gameModelBuffer.IdOfCardsOfCenterStacks[right].Add(idOfCard);
        }

        // 右の台札をシャッフル
        gameModelBuffer.IdOfCardsOfCenterStacks[right] = gameModelBuffer.IdOfCardsOfCenterStacks[right].OrderBy(i => Guid.NewGuid()).ToList();

        // 右の台札をすべて、色分けして、黒色なら１プレイヤーの、赤色なら２プレイヤーの、手札に乗せる
        while (0 < gameModel.GetLengthOfCenterStackCards(right))
        {
            // 即実行
            new Commands.MoveCardsToPileFromCenterStacks(place: right).DoIt(gameModelBuffer, gameViewModel);
        }

        // １，２プレイヤーについて、手札から５枚抜いて、場札として置く（画面上の場札の位置は調整される）
        var time = 0.0f;
        this.commandStorage.Add(time, unitSeconds, new Commands.MoveCardsToHandFromPile(player: 0, numberOfCards: 5));
        this.commandStorage.Add(time, unitSeconds, new Commands.MoveCardsToHandFromPile(player: 1, numberOfCards: 5));

        // 以下、デモ・プレイを登録
        SetupDemo();

        // OnTick を 1.0 秒後に呼び出し、以降は unitSeconds 秒毎に実行
        InvokeRepeating(nameof(OnTick), 1.0f, unitSeconds);
    }

    // Update is called once per frame
    void Update()
    {
        // 入力をコマンドとして登録
        UpdateInput();
    }

    /// <summary>
    /// 一定間隔で呼び出される
    /// </summary>
    void OnTick()
    {
        // 時限式で、コマンドを消化
        this.commandStorage.DoIt(elapsedSeconds, gameModelBuffer, gameViewModel);

        // モーションの補間
        this.commandStorage.Leap(elapsedSeconds);

        this.commandStorage.DebugWrite(); // TODO ★ 消す

        elapsedSeconds += unitSeconds;
    }

    /// <summary>
    /// 入力を、コマンドに変換して、タイムラインへ登録します
    /// </summary>
    private void UpdateInput()
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
            this.commandStorage.Add(elapsedSeconds, unitSeconds, new Commands.MoveCardToCenterStackFromHand(
                player: 0, // １プレイヤーが
                place: right // 右の
                ));
            handled1player = true;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            // ２プレイヤーが、ピックアップ中の場札を抜いて、（１プレイヤーから見て）右の台札へ積み上げる
            this.commandStorage.Add(elapsedSeconds, unitSeconds, new Commands.MoveCardToCenterStackFromHand(
                player: 1, // ２プレイヤーが
                place: right // 右の
                ));
            handled2player = true;
        }

        // （ボタン押下が同時なら）左の台札は２プレイヤー優先
        // ==================================================

        // ２プレイヤー
        if (Input.GetKeyDown(KeyCode.S))
        {
            // ２プレイヤーが、ピックアップ中の場札を抜いて、（１プレイヤーから見て）左の台札へ積み上げる
            this.commandStorage.Add(elapsedSeconds, unitSeconds, new Commands.MoveCardToCenterStackFromHand(
                player: 1, // ２プレイヤーが
                place: left // 左の
                ));
            handled2player = true;
        }

        // １プレイヤー
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // １プレイヤーが、ピックアップ中の場札を抜いて、（１プレイヤーから見て）左の台札へ積み上げる
            this.commandStorage.Add(elapsedSeconds, unitSeconds, new Commands.MoveCardToCenterStackFromHand(
                player: 0, // １プレイヤーが
                place: left // 左の
                ));
            handled1player = true;
        }

        // それ以外のキー入力は、同時でも勝敗に関係しない
        // ==============================================

        // １プレイヤー
        if(handled1player)
        {

        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // １プレイヤーのピックアップしているカードから見て、（１プレイヤーから見て）左隣のカードをピックアップするように変えます
            var player = 0;
            this.commandStorage.Add(elapsedSeconds, unitSeconds, new Commands.MoveFocusToNextCard(
                player: player,
                direction: 1,
                setIndexOfNextFocusedHandCard: (indexOfNextFocusedHandCard) =>
                {
                    gameModelBuffer.IndexOfFocusedCardOfPlayers[player] = indexOfNextFocusedHandCard;     // 更新
                }));
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // １プレイヤーのピックアップしているカードから見て、（１プレイヤーから見て）右隣のカードをピックアップするように変えます
            var player = 0;
            this.commandStorage.Add(elapsedSeconds, unitSeconds, new Commands.MoveFocusToNextCard(
                player: player,
                direction: 0,
                setIndexOfNextFocusedHandCard: (indexOfNextFocusedHandCard) =>
                {
                    gameModelBuffer.IndexOfFocusedCardOfPlayers[player] = indexOfNextFocusedHandCard;     // 更新
                }));
        }

        // ２プレイヤー
        if(handled2player)
        {

        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            // ２プレイヤーのピックアップしているカードから見て、（２プレイヤーから見て）左隣のカードをピックアップするように変えます
            var player = 1;
            this.commandStorage.Add(elapsedSeconds, unitSeconds, new Commands.MoveFocusToNextCard(
                player: player,
                direction: 1,
                setIndexOfNextFocusedHandCard: (indexOfNextFocusedHandCard) =>
                {
                    gameModelBuffer.IndexOfFocusedCardOfPlayers[player] = indexOfNextFocusedHandCard;     // 更新
                }));
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            // ２プレイヤーのピックアップしているカードから見て、（２プレイヤーから見て）右隣のカードをピックアップするように変えます
            var player = 1;
            this.commandStorage.Add(elapsedSeconds, unitSeconds, new Commands.MoveFocusToNextCard(
                player: player,
                direction: 0,
                setIndexOfNextFocusedHandCard: (indexOfNextFocusedHandCard) =>
                {
                    gameModelBuffer.IndexOfFocusedCardOfPlayers[player] = indexOfNextFocusedHandCard;     // 更新
                }));
        }

        // デバッグ用
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 両プレイヤーは手札から１枚抜いて、場札として置く
            for (var player = 0; player < 2; player++)
            {
                // 場札を並べる
                this.commandStorage.Add(elapsedSeconds, unitSeconds, new Commands.MoveCardsToHandFromPile(
                    player: player,
                    numberOfCards: 1));
            }
        }
    }

    /// <summary>
    /// タイムライン作成
    /// 
    /// - デモ
    /// </summary>
    void SetupDemo()
    {
        // 卓準備
        const int right = 0;// 台札の右
        const int left = 1;// 台札の左

        float scheduleSeconds = 1.0f;
        float oneSecond = 1.0f;

        // 登録：ピックアップ場札を、台札へ積み上げる
        {
            // １プレイヤーが、ピックアップ中の場札を抜いて、右の台札へ積み上げる
            this.commandStorage.Add(scheduleSeconds, unitSeconds, new Commands.MoveCardToCenterStackFromHand(
                player: 0, // １プレイヤーが
                place: right // 右の
                ));

            // ２プレイヤーが、ピックアップ中の場札を抜いて、左の台札へ積み上げる
            this.commandStorage.Add(scheduleSeconds, unitSeconds, new Commands.MoveCardToCenterStackFromHand(
                player: 1, // ２プレイヤーが
                place: left // 左の
                ));

            scheduleSeconds += oneSecond;
        }

        // ゲーム・デモ開始

        // 登録：カード選択
        {
            for (int i = 0; i < 2; i++)
            {
                // １プレイヤーの右隣のカードへフォーカスを移します
                {
                    var player = 0;
                    this.commandStorage.Add(scheduleSeconds, unitSeconds, new Commands.MoveFocusToNextCard(
                        player: player,
                        direction: 0,
                        setIndexOfNextFocusedHandCard: (indexOfNextFocusedHandCard) =>
                        {
                            gameModelBuffer.IndexOfFocusedCardOfPlayers[player] = indexOfNextFocusedHandCard;     // 更新
                        }));
                }

                // ２プレイヤーの右隣のカードへフォーカスを移します
                {
                    var player = 1;
                    this.commandStorage.Add(scheduleSeconds, unitSeconds, new Commands.MoveFocusToNextCard(
                        player: player,
                        direction: 0,
                        setIndexOfNextFocusedHandCard: (indexOfNextFocusedHandCard) =>
                        {
                            gameModelBuffer.IndexOfFocusedCardOfPlayers[player] = indexOfNextFocusedHandCard;     // 更新
                        }));
                }

                scheduleSeconds += oneSecond;
            }
        }

        // 登録：台札を積み上げる
        {
            this.commandStorage.Add(scheduleSeconds, unitSeconds, new Commands.MoveCardToCenterStackFromHand(
                player: 0, // １プレイヤーが
                place: 1 // 左の台札
                ));

            this.commandStorage.Add(scheduleSeconds, unitSeconds, new Commands.MoveCardToCenterStackFromHand(
                player: 1, // ２プレイヤーが
                place: 0 // 右の台札
                ));

            scheduleSeconds += oneSecond;
        }
        // 登録：手札から１枚引く
        {
            // １プレイヤーは手札から１枚抜いて、場札として置く
            this.commandStorage.Add(scheduleSeconds, unitSeconds, new Commands.MoveCardsToHandFromPile(
                player: 0,
                numberOfCards: 1));

            // ２プレイヤーは手札から１枚抜いて、場札として置く
            this.commandStorage.Add(scheduleSeconds, unitSeconds, new Commands.MoveCardsToHandFromPile(
                player: 1,
                numberOfCards: 1));

            scheduleSeconds += oneSecond;
        }
    }
}
