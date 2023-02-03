using Assets.Scripts.Models;
using Assets.Scripts.Models.Timeline.Spans;
using Assets.Scripts.Simulators.Timeline;
using Assets.Scripts.Views;
using Assets.Scripts.Views.Timeline;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ModelsOfTimeline = Assets.Scripts.Models.Timeline;
using SimulatorsOfTimeline = Assets.Scripts.Simulators.Timeline;
using Spans = Assets.Scripts.Views.Timeline.Spans;
using TimeSpan = Assets.Scripts.Views.Timeline.TimeSpanView;
using ViewsOfTimeline = Assets.Scripts.Views.Timeline;

/// <summary>
/// ゲーム・マネージャー
/// 
/// - スピードは、日本と海外で　ルールとプレイング・スタイルに違いがあるので、用語に統一感はない
/// </summary>
public class GameManager : MonoBehaviour
{
    ViewsOfTimeline.View timelineView;
    GameModelBuffer gameModelBuffer;
    GameModel gameModel;
    GameViewModel gameViewModel;

    // ゲーム内単位時間
    float tickSeconds = 1.0f / 60.0f;

    /// <summary>
    /// 持続時間
    /// 
    /// - 隣のカードをピックアップする
    /// </summary>
    float durationOfMoveFocusToNextCard = 0.15f;

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

        // タイムライン・ビューは、タイムライン・シミュレーターを持ち、
        // タイムライン・シミュレーターは、タイムライン・モデルを持つ。
        timelineView = new ViewsOfTimeline.View(
            new SimulatorsOfTimeline.Simulator(
                new ModelsOfTimeline.Model()));
        gameModelBuffer = new GameModelBuffer();
        gameModel = new GameModel(gameModelBuffer);
        gameViewModel = new GameViewModel();

        // ゲーム開始時、とりあえず、すべてのカードは、いったん右の台札という扱いにする
        const int right = 0;// 台札の右
                            // const int left = 1;// 台札の左
        foreach (var idOfGo in GameObjectStorage.Items.Keys)
        {
            // 右の台札
            gameModelBuffer.IdOfCardsOfCenterStacks[right].Add(Specification.GetIdOfPlayingCard(idOfGo));
        }

        // 右の台札をシャッフル
        gameModelBuffer.IdOfCardsOfCenterStacks[right] = gameModelBuffer.IdOfCardsOfCenterStacks[right].OrderBy(i => Guid.NewGuid()).ToList();

        // 右の台札をすべて、色分けして、黒色なら１プレイヤーの、赤色なら２プレイヤーの、手札に乗せる
        while (0 < gameModel.GetLengthOfCenterStackCards(right))
        {
            // 即実行
            var spanModel = new MoveCardsToPileFromCenterStacksModel(
                    place: right
                    );
            var timeSpan = new TimeSpan(
                    startSeconds: 0.0f,
                    spanModel: spanModel);

            var viewSpan = Specification.SpawnView(typeof(Spans.MoveCardsToPileFromCenterStacksView), timeSpan);
            new Spans.MoveCardsToPileFromCenterStacksView(
                timeSpan).OnEnter(timeSpan, gameModelBuffer, gameViewModel,
                        setCardMovementViewModel: (cardMovementViewModel) =>
                        {
                            var cardMovementView = new CardMovementView(cardMovementViewModel);
                            cardMovementView.Lerp(1.0f);
                        });
        }

        // １，２プレイヤーについて、手札から５枚抜いて、場札として置く（画面上の場札の位置は調整される）
        {
            var player = 0;
            var spanModel = new MoveCardsToHandFromPileModel(
                    player: player,
                    numberOfCards: 5);
            var timeSpan = new TimeSpan(
                    startSeconds: this.timelineView.Simulator.ScheduledSeconds[player],
                    spanModel: spanModel);
            this.timelineView.Simulator.Model.Add(new Spans.MoveCardsToHandFromPileView(
                timeSpan: timeSpan
                ));
            this.timelineView.Simulator.AddScheduleSeconds(player, timeSpan.Duration);
        }
        {
            var player = 1;
            var spanModel = new MoveCardsToHandFromPileModel(
                    player: player,
                    numberOfCards: 5);
            var timeSpan = new TimeSpan(
                    startSeconds: this.timelineView.Simulator.ScheduledSeconds[player],
                    spanModel: spanModel);
            this.timelineView.Simulator.Model.Add(new Spans.MoveCardsToHandFromPileView(
                timeSpan: timeSpan
                ));
            this.timelineView.Simulator.AddScheduleSeconds(player, timeSpan.Duration);
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
        var launchedCardMovementModels = new List<MovementViewModel>();

        // 時限式で、コマンドを消化
        this.timelineView.Simulator.OnEnter(
            this.gameModelBuffer.ElapsedSeconds,
            gameModelBuffer,
            gameViewModel,
            setCardMovementViewModel: (cardMovementViewModel) =>
            {
                launchedCardMovementModels.Add(cardMovementViewModel);
            });

        // モーションの補間
        this.timelineView.Lerp(this.gameModelBuffer.ElapsedSeconds, launchedCardMovementModels);

        this.timelineView.Simulator.Model.DebugWrite(); // TODO ★ 消す
        this.timelineView.DebugWrite(); // TODO ★ 消す

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
            var spanModel = new MoveCardToCenterStackFromHandModel(
                    player: 0, // １プレイヤーが
                    place: right); // 右の
            var timeSpan = new TimeSpan(
                    startSeconds: this.gameModelBuffer.ElapsedSeconds,
                    spanModel: spanModel);
            this.timelineView.Simulator.Model.Add(new Spans.MoveCardToCenterStackFromHandView(
                timeSpan: timeSpan
                ));
            handled1player = true;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            // ２プレイヤーが、ピックアップ中の場札を抜いて、（１プレイヤーから見て）右の台札へ積み上げる
            var spanModel = new MoveCardToCenterStackFromHandModel(
                    player: 1, // ２プレイヤーが
                    place: right); // 右の
            var timeSpan = new TimeSpan(
                    startSeconds: this.gameModelBuffer.ElapsedSeconds,
                    spanModel: spanModel);
            this.timelineView.Simulator.Model.Add(new Spans.MoveCardToCenterStackFromHandView(
                timeSpan: timeSpan
                ));
            handled2player = true;
        }

        // （ボタン押下が同時なら）左の台札は２プレイヤー優先
        // ==================================================

        // ２プレイヤー
        if (Input.GetKeyDown(KeyCode.S))
        {
            // ２プレイヤーが、ピックアップ中の場札を抜いて、（１プレイヤーから見て）左の台札へ積み上げる
            var spanModel = new MoveCardToCenterStackFromHandModel(
                    player: 1, // ２プレイヤーが
                    place: left); // 左の
            var timeSpan = new TimeSpan(
                    startSeconds: this.gameModelBuffer.ElapsedSeconds,
                    spanModel: spanModel);
            this.timelineView.Simulator.Model.Add(new Spans.MoveCardToCenterStackFromHandView(
                timeSpan: timeSpan
                ));
            handled2player = true;
        }

        // １プレイヤー
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // １プレイヤーが、ピックアップ中の場札を抜いて、（１プレイヤーから見て）左の台札へ積み上げる
            var spanModel = new MoveCardToCenterStackFromHandModel(
                    player: 0, // １プレイヤーが
                    place: left); // 左の
            var timeSpan = new TimeSpan(
                    startSeconds: this.gameModelBuffer.ElapsedSeconds,
                    spanModel: spanModel);
            this.timelineView.Simulator.Model.Add(new Spans.MoveCardToCenterStackFromHandView(
                timeSpan: timeSpan
                ));
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
            var timeSpan = new TimeSpan(
                    startSeconds: this.gameModelBuffer.ElapsedSeconds,
                    spanModel);
            this.timelineView.Simulator.Model.Add(new Spans.MoveFocusToNextCardView(
                timeSpan: timeSpan
                ));
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
            var timeSpan = new TimeSpan(
                    startSeconds: this.gameModelBuffer.ElapsedSeconds,
                    spanModel: spanModel);
            this.timelineView.Simulator.Model.Add(new Spans.MoveFocusToNextCardView(
                timeSpan: timeSpan
                ));
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
            var timeSpan = new TimeSpan(
                    startSeconds: this.gameModelBuffer.ElapsedSeconds,
                    spanModel: spanModel);
            this.timelineView.Simulator.Model.Add(new Spans.MoveFocusToNextCardView(
                timeSpan: timeSpan
                ));
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
            var timeSpan = new TimeSpan(
                    startSeconds: this.gameModelBuffer.ElapsedSeconds,
                    spanModel: spanModel);
            this.timelineView.Simulator.Model.Add(new Spans.MoveFocusToNextCardView(
                timeSpan: timeSpan
                ));
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
                var timeSpan = new TimeSpan(
                        startSeconds: this.gameModelBuffer.ElapsedSeconds,
                        spanModel: spanModel);
                this.timelineView.Simulator.Model.Add(new Spans.MoveCardsToHandFromPileView(
                    timeSpan: timeSpan
                    )); ;
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
            this.timelineView.Simulator.AddScheduleSeconds(player: player, seconds: interval);
        }

        // 登録：ピックアップ場札を、台札へ積み上げる
        {
            {
                // １プレイヤーが、ピックアップ中の場札を抜いて、右の台札へ積み上げる
                var player = 0;
                var spanModel = new MoveCardToCenterStackFromHandModel(
                        player: player, // １プレイヤーが
                        place: right); // 右の
                var timeSpan = new TimeSpan(
                        startSeconds: this.timelineView.Simulator.ScheduledSeconds[player],
                        spanModel: spanModel);
                this.timelineView.Simulator.Model.Add(new Spans.MoveCardToCenterStackFromHandView(
                    timeSpan: timeSpan
                    ));
                this.timelineView.Simulator.AddScheduleSeconds(player, timeSpan.Duration);
            }
            {
                // ２プレイヤーが、ピックアップ中の場札を抜いて、左の台札へ積み上げる
                var player = 1;
                var spanModel = new MoveCardToCenterStackFromHandModel(
                        player: player, // ２プレイヤーが
                        place: left); // 左の;
                var timeSpan = new TimeSpan(
                        startSeconds: this.timelineView.Simulator.ScheduledSeconds[player],
                        spanModel: spanModel);
                this.timelineView.Simulator.Model.Add(new Spans.MoveCardToCenterStackFromHandView(
                    timeSpan: timeSpan
                    ));
                this.timelineView.Simulator.AddScheduleSeconds(player, timeSpan.Duration);
            }
        }

        // 間
        for (int player = 0; player < 2; player++)
        {
            this.timelineView.Simulator.AddScheduleSeconds(player: player, seconds: interval);
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
                    var timeSpan = new TimeSpan(
                            startSeconds: this.timelineView.Simulator.ScheduledSeconds[player],
                            spanModel: spanModel);
                    this.timelineView.Simulator.Model.Add(new Spans.MoveFocusToNextCardView(
                        timeSpan: timeSpan
                        ));
                    this.timelineView.Simulator.AddScheduleSeconds(player, timeSpan.Duration);
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
                    var timeSpan = new TimeSpan(
                            startSeconds: this.timelineView.Simulator.ScheduledSeconds[player],
                            spanModel: spanModel);
                    this.timelineView.Simulator.Model.Add(new Spans.MoveFocusToNextCardView(
                        timeSpan: timeSpan
                        ));
                    this.timelineView.Simulator.AddScheduleSeconds(player, timeSpan.Duration);
                }

                // 間
                for (int player = 0; player < 2; player++)
                {
                    this.timelineView.Simulator.AddScheduleSeconds(player: player, seconds: interval);
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
                var timeSpan = new TimeSpan(
                        startSeconds: this.timelineView.Simulator.ScheduledSeconds[player],
                        spanModel: spanModel);
                this.timelineView.Simulator.Model.Add(new Spans.MoveCardToCenterStackFromHandView(
                    timeSpan: timeSpan
                    ));
                this.timelineView.Simulator.AddScheduleSeconds(player, timeSpan.Duration);
            }
            {
                var player = 1;
                var spanModel = new MoveCardToCenterStackFromHandModel(
                        player: player, // ２プレイヤーが
                        place: 0); // 右の台札
                var timeSpan = new TimeSpan(
                        startSeconds: this.timelineView.Simulator.ScheduledSeconds[player],
                        spanModel: spanModel);
                this.timelineView.Simulator.Model.Add(new Spans.MoveCardToCenterStackFromHandView(
                    timeSpan: timeSpan
                    ));
                this.timelineView.Simulator.AddScheduleSeconds(player, timeSpan.Duration);
            }
        }

        // 間
        for (int player = 0; player < 2; player++)
        {
            this.timelineView.Simulator.AddScheduleSeconds(player: player, seconds: interval);
        }

        // 登録：手札から１枚引く
        {
            {
                // １プレイヤーは手札から１枚抜いて、場札として置く
                var player = 0;
                var spanModel = new MoveCardsToHandFromPileModel(
                        player: player,
                        numberOfCards: 1);
                var timeSpan = new TimeSpan(
                        startSeconds: this.timelineView.Simulator.ScheduledSeconds[player],
                        spanModel: spanModel);
                this.timelineView.Simulator.Model.Add(new Spans.MoveCardsToHandFromPileView(
                    timeSpan: timeSpan
                    ));
                this.timelineView.Simulator.AddScheduleSeconds(player, timeSpan.Duration);
            }
            {
                // ２プレイヤーは手札から１枚抜いて、場札として置く
                var player = 1;
                var spanModel = new MoveCardsToHandFromPileModel(
                        player: 1,
                        numberOfCards: 1);
                var timeSpan = new TimeSpan(
                        startSeconds: this.timelineView.Simulator.ScheduledSeconds[player],
                        spanModel: spanModel);
                this.timelineView.Simulator.Model.Add(new Spans.MoveCardsToHandFromPileView(
                    timeSpan: timeSpan
                    ));
                this.timelineView.Simulator.AddScheduleSeconds(player, timeSpan.Duration);
            }
        }
    }
}
