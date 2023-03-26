using Assets.Scripts.Vision.Behaviours;
using Assets.Scripts.Vision.Models;
using Assets.Scripts.Vision.Models.Replays;
using System.Collections.Generic;
using UnityEngine;
using ModelOfGame = Assets.Scripts.ThinkingEngine.Models.Game;
using ModelOfGameBuffer = Assets.Scripts.ThinkingEngine.Models.GameBuffer;
using ModelOfInput = Assets.Scripts.Vision.Models.Input;
using ModelOfScheduler = Assets.Scripts.Vision.Models.Scheduler;
using ModelOfSchedulerO1stTimelineSpan = Assets.Scripts.Vision.Models.Scheduler.O1stTimelineSpan;

/// <summary>
/// �^�C�����C���E�}�l�[�W���[
/// </summary>
public class SchedulerManager : MonoBehaviour
{
    // - �t�B�[���h

    // �Q�[�����P�ʎ���
    GameSeconds gameTimeObj = new GameSeconds(1.0f / 60.0f);

    /// <summary>
    /// ���́E���f��
    /// </summary>
    internal ModelOfInput.Init InputModel { get; private set; }

    /// <summary>
    /// �X�P�W���[���[�E���f��
    /// </summary>
    internal ModelOfScheduler.Model Model { get; private set; }

    /// <summary>
    /// �R���s���[�^�[�E�v���C���[�p
    /// </summary>
    ModelOfGame.Default gameModel;

    /// <summary>
    /// �Q�[���E���f���E�o�b�t�@�[
    /// </summary>
    ModelOfGameBuffer.Model gameModelBuffer;

    // - ���\�b�h

    /// <summary>
    /// �΋ǊJ�n
    /// </summary>
    internal void StartGame()
    {
        // �J�n�ǖʂ܂œo�^
        SetStartPosition.DoIt(
            gameModelBuffer,
            this.InputModel,
            this.Model);

        // �ȉ��A�f���E�v���C��o�^
        // SetupDemo(this.Timeline);

        // �yOnTick �� ���b��ɌĂяo���A�ȍ~�� tickSeconds �b���Ɏ��s�z
        InvokeRepeating(
            methodName: nameof(OnTick),
            time: this.Model.Timeline.LastSeconds(),
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
            gameModelBuffer,
            this.InputModel,
            this.Model,
            setTimespan: (spanToLerp) =>
            {
                additionSpansToLerp.Add(spanToLerp);
            });

        // ���[�V�����̕��
        this.Model.Add(additionSpansToLerp);
        this.Model.Update(gameModelBuffer.ElapsedTimeObj);

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

        var inputManager = GameObject.Find("Input Manager").GetComponent<InputManager>();
        this.InputModel = inputManager.Model;

        // �X�P�W���[���[�E���f��
        this.Model = new ModelOfScheduler.Model(this.gameModel);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
