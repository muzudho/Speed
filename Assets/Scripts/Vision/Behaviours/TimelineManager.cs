using Assets.Scripts.Vision.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.ThinkingEngine;
using Assets.Scripts.ThinkingEngine.Models;
using Assets.Scripts.Vision.Models.Replays;
using Assets.Scripts.Vision.Models.World;
using System;
using System.Linq;
using ModelOfGame = Assets.Scripts.ThinkingEngine.Models.Game;
using ModelOfScheduler = Assets.Scripts.Vision.Models.Scheduler;
using ModelOfSchedulerO1stTimelineSpan = Assets.Scripts.Vision.Models.Scheduler.O1stTimelineSpan;
using ModelOfSchedulerO7thTimeline = Assets.Scripts.Vision.Models.Scheduler.O7thTimeline;
using Assets.Scripts.Vision.Behaviours;

/// <summary>
/// タイムライン・マネージャー
/// </summary>
public class TimelineManager : MonoBehaviour
{
    // - フィールド

    // ゲーム内単位時間
    GameSeconds gameTimeObj = new GameSeconds(1.0f / 60.0f);

    /// <summary>
    /// スケジューラー
    /// </summary>
    ModelOfScheduler.Model scheduler;

    /// <summary>
    /// コンピューター・プレイヤー用
    /// </summary>
    ModelOfGame.Default gameModel;

    /// <summary>
    /// ゲーム・モデル・バッファー
    /// </summary>
    GameModelBuffer gameModelBuffer;

    // - プロパティ

    ModelOfSchedulerO7thTimeline.Model timeline;

    /// <summary>
    /// タイムライン
    /// </summary>
    internal ModelOfSchedulerO7thTimeline.Model Timeline
    {
        get
        {
            if (timeline == null)
            {
                // タイムラインは、ゲーム・モデルを持つ。
                timeline = new ModelOfSchedulerO7thTimeline.Model(this.gameModel);
            }
            return timeline;
        }
    }

    // - メソッド

    /// <summary>
    /// 対局開始
    /// </summary>
    internal void StartGame()
    {
        // 開始局面まで登録
        SetStartPosition.DoIt(
            gameModelBuffer,
            this.Timeline);

        // 以下、デモ・プレイを登録
        // SetupDemo(this.Timeline);

        // 【OnTick を ○秒後に呼び出し、以降は tickSeconds 秒毎に実行】
        InvokeRepeating(
            methodName: nameof(OnTick),
            time: this.Timeline.LastSeconds(),
            repeatRate: gameTimeObj.AsFloat);
    }

    /// <summary>
    /// 一定間隔で呼び出される
    /// </summary>
    void OnTick()
    {
        // モデルからビューへ、起動したタイム・スパンを引き継ぎたい
        var additionSpansToLerp = new List<ModelOfSchedulerO1stTimelineSpan.IModel>();

        // スケジュールを消化していきます
        ModelOfScheduler.Helper.ConvertToSpans(
            this.Timeline,
            gameModelBuffer.ElapsedTimeObj,
            gameModelBuffer,
            setTimelineSpan: (spanToLerp) =>
            {
                additionSpansToLerp.Add(spanToLerp);
            });

        // モーションの補間
        this.scheduler.Add(additionSpansToLerp);
        this.scheduler.DrawThisMoment(gameModelBuffer.ElapsedTimeObj);

        //this.Timeline.DebugWrite();
        //this.playerToLerp.DebugWrite();

        gameModelBuffer.ElapsedTimeObj = new GameSeconds(gameModelBuffer.ElapsedTimeObj.AsFloat + gameTimeObj.AsFloat);
    }

    // - イベントハンドラ

    // Start is called before the first frame update
    void Start()
    {
        var gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        this.gameModel = gameManager.Model;
        this.gameModelBuffer = gameManager.ModelBuffer;

        // Lerp を実行するだけのクラス
        this.scheduler = new ModelOfScheduler.Model();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
