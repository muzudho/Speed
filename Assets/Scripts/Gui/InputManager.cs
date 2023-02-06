using Assets.Scripts.Gui.SpanOfLerp.TimedGenerator;
using Assets.Scripts.ThinkingEngine;
using Assets.Scripts.ThinkingEngine.CommandArgs;
using UnityEngine;
using GuiOfTimedCommandArgs = Assets.Scripts.Gui.TimedCommandArgs;

public class InputManager : MonoBehaviour
{
    // - �t�B�[���h

    ScheduleRegister scheduleRegister;

    float[] spamSeconds = new[] { 0f, 0f };

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
        // �������͂ł��Ȃ��Ȃ�^
        bool[] handled = { false, false };

        for (var player = 0; player < 2; player++)
        {
            // �O����
            // �������͂ł��Ȃ��Ȃ�^
            handled[player] = 0 < spamSeconds[player];

            // �X�p�����ԏ���
            if (0 < spamSeconds[player])
            {
                // �����ɂȂ��Ă��C�ɂ��Ȃ�
                spamSeconds[player] -= Time.deltaTime;
            }
        }

        const int right = 0;// ��D�̉E
        const int left = 1;// ��D�̍�

        // ��ɓo�^�����R�}���h�̕����������s�����

        // �i�{�^�������������Ȃ�j�E�̑�D�͂P�v���C���[�D��
        // ==================================================

        // �P�v���C���[
        {
            var player = 0;
            if (!handled[player] && Input.GetKeyDown(KeyCode.DownArrow) && LegalMove.CanPutToCenterStack(
                gameModel: scheduleRegister.GameModel,
                player: player,
                place: right))  // �E��
            {
                // �P�v���C���[���A�s�b�N�A�b�v���̏�D�𔲂��āA�i�P�v���C���[���猩�āj�E�̑�D�֐ςݏグ��
                var timedCommandArg = new GuiOfTimedCommandArgs.Model(new MoveCardToCenterStackFromHandModel(
                    player: player,      // �P�v���C���[��
                    place: right)); // �E��

                spamSeconds[player] = timedCommandArg.Duration;
                scheduleRegister.AddJustNow(timedCommandArg);
                handled[player] = true;
            }
        }

        // �Q�v���C���[
        {
            var player = 1;
            if (!handled[player] && Input.GetKeyDown(KeyCode.W) && LegalMove.CanPutToCenterStack(
                gameModel: scheduleRegister.GameModel,
                player: player,
                place: right))  // �E��)
            {
                // �Q�v���C���[���A�s�b�N�A�b�v���̏�D�𔲂��āA�i�P�v���C���[���猩�āj�E�̑�D�֐ςݏグ��
                var timedCommandArg = new GuiOfTimedCommandArgs.Model(new MoveCardToCenterStackFromHandModel(
                    player: player,      // �Q�v���C���[��
                    place: right)); // �E��

                spamSeconds[player] = timedCommandArg.Duration;
                scheduleRegister.AddJustNow(timedCommandArg);
                handled[player] = true;
            }
        }

        // �i�{�^�������������Ȃ�j���̑�D�͂Q�v���C���[�D��
        // ==================================================

        // �Q�v���C���[
        {
            var player = 1;
            if (!handled[player] && Input.GetKeyDown(KeyCode.S) && LegalMove.CanPutToCenterStack(
                gameModel: scheduleRegister.GameModel,
                player: player,
                place: left))
            {
                // �Q�v���C���[���A�s�b�N�A�b�v���̏�D�𔲂��āA�i�P�v���C���[���猩�āj���̑�D�֐ςݏグ��
                var timedCommandArg = new GuiOfTimedCommandArgs.Model(new MoveCardToCenterStackFromHandModel(
                    player: player,      // �Q�v���C���[��
                    place: left));  // ����

                spamSeconds[player] = timedCommandArg.Duration;
                scheduleRegister.AddJustNow(timedCommandArg);
                handled[player] = true;
            }
        }

        // �P�v���C���[
        {
            var player = 0;
            if (!handled[player] && Input.GetKeyDown(KeyCode.UpArrow) && LegalMove.CanPutToCenterStack(
                gameModel: scheduleRegister.GameModel,
                player: player,
                place: left))
            {
                // �P�v���C���[���A�s�b�N�A�b�v���̏�D�𔲂��āA�i�P�v���C���[���猩�āj���̑�D�֐ςݏグ��
                var timedCommandArg = new GuiOfTimedCommandArgs.Model(new MoveCardToCenterStackFromHandModel(
                    player: player,      // �P�v���C���[��
                    place: left));  // ����

                spamSeconds[player] = timedCommandArg.Duration;
                scheduleRegister.AddJustNow(timedCommandArg);
                handled[player] = true;
            }
        }

        // ����ȊO�̃L�[���͂́A�����ł����s�Ɋ֌W���Ȃ�
        // ==============================================

        // �P�v���C���[
        {
            var player = 0;

            if (handled[player])
            {

            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                // �P�v���C���[�̃s�b�N�A�b�v���Ă���J�[�h���猩�āA�i�P�v���C���[���猩�āj���ׂ̃J�[�h���s�b�N�A�b�v����悤�ɕς��܂�
                var timedCommandArg = new GuiOfTimedCommandArgs.Model(new MoveFocusToNextCardModel(
                    player: player,
                    direction: 1));

                spamSeconds[player] = timedCommandArg.Duration;
                scheduleRegister.AddJustNow(timedCommandArg);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                // �P�v���C���[�̃s�b�N�A�b�v���Ă���J�[�h���猩�āA�i�P�v���C���[���猩�āj�E�ׂ̃J�[�h���s�b�N�A�b�v����悤�ɕς��܂�
                var timedCommandArg = new GuiOfTimedCommandArgs.Model(new MoveFocusToNextCardModel(
                    player: player,
                    direction: 0));

                spamSeconds[player] = timedCommandArg.Duration;
                scheduleRegister.AddJustNow(timedCommandArg);
            }
        }

        // �Q�v���C���[
        {
            var player = 1;

            if (handled[player])
            {

            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                // �Q�v���C���[�̃s�b�N�A�b�v���Ă���J�[�h���猩�āA�i�Q�v���C���[���猩�āj���ׂ̃J�[�h���s�b�N�A�b�v����悤�ɕς��܂�
                var timedCommandArg = new GuiOfTimedCommandArgs.Model(new MoveFocusToNextCardModel(
                    player: player,
                    direction: 1));

                spamSeconds[player] = timedCommandArg.Duration;
                scheduleRegister.AddJustNow(timedCommandArg);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                // �Q�v���C���[�̃s�b�N�A�b�v���Ă���J�[�h���猩�āA�i�Q�v���C���[���猩�āj�E�ׂ̃J�[�h���s�b�N�A�b�v����悤�ɕς��܂�
                var timedCommandArg = new GuiOfTimedCommandArgs.Model(new MoveFocusToNextCardModel(
                    player: player,
                    direction: 0));

                spamSeconds[player] = timedCommandArg.Duration;
                scheduleRegister.AddJustNow(timedCommandArg);
            }
        }

        // �f�o�b�O�p
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // ���v���C���[�͎�D����P�������āA��D�Ƃ��Ēu��
            for (var player = 0; player < 2; player++)
            {
                // ��D����ׂ�
                var timedCommandArg = new GuiOfTimedCommandArgs.Model(new MoveCardsToHandFromPileModel(
                    player: player,
                    numberOfCards: 1));

                spamSeconds[player] = timedCommandArg.Duration;
                scheduleRegister.AddJustNow(timedCommandArg);
            }
        }
    }
}
