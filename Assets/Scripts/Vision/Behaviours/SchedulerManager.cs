﻿using Assets.Scripts.Vision.Behaviours;
using Assets.Scripts.Vision.Models;
using Assets.Scripts.Vision.Models.Replays;
using System.Collections.Generic;
using UnityEngine;
using ModelOfObservableGame = Assets.Scripts.ThinkingEngine.Models.Game.Observable;
using ModelOfGameBuffer = Assets.Scripts.ThinkingEngine.Models.GameBuffer;
using ModelOfInput = Assets.Scripts.Vision.Models.Input;
using ModelOfScheduler = Assets.Scripts.Vision.Models.Scheduler;
using ModelOfSchedulerO1stTimelineSpan = Assets.Scripts.Vision.Models.Scheduler.O1stTimelineSpan;

/// <summary>
/// タイムライン・マネージャー
/// </summary>
public class SchedulerManager : MonoBehaviour
{
    // - フィールド

    // ゲーム内単位時間
    GameSeconds gameTimeObj = new GameSeconds(1.0f / 60.0f);

    /// <summary>
    /// 入力・モデル
    /// </summary>
    internal ModelOfInput.Init InputModel { get; private set; }

    /// <summary>
    /// スケジューラー・モデル
    /// </summary>
    internal ModelOfScheduler.Model Model { get; private set; }

    /// <summary>
    /// コンピューター・プレイヤー用
    /// </summary>
    ModelOfObservableGame.Model gameModel;

    /// <summary>
    /// ゲーム・モデル・バッファー
    /// </summary>
    ModelOfGameBuffer.Model gameModelBuffer;

    // - メソッド

    #region メソッド（対局開始）
    /// <summary>
    /// 対局開始
    /// </summary>
    internal void StartGame()
    {
        // 開始局面まで登録
        SetStartPosition.DoIt(
            gameModelBuffer,
            this.InputModel,
            this.Model);

        // 以下、デモ・プレイを登録
        // SetupDemo(this.Timeline);

        // 【OnTick を ○秒後に呼び出し、以降は tickSeconds 秒毎に実行】
        InvokeRepeating(
            methodName: nameof(OnTick),
            time: this.Model.Timeline.LastSeconds(),
            repeatRate: gameTimeObj.AsFloat);
    }
    #endregion

    #region メソッド（一定間隔で呼び出される）
    /// <summary>
    /// 一定間隔で呼び出される
    /// </summary>
    void OnTick()
    {
        // モデルからビューへ、起動したタイム・スパンを引き継ぎたい
        var additionSpansToLerp = new List<ModelOfSchedulerO1stTimelineSpan.IModel>();

        // スケジュールを消化していきます
        ModelOfScheduler.Helper.ConvertToSpans(
            gameModelBuffer,
            this.InputModel,
            this.Model,
            setTimespan: (spanToLerp) =>
            {
                additionSpansToLerp.Add(spanToLerp);
            });

        // モーションの補間
        this.Model.Add(additionSpansToLerp);
        this.Model.Update(gameModelBuffer.ElapsedTimeObj);

        //this.Timeline.DebugWrite();
        //this.playerToLerp.DebugWrite();

        gameModelBuffer.ElapsedTimeObj = new GameSeconds(gameModelBuffer.ElapsedTimeObj.AsFloat + gameTimeObj.AsFloat);
    }
    #endregion

    /// <summary>
    /// 初期化
    /// </summary>
    internal void CleanUp()
    {
        this.Model.CleanUp();
    }

    // - イベントハンドラ

    // Start is called before the first frame update
    void Start()
    {
        var gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        this.gameModel = gameManager.Model;
        this.gameModelBuffer = gameManager.ModelBuffer;

        var inputManager = GameObject.Find("Input Manager").GetComponent<InputManager>();
        this.InputModel = inputManager.Model;

        // スケジューラー・モデル
        this.Model = new ModelOfScheduler.Model(this.gameModel);

        // 初期化
        this.CleanUp();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
