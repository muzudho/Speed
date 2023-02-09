using Assets.Scripts.Gui.SpanOfLerp;
using Assets.Scripts.Gui.SpanOfLerp.TimedGenerator;
using Assets.Scripts.ThinkingEngine;
using Assets.Scripts.ThinkingEngine.CommandArgs;
using Assets.Scripts.Views;
using Assets.Scripts.Views.Timeline;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SpanOfLeap = Assets.Scripts.Gui.SpanOfLerp;
using TimedGeneratorOfSpanOfLearp = Assets.Scripts.Gui.SpanOfLerp.TimedGenerator;
using ViewsOfTimeline = Assets.Scripts.Views.Timeline;
using GuiOfTimedCommandArgs = Assets.Scripts.Gui.TimedCommandArgs;

/// <summary>
/// ゲーム・マネージャー
/// 
/// - スピードは、日本と海外で　ルールとプレイング・スタイルに違いがあるので、用語に統一感はない
/// </summary>
public class GameManager : MonoBehaviour
{
    // - フィールド

    ViewsOfTimeline.PlayerToLerp playerToLerp;

    // ゲーム内単位時間
    float tickSeconds = 1.0f / 60.0f;

    // - プロパティ

    // モデル・バッファー
    GameModelBuffer modelBuffer = new GameModelBuffer();

    /// <summary>
    /// ゲーム・モデル
    /// </summary>
    internal GameModel Model
    {
        get
        {
            if (model == null)
            {
                // ゲーム・モデルは、ゲーム・モデル・バッファーを持つ
                model = new GameModel(modelBuffer);
            }
            return model;
        }
    }
    GameModel model;


    /// <summary>
    /// スケジュール・レジスター
    /// </summary>
    internal ScheduleRegister ScheduleRegister
    {
        get
        {
            if (scheduleRegister == null)
            {
                // スケジューラー・レジスターは、ゲーム・モデルを持つ。
                scheduleRegister = new TimedGeneratorOfSpanOfLearp.ScheduleRegister(this.Model);
            }
            return scheduleRegister;
        }
    }
    ScheduleRegister scheduleRegister;

    // - メソッド

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
                            direction: 0);
                    this.ScheduleRegister.AddWithinScheduler(player, spanModel);
                }

                // ２プレイヤーの右隣のカードへフォーカスを移します
                {
                    var player = 1;
                    var spanModel = new MoveFocusToNextCardModel(
                            player: player,
                            direction: 0);
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

    // - イベントハンドラ

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

        // ゲーム初期状態へセット
        {
            // ゲーム開始時、とりあえず、すべてのカードを集める
            List<IdOfPlayingCards> cardsOfGame = new();
            foreach (var idOfGo in GameObjectStorage.CreatePlayingCards().Keys)
            {
                cardsOfGame.Add(IdMapping.GetIdOfPlayingCard(idOfGo));
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

                modelBuffer.AddCardOfPlayersPile(player, idOfCard);

                // 画面上の位置も調整
                var goCard = GameObjectStorage.Items[IdMapping.GetIdOfGameObject(idOfCard)];

                var length = modelBuffer.IdOfCardsOfPlayersPile[player].Count;
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
                    var previousTopCard = modelBuffer.IdOfCardsOfPlayersPile[player][length - 2]; // 天辺より１つ下のカードが、前のカード
                    var goPreviousTopCard = GameObjectStorage.Items[IdMapping.GetIdOfGameObject(previousTopCard)];
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
        while (0 < model.GetLengthOfCenterStackCards(right))
        {
            // 即実行
            var timedCommandArg = new GuiOfTimedCommandArgs.Model(new MoveCardsToPileFromCenterStacksModel(
                    place: right
                    ));
            var timedGenerator = new TimedGeneratorOfSpanOfLearp.TimedGenerator(
                    startSeconds: 0.0f,
                    timedCommandArg: timedCommandArg,
                    spanGenerator: TimedGeneratorOfSpanOfLearp.Mapping.SpawnViewFromModel(timedCommandArg.GetType()));
            timedGenerator.SpanGenerator.CreateSpanToLerp(
                timedGenerator,
                modelBuffer,
                setSpanToLerp: (movementViewModel) => movementViewModel.Lerp(1.0f));
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
    }

    /// <summary>
    /// 一定間隔で呼び出される
    /// </summary>
    void OnTick()
    {
        // モデルからビューへ、起動したタイム・スパンを引き継ぎたい
        var additionSpansToLerp = new List<SpanOfLeap.Model>();

        // スケジュールを消化していきます
        ScheduleConverter.ConvertToSpansToLerp(
            this.ScheduleRegister,
            modelBuffer.ElapsedSeconds,
            modelBuffer,
            setSpanToLerp: (spanToLerp) =>
            {
                additionSpansToLerp.Add(spanToLerp);
            });

        // モーションの補間
        this.playerToLerp.Lerp(modelBuffer.ElapsedSeconds, additionSpansToLerp);

        this.ScheduleRegister.DebugWrite(); // TODO ★ 消す
        this.playerToLerp.DebugWrite(); // TODO ★ 消す

        modelBuffer.ElapsedSeconds += tickSeconds;
    }
}
