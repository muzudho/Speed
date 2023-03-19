namespace Assets.Scripts.Vision.Behaviours
{
    using Assets.Scripts.ThinkingEngine.Models;
    using ModelOfGame = Assets.Scripts.ThinkingEngine.Models.Game;
    using Assets.Scripts.Vision.World.Replays;
    using Assets.Scripts.Vision.World.SpanOfLerp;
    using Assets.Scripts.Vision.World.SpanOfLerp.TimedGenerator;
    using Assets.Scripts.Vision.World.Views;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using SpanOfLeap = Assets.Scripts.Vision.World.SpanOfLerp;
    using TimedGeneratorOfSpanOfLearp = Assets.Scripts.Vision.World.SpanOfLerp.TimedGenerator;
    using ViewsOfTimeline = Assets.Scripts.Vision.Timeline;
    using Assets.Scripts.ThinkingEngine;

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
        internal ModelOfGame.Default Model
        {
            get
            {
                if (model == null)
                {
                    // ゲーム・モデルは、ゲーム・モデル・バッファーを持つ
                    model = new ModelOfGame.Default(modelBuffer);
                }
                return model;
            }
        }
        ModelOfGame.Default model;


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
        /// 対局開始
        /// </summary>
        internal void StartGame()
        {
            // 開始局面まで登録
            SetStartPosition.DoIt(
                modelBuffer,
                this.ScheduleRegister);

            // 以下、デモ・プレイを登録
            // SetupDemo(this.ScheduleRegister);

            // 【OnTick を ○秒後に呼び出し、以降は tickSeconds 秒毎に実行】
            InvokeRepeating(
                methodName: nameof(OnTick),
                time: this.ScheduleRegister.LastSeconds(),
                repeatRate: tickSeconds);
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
            playerToLerp = new ViewsOfTimeline.PlayerToLerp();

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
                    Player playerObj;
                    switch (idOfCard.Suit())
                    {
                        case IdOfCardSuits.Clubs:
                        case IdOfCardSuits.Spades:
                            playerObj = Commons.Player1;
                            break;

                        case IdOfCardSuits.Diamonds:
                        case IdOfCardSuits.Hearts:
                            playerObj = Commons.Player2;
                            break;

                        default:
                            throw new Exception();
                    }

                    modelBuffer.AddCardOfPlayersPile(playerObj, idOfCard);

                    // 画面上の位置も調整
                    var goCard = GameObjectStorage.Items[IdMapping.GetIdOfGameObject(idOfCard)];

                    var length = modelBuffer.IdOfCardsOfPlayersPile[playerObj.AsInt].Count;
                    // 最初の１枚目
                    if (length == 1)
                    {
                        var position = Vision.Commons.positionOfPileCardsOrigin[playerObj.AsInt];
                        goCard.transform.position = position.ToMutable();
                        // 裏返す
                        goCard.transform.rotation = Quaternion.Euler(
                            x: goCard.transform.rotation.x,
                            y: goCard.transform.rotation.y,
                            z: 180.0f);
                    }
                    else
                    {
                        var previousTopCard = modelBuffer.IdOfCardsOfPlayersPile[playerObj.AsInt][length - 2]; // 天辺より１つ下のカードが、前のカード
                        var goPreviousTopCard = GameObjectStorage.Items[IdMapping.GetIdOfGameObject(previousTopCard)];
                        goCard.transform.position = Vision.Commons.yOfCardThickness.Add(goPreviousTopCard.transform.position); // 下のカードの上に被せる
                                                                                                                         // 裏返す
                        goCard.transform.rotation = Quaternion.Euler(
                            x: goCard.transform.rotation.x,
                            y: goCard.transform.rotation.y,
                            z: 180.0f);
                    }
                }
            }

            // UI ボタンを押下して、ゲーム開始
            // this.StartGame();
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

            //this.ScheduleRegister.DebugWrite();
            //this.playerToLerp.DebugWrite();

            modelBuffer.ElapsedSeconds += tickSeconds;
        }
    }
}
