using Assets.Scripts.Gui.SpanOfLerp.TimedGenerator;
using Assets.Scripts.ThinkingEngine.CommandArgs;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // - �t�B�[���h

    ScheduleRegister scheduleRegister;

    // - �C�x���g�n���h��

    // Start is called before the first frame update
    void Start()
    {
        scheduleRegister = GameObject.Find("Game Manager").GetComponent<GameManager>().ScheduleRegister;
    }

    /// <summary>
    /// Update is called once per frame
    /// 
    /// - ���͂́A�����Ɏ��s�́A���܂���
    /// - ���͂́A�R�}���h�ɕϊ����āA�^�C�����C���֓o�^���܂�
    /// </summary>
    void Update()
    {
        const int right = 0;// ��D�̉E
        const int left = 1;// ��D�̍�
        bool handled1player = false;
        bool handled2player = false;

        // ��ɓo�^�����R�}���h�̕����������s�����

        // �i�{�^�������������Ȃ�j�E�̑�D�͂P�v���C���[�D��
        // ==================================================

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // �P�v���C���[���A�s�b�N�A�b�v���̏�D�𔲂��āA�i�P�v���C���[���猩�āj�E�̑�D�֐ςݏグ��
            scheduleRegister.AddJustNow(new MoveCardToCenterStackFromHandModel(
                player: 0,      // �P�v���C���[��
                place: right)); // �E��
            handled1player = true;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            // �Q�v���C���[���A�s�b�N�A�b�v���̏�D�𔲂��āA�i�P�v���C���[���猩�āj�E�̑�D�֐ςݏグ��
            scheduleRegister.AddJustNow(new MoveCardToCenterStackFromHandModel(
                player: 1, // �Q�v���C���[��
                place: right)); // �E��
            handled2player = true;
        }

        // �i�{�^�������������Ȃ�j���̑�D�͂Q�v���C���[�D��
        // ==================================================

        // �Q�v���C���[
        if (Input.GetKeyDown(KeyCode.S))
        {
            // �Q�v���C���[���A�s�b�N�A�b�v���̏�D�𔲂��āA�i�P�v���C���[���猩�āj���̑�D�֐ςݏグ��
            scheduleRegister.AddJustNow(new MoveCardToCenterStackFromHandModel(
                player: 1,      // �Q�v���C���[��
                place: left));  // ����
            handled2player = true;
        }

        // �P�v���C���[
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // �P�v���C���[���A�s�b�N�A�b�v���̏�D�𔲂��āA�i�P�v���C���[���猩�āj���̑�D�֐ςݏグ��
            scheduleRegister.AddJustNow(new MoveCardToCenterStackFromHandModel(
                player: 0, // �P�v���C���[��
                place: left));  // ����
            handled1player = true;
        }

        // ����ȊO�̃L�[���͂́A�����ł����s�Ɋ֌W���Ȃ�
        // ==============================================

        // �P�v���C���[
        if (handled1player)
        {

        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // �P�v���C���[�̃s�b�N�A�b�v���Ă���J�[�h���猩�āA�i�P�v���C���[���猩�āj���ׂ̃J�[�h���s�b�N�A�b�v����悤�ɕς��܂�
            scheduleRegister.AddJustNow(new MoveFocusToNextCardModel(
                player: 0,
                direction: 1));
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // �P�v���C���[�̃s�b�N�A�b�v���Ă���J�[�h���猩�āA�i�P�v���C���[���猩�āj�E�ׂ̃J�[�h���s�b�N�A�b�v����悤�ɕς��܂�
            scheduleRegister.AddJustNow(new MoveFocusToNextCardModel(
                player: 0,
                direction: 0));
        }

        // �Q�v���C���[
        if (handled2player)
        {

        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            // �Q�v���C���[�̃s�b�N�A�b�v���Ă���J�[�h���猩�āA�i�Q�v���C���[���猩�āj���ׂ̃J�[�h���s�b�N�A�b�v����悤�ɕς��܂�
            scheduleRegister.AddJustNow(new MoveFocusToNextCardModel(
                player: 1,
                direction: 1));
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            // �Q�v���C���[�̃s�b�N�A�b�v���Ă���J�[�h���猩�āA�i�Q�v���C���[���猩�āj�E�ׂ̃J�[�h���s�b�N�A�b�v����悤�ɕς��܂�
            scheduleRegister.AddJustNow(new MoveFocusToNextCardModel(
                player: 1,
                direction: 0));
        }

        // �f�o�b�O�p
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // ���v���C���[�͎�D����P�������āA��D�Ƃ��Ēu��
            for (var player = 0; player < 2; player++)
            {
                // ��D����ׂ�
                scheduleRegister.AddJustNow(new MoveCardsToHandFromPileModel(
                    player: player,
                    numberOfCards: 1));
            }
        }
    }
}
