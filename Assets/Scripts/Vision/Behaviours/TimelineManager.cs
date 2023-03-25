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
/// �^�C�����C���E�}�l�[�W���[
/// </summary>
public class TimelineManager : MonoBehaviour
{
    // - �t�B�[���h

    // �Q�[�����P�ʎ���
    GameSeconds gameTimeObj = new GameSeconds(1.0f / 60.0f);

    /// <summary>
    /// �X�P�W���[���[
    /// </summary>
    ModelOfScheduler.Model scheduler;

    /// <summary>
    /// �R���s���[�^�[�E�v���C���[�p
    /// </summary>
    ModelOfGame.Default gameModel;

    /// <summary>
    /// �Q�[���E���f���E�o�b�t�@�[
    /// </summary>
    GameModelBuffer gameModelBuffer;

    // - �v���p�e�B

    ModelOfSchedulerO7thTimeline.Model timeline;

    /// <summary>
    /// �^�C�����C��
    /// </summary>
    internal ModelOfSchedulerO7thTimeline.Model Timeline
    {
        get
        {
            if (timeline == null)
            {
                // �^�C�����C���́A�Q�[���E���f�������B
                timeline = new ModelOfSchedulerO7thTimeline.Model(this.gameModel);
            }
            return timeline;
        }
    }

    // - ���\�b�h

    /// <summary>
    /// �΋ǊJ�n
    /// </summary>
    internal void StartGame()
    {
        // �J�n�ǖʂ܂œo�^
        SetStartPosition.DoIt(
            gameModelBuffer,
            this.Timeline);

        // �ȉ��A�f���E�v���C��o�^
        // SetupDemo(this.Timeline);

        // �yOnTick �� ���b��ɌĂяo���A�ȍ~�� tickSeconds �b���Ɏ��s�z
        InvokeRepeating(
            methodName: nameof(OnTick),
            time: this.Timeline.LastSeconds(),
            repeatRate: gameTimeObj.AsFloat);
    }

    /// <summary>
    /// ���Ԋu�ŌĂяo�����
    /// </summary>
    void OnTick()
    {
        // ���f������r���[�ցA�N�������^�C���E�X�p���������p������
        var additionSpansToLerp = new List<ModelOfSchedulerO1stTimelineSpan.IModel>();

        // �X�P�W���[�����������Ă����܂�
        ModelOfScheduler.Helper.ConvertToSpans(
            this.Timeline,
            gameModelBuffer.ElapsedTimeObj,
            gameModelBuffer,
            setTimelineSpan: (spanToLerp) =>
            {
                additionSpansToLerp.Add(spanToLerp);
            });

        // ���[�V�����̕��
        this.scheduler.Add(additionSpansToLerp);
        this.scheduler.DrawThisMoment(gameModelBuffer.ElapsedTimeObj);

        //this.Timeline.DebugWrite();
        //this.playerToLerp.DebugWrite();

        gameModelBuffer.ElapsedTimeObj = new GameSeconds(gameModelBuffer.ElapsedTimeObj.AsFloat + gameTimeObj.AsFloat);
    }

    // - �C�x���g�n���h��

    // Start is called before the first frame update
    void Start()
    {
        var gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        this.gameModel = gameManager.Model;
        this.gameModelBuffer = gameManager.ModelBuffer;

        // Lerp �����s���邾���̃N���X
        this.scheduler = new ModelOfScheduler.Model();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
