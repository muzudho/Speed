using Assets.Scripts.Gui.Models;
using Assets.Scripts.Gui.Models.Timeline.Spans;
using Assets.Scripts.Simulators;
using Assets.Scripts.Views;
using Assets.Scripts.Views.Timeline;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SimulatorsOfTimeline = Assets.Scripts.Simulators;
using ViewsOfTimeline = Assets.Scripts.Views.Timeline;

/// <summary>
/// ゲーム・マネージャー
/// 
/// - スピードは、日本と海外で　ルールとプレイング・スタイルに違いがあるので、用語に統一感はない
/// </summary>
public class GameManager : MonoBehaviour
{
    internal SimulatorsOfTimeline.ScheduleRegister ScheduleRegister { get; private set; }

    ViewsOfTimeline.PlayerToLerp playerToLerp;
    GameModelBuffer gameModelBuffer;
    GameModel gameModel;

    // ゲーム内単位時間
    float tickSeconds = 1.0f / 60.0f;

    // Start is called before the first frame update
    void Start()
    {
        // 全てのカードのゲーム・オブジェクトを、IDに紐づける
        GameObjectStorage.Add(IdOfGameObjects.Clubs1, GameObject.Find($"Clubs 1"));
        GameObjectStorage.Add(IdOfGameObjects.Clubs2, GameObject.Find($"Clubs 2"));
        GameObjectStorage.Add(IdOfGameObjects.Clubs3, GameObject.Find($"Clubs 3"));
        GameObjectStorage.Add(IdOfGameObjects.Clubs4, GameObject.Find($"Clubs 4"));
        GameObjectStorage.Add(IdOfGameObjects.Clubs5, GameObject.Find($"Clubs 5"));
        GameObjectStorage.Add(IdOfGameObjects.Clubs6, GameObject.Find($"Clubs 6"));
        GameObjectStorage.Add(IdOfGameObjects.Clubs7, GameObject.Find($"Clubs 7"));
        GameObjectStorage.Add(IdOfGameObjects.Clubs8, GameObject.Find($"Clubs 8"));
        GameObjectStorage.Add(IdOfGameObjects.Clubs9, GameObject.Find($"Clubs 9"));
        GameObjectStorage.Add(IdOfGameObjects.Clubs10, GameObject.Find($"Clubs 10"));
        GameObjectStorage.Add(IdOfGameObjects.Clubs11, GameObject.Find($"Clubs 11"));
        GameObjectStorage.Add(IdOfGameObjects.Clubs12, GameObject.Find($"Clubs 12"));
        GameObjectStorage.Add(IdOfGameObjects.Clubs13, GameObject.Find($"Clubs 13"));

        GameObjectStorage.Add(IdOfGameObjects.Diamonds1, GameObject.Find($"Diamonds 1"));
        GameObjectStorage.Add(IdOfGameObjects.Diamonds2, GameObject.Find($"Diamonds 2"));
        GameObjectStorage.Add(IdOfGameObjects.Diamonds3, GameObject.Find($"Diamonds 3"));
        GameObjectStorage.Add(IdOfGameObjects.Diamonds4, GameObject.Find($"Diamonds 4"));
        GameObjectStorage.Add(IdOfGameObjects.Diamonds5, GameObject.Find($"Diamonds 5"));
        GameObjectStorage.Add(IdOfGameObjects.Diamonds6, GameObject.Find($"Diamonds 6"));
        GameObjectStorage.Add(IdOfGameObjects.Diamonds7, GameObject.Find($"Diamonds 7"));
        GameObjectStorage.Add(IdOfGameObjects.Diamonds8, GameObject.Find($"Diamonds 8"));
        GameObjectStorage.Add(IdOfGameObjects.Diamonds9, GameObject.Find($"Diamonds 9"));
        GameObjectStorage.Add(IdOfGameObjects.Diamonds10, GameObject.Find($"Diamonds 10"));
        GameObjectStorage.Add(IdOfGameObjects.Diamonds11, GameObject.Find($"Diamonds 11"));
        GameObjectStorage.Add(IdOfGameObjects.Diamonds12, GameObject.Find($"Diamonds 12"));
        GameObjectStorage.Add(IdOfGameObjects.Diamonds13, GameObject.Find($"Diamonds 13"));

        GameObjectStorage.Add(IdOfGameObjects.Hearts1, GameObject.Find($"Hearts 1"));
        GameObjectStorage.Add(IdOfGameObjects.Hearts2, GameObject.Find($"Hearts 2"));
        GameObjectStorage.Add(IdOfGameObjects.Hearts3, GameObject.Find($"Hearts 3"));
        GameObjectStorage.Add(IdOfGameObjects.Hearts4, GameObject.Find($"Hearts 4"));
        GameObjectStorage.Add(IdOfGameObjects.Hearts5, GameObject.Find($"Hearts 5"));
        GameObjectStorage.Add(IdOfGameObjects.Hearts6, GameObject.Find($"Hearts 6"));
        GameObjectStorage.Add(IdOfGameObjects.Hearts7, GameObject.Find($"Hearts 7"));
        GameObjectStorage.Add(IdOfGameObjects.Hearts8, GameObject.Find($"Hearts 8"));
        GameObjectStorage.Add(IdOfGameObjects.Hearts9, GameObject.Find($"Hearts 9"));
        GameObjectStorage.Add(IdOfGameObjects.Hearts10, GameObject.Find($"Hearts 10"));
        GameObjectStorage.Add(IdOfGameObjects.Hearts11, GameObject.Find($"Hearts 11"));
        GameObjectStorage.Add(IdOfGameObjects.Hearts12, GameObject.Find($"Hearts 12"));
        GameObjectStorage.Add(IdOfGameObjects.Hearts13, GameObject.Find($"Hearts 13"));

        GameObjectStorage.Add(IdOfGameObjects.Spades1, GameObject.Find($"Spades 1"));
        GameObjectStorage.Add(IdOfGameObjects.Spades2, GameObject.Find($"Spades 2"));
        GameObjectStorage.Add(IdOfGameObjects.Spades3, GameObject.Find($"Spades 3"));
        GameObjectStorage.Add(IdOfGameObjects.Spades4, GameObject.Find($"Spades 4"));
        GameObjectStorage.Add(IdOfGameObjects.Spades5, GameObject.Find($"Spades 5"));
        GameObjectStorage.Add(IdOfGameObjects.Spades6, GameObject.Find($"Spades 6"));
        GameObjectStorage.Add(IdOfGameObjects.Spades7, GameObject.Find($"Spades 7"));
        GameObjectStorage.Add(IdOfGameObjects.Spades8, GameObject.Find($"Spades 8"));
        GameObjectStorage.Add(IdOfGameObjects.Spades9, GameObject.Find($"Spades 9"));
        GameObjectStorage.Add(IdOfGameObjects.Spades10, GameObject.Find($"Spades 10"));
        GameObjectStorage.Add(IdOfGameObjects.Spades11, GameObject.Find($"Spades 11"));
        GameObjectStorage.Add(IdOfGameObjects.Spades12, GameObject.Find($"Spades 12"));
        GameObjectStorage.Add(IdOfGameObjects.Spades13, GameObject.Find($"Spades 13"));

        // Lerp を実行するだけのクラス
        playerToLerp = new PlayerToLerp();

        // タイムライン・シミュレーターは、タイム・スパンを持つ。
        ScheduleRegister = new SimulatorsOfTimeline.ScheduleRegister();
        gameModelBuffer = new GameModelBuffer();
        gameModel = new GameModel(gameModelBuffer);

        // ゲーム初期状態へセット
        {
            // ゲーム開始時、とりあえず、すべてのカードを集める
            List<IdOfPlayingCards> cardsOfGame = new();
            foreach (var idOfGo in GameObjectStorage.CreatePlayingCards().Keys)
            {
                cardsOfGame.Add(Specification.GetIdOfPlayingCard(idOfGo));
            }

            // すべてのカードをシャッフル
            cardsOfGame = cardsOfGame.OrderBy(i => Guid.NewGuid()).ToList();

            // すべてのカードを、色分けして、黒色なら１プレイヤーの、赤色なら２プレイヤーの、手札に乗せる
            foreach (var idOfCard in cardsOfGame)
            {
                int player;
                switch (idOfCard.Suit())
                {
                    case IdOfCardSuits.Clubs:
                    case IdOfCardSuits.Spades:
                        player = 0;
                        break;
                    case IdOfCardSuits.Diamonds:
                    case IdOfCardSuits.Hearts:
                        player = 1;
                        break;
                    default:
                        throw new Exception();
                }

                gameModelBuffer.AddCardOfPlayersPile(player, idOfCard);

                // 画面上の位置も調整
                var goCard = GameObjectStorage.Items[Specification.GetIdOfGameObject(idOfCard)];

                var length = gameModelBuffer.IdOfCardsOfPlayersPile[player].Count;
                // 最初の１枚目
                if (length == 1)
                {
                    var position = GameView.positionOfPileCardsOrigin[player];
                    goCard.transform.position = position.ToMutable();
                    // 裏返す
                    goCard.transform.rotation = Quaternion.Euler(
                        x: goCard.transform.rotation.x,
                        y: goCard.transform.rotation.y,
                        z: 180.0f);
                }
                else
                {
                    var previousTopCard = gameModelBuffer.IdOfCardsOfPlayersPile[player][length - 2]; // 天辺より１つ下のカードが、前のカード
                    var goPreviousTopCard = GameObjectStorage.Items[Specification.GetIdOfGameObject(previousTopCard)];
                    goCard.transform.position = GameView.yOfCardThickness.Add(goPreviousTopCard.transform.position); // 下のカードの上に被せる
                                                                                                                       // 裏返す
                    goCard.transform.rotation = Quaternion.Euler(
                        x: goCard.transform.rotation.x,
                        y: goCard.transform.rotation.y,
                        z: 180.0f);
                }
            }
        }

        const int right = 0;// 台札の右
                            // const int left = 1;// 台札の左
        while (0 < gameModel.GetLengthOfCenterStackCards(right))
        {
            // 即実行
            var spanModel = new MoveCardsToPileFromCenterStacksModel(
                    place: right
                    );
            var timeSpan = new SimulatorsOfTimeline.TimeSpan(
                    startSeconds: 0.0f,
                    spanModel: spanModel,
                    spanView: Specification.SpawnViewFromModel(spanModel.GetType()));
            timeSpan.SpanView.OnEnter(
                timeSpan,
                gameModelBuffer,
                setViewMovement: (movementViewModel) => movementViewModel.Lerp(1.0f));
        }

        // １，２プレイヤーについて、手札から５枚抜いて、場札として置く（画面上の場札の位置は調整される）
        {
            var player = 0;
            var spanModel = new MoveCardsToHandFromPileModel(
                    player: player,
                    numberOfCards: 5);
            this.ScheduleRegister.AddWithinScheduler(player, spanModel);
        }
        {
            var player = 1;
            var spanModel = new MoveCardsToHandFromPileModel(
                    player: player,
                    numberOfCards: 5);
            this.ScheduleRegister.AddWithinScheduler(player, spanModel);
        }

        // 以下、デモ・プレイを登録
        SetupDemo();

        // OnTick を 1.0 秒後に呼び出し、以降は tickSeconds 秒毎に実行
        InvokeRepeating(nameof(OnTick), 1.0f, tickSeconds);
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
        // モデルからビューへ、起動したタイム・スパンを引き継ぎたい
        var viewMovement = new List<SpanToLerp>();

        // 時限式で、ゲーム画面の同期を始めます
        this.ScheduleRegister.OnEnter(
            this.gameModelBuffer.ElapsedSeconds,
            gameModelBuffer,
            setViewMovement: (movementViewModel) =>
            {
                viewMovement.Add(movementViewModel);
            });

        // モーションの補間
        this.playerToLerp.Lerp(this.gameModelBuffer.ElapsedSeconds, viewMovement);

        this.ScheduleRegister.DebugWrite(); // TODO ★ 消す
        this.playerToLerp.DebugWrite(); // TODO ★ 消す

        this.gameModelBuffer.ElapsedSeconds += tickSeconds;
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
            var player = 0;
            var spanModel = new MoveCardToCenterStackFromHandModel(
                    player: player, // １プレイヤーが
                    place: right); // 右の
            this.ScheduleRegister.AddJustNow(
                this.gameModelBuffer.ElapsedSeconds,
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
            this.ScheduleRegister.AddJustNow(
                this.gameModelBuffer.ElapsedSeconds, 
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
            this.ScheduleRegister.AddJustNow(
                this.gameModelBuffer.ElapsedSeconds, 
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
            this.ScheduleRegister.AddJustNow(
                this.gameModelBuffer.ElapsedSeconds, 
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
                    direction: 1,
                    setIndexOfNextFocusedHandCard: (indexOfNextFocusedHandCard) =>
                    {
                        gameModelBuffer.IndexOfFocusedCardOfPlayers[player] = indexOfNextFocusedHandCard;     // 更新
                    });
            this.ScheduleRegister.AddJustNow(
                this.gameModelBuffer.ElapsedSeconds, 
                spanModel);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // １プレイヤーのピックアップしているカードから見て、（１プレイヤーから見て）右隣のカードをピックアップするように変えます
            var player = 0;
            var spanModel = new MoveFocusToNextCardModel(
                    player: player,
                    direction: 0,
                    setIndexOfNextFocusedHandCard: (indexOfNextFocusedHandCard) =>
                    {
                        gameModelBuffer.IndexOfFocusedCardOfPlayers[player] = indexOfNextFocusedHandCard;     // 更新
                    });
            this.ScheduleRegister.AddJustNow(
                this.gameModelBuffer.ElapsedSeconds, 
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
                    direction: 1,
                    setIndexOfNextFocusedHandCard: (indexOfNextFocusedHandCard) =>
                    {
                        gameModelBuffer.IndexOfFocusedCardOfPlayers[player] = indexOfNextFocusedHandCard;     // 更新
                    });
            this.ScheduleRegister.AddJustNow(
                this.gameModelBuffer.ElapsedSeconds, 
                spanModel);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            // ２プレイヤーのピックアップしているカードから見て、（２プレイヤーから見て）右隣のカードをピックアップするように変えます
            var player = 1;
            var spanModel = new MoveFocusToNextCardModel(
                    player: player,
                    direction: 0,
                    setIndexOfNextFocusedHandCard: (indexOfNextFocusedHandCard) =>
                    {
                        gameModelBuffer.IndexOfFocusedCardOfPlayers[player] = indexOfNextFocusedHandCard;     // 更新
                    });
            this.ScheduleRegister.AddJustNow(
                this.gameModelBuffer.ElapsedSeconds, 
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
                this.ScheduleRegister.AddJustNow(
                    this.gameModelBuffer.ElapsedSeconds, 
                    spanModel);
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

        // 間
        float interval = 0.85f;

        // 間
        for (int player = 0; player < 2; player++)
        {
            this.ScheduleRegister.AddScheduleSeconds(player: player, seconds: interval);
        }

        // 登録：ピックアップ場札を、台札へ積み上げる
        {
            {
                // １プレイヤーが、ピックアップ中の場札を抜いて、右の台札へ積み上げる
                var player = 0;
                var spanModel = new MoveCardToCenterStackFromHandModel(
                        player: player, // １プレイヤーが
                        place: right); // 右の
                this.ScheduleRegister.AddWithinScheduler(player, spanModel);
            }
            {
                // ２プレイヤーが、ピックアップ中の場札を抜いて、左の台札へ積み上げる
                var player = 1;
                var spanModel = new MoveCardToCenterStackFromHandModel(
                        player: player, // ２プレイヤーが
                        place: left); // 左の;
                this.ScheduleRegister.AddWithinScheduler(player, spanModel);
            }
        }

        // 間
        for (int player = 0; player < 2; player++)
        {
            this.ScheduleRegister.AddScheduleSeconds(player: player, seconds: interval);
        }

        // ゲーム・デモ開始

        // 登録：カード選択
        {
            for (int i = 0; i < 2; i++)
            {
                // １プレイヤーの右隣のカードへフォーカスを移します
                {
                    var player = 0;
                    var spanModel = new MoveFocusToNextCardModel(
                            player: player,
                            direction: 0,
                            setIndexOfNextFocusedHandCard: (indexOfNextFocusedHandCard) =>
                            {
                                gameModelBuffer.IndexOfFocusedCardOfPlayers[player] = indexOfNextFocusedHandCard;     // 更新
                            });
                    this.ScheduleRegister.AddWithinScheduler(player, spanModel);
                }

                // ２プレイヤーの右隣のカードへフォーカスを移します
                {
                    var player = 1;
                    var spanModel = new MoveFocusToNextCardModel(
                            player: player,
                            direction: 0,
                            setIndexOfNextFocusedHandCard: (indexOfNextFocusedHandCard) =>
                            {
                                gameModelBuffer.IndexOfFocusedCardOfPlayers[player] = indexOfNextFocusedHandCard;     // 更新
                            });
                    this.ScheduleRegister.AddWithinScheduler(player, spanModel);
                }

                // 間
                for (int player = 0; player < 2; player++)
                {
                    this.ScheduleRegister.AddScheduleSeconds(player: player, seconds: interval);
                }
            }
        }

        // 登録：台札を積み上げる
        {
            {
                var player = 0;
                var spanModel = new MoveCardToCenterStackFromHandModel(
                        player: player, // １プレイヤーが
                        place: 1); // 左の台札
                this.ScheduleRegister.AddWithinScheduler(player, spanModel);
            }
            {
                var player = 1;
                var spanModel = new MoveCardToCenterStackFromHandModel(
                        player: player, // ２プレイヤーが
                        place: 0); // 右の台札
                this.ScheduleRegister.AddWithinScheduler(player, spanModel);
            }
        }

        // 間
        for (int player = 0; player < 2; player++)
        {
            this.ScheduleRegister.AddScheduleSeconds(player: player, seconds: interval);
        }

        // 登録：手札から１枚引く
        {
            {
                // １プレイヤーは手札から１枚抜いて、場札として置く
                var player = 0;
                var spanModel = new MoveCardsToHandFromPileModel(
                        player: player,
                        numberOfCards: 1);
                this.ScheduleRegister.AddWithinScheduler(player, spanModel);
            }
            {
                // ２プレイヤーは手札から１枚抜いて、場札として置く
                var player = 1;
                var spanModel = new MoveCardsToHandFromPileModel(
                        player: 1,
                        numberOfCards: 1);
                this.ScheduleRegister.AddWithinScheduler(player, spanModel);
            }
        }
    }
}
