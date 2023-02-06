using Assets.Scripts.ThinkingEngine.CommandArgs;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
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
            var player = 0;
            var spanModel = new MoveCardToCenterStackFromHandModel(
                    player: player, // �P�v���C���[��
                    place: right); // �E��
            gameManager.ScheduleRegister.AddJustNow(
                gameManager.ElapsedSeconds,
                spanModel);
            handled1player = true;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            // �Q�v���C���[���A�s�b�N�A�b�v���̏�D�𔲂��āA�i�P�v���C���[���猩�āj�E�̑�D�֐ςݏグ��
            var player = 1;
            var spanModel = new MoveCardToCenterStackFromHandModel(
                    player: player, // �Q�v���C���[��
                    place: right); // �E��
            gameManager.ScheduleRegister.AddJustNow(
                gameManager.ElapsedSeconds,
                spanModel);
            handled2player = true;
        }

        // �i�{�^�������������Ȃ�j���̑�D�͂Q�v���C���[�D��
        // ==================================================

        // �Q�v���C���[
        if (Input.GetKeyDown(KeyCode.S))
        {
            // �Q�v���C���[���A�s�b�N�A�b�v���̏�D�𔲂��āA�i�P�v���C���[���猩�āj���̑�D�֐ςݏグ��
            var player = 1;
            var spanModel = new MoveCardToCenterStackFromHandModel(
                    player: player, // �Q�v���C���[��
                    place: left); // ����
            gameManager.ScheduleRegister.AddJustNow(
                gameManager.ElapsedSeconds,
                spanModel);
            handled2player = true;
        }

        // �P�v���C���[
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // �P�v���C���[���A�s�b�N�A�b�v���̏�D�𔲂��āA�i�P�v���C���[���猩�āj���̑�D�֐ςݏグ��
            var player = 0;
            var spanModel = new MoveCardToCenterStackFromHandModel(
                    player: player, // �P�v���C���[��
                    place: left); // ����
            gameManager.ScheduleRegister.AddJustNow(
                gameManager.ElapsedSeconds,
                spanModel);
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
            var player = 0;
            var spanModel = new MoveFocusToNextCardModel(
                    player: player,
                    direction: 1);
            gameManager.ScheduleRegister.AddJustNow(
                gameManager.ElapsedSeconds,
                spanModel);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // �P�v���C���[�̃s�b�N�A�b�v���Ă���J�[�h���猩�āA�i�P�v���C���[���猩�āj�E�ׂ̃J�[�h���s�b�N�A�b�v����悤�ɕς��܂�
            var player = 0;
            var spanModel = new MoveFocusToNextCardModel(
                    player: player,
                    direction: 0);
            gameManager.ScheduleRegister.AddJustNow(
                gameManager.ElapsedSeconds,
                spanModel);
        }

        // �Q�v���C���[
        if (handled2player)
        {

        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            // �Q�v���C���[�̃s�b�N�A�b�v���Ă���J�[�h���猩�āA�i�Q�v���C���[���猩�āj���ׂ̃J�[�h���s�b�N�A�b�v����悤�ɕς��܂�
            var player = 1;
            var spanModel = new MoveFocusToNextCardModel(
                    player: player,
                    direction: 1);
            gameManager.ScheduleRegister.AddJustNow(
                gameManager.ElapsedSeconds,
                spanModel);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            // �Q�v���C���[�̃s�b�N�A�b�v���Ă���J�[�h���猩�āA�i�Q�v���C���[���猩�āj�E�ׂ̃J�[�h���s�b�N�A�b�v����悤�ɕς��܂�
            var player = 1;
            var spanModel = new MoveFocusToNextCardModel(
                    player: player,
                    direction: 0);
            gameManager.ScheduleRegister.AddJustNow(
                gameManager.ElapsedSeconds,
                spanModel);
        }

        // �f�o�b�O�p
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // ���v���C���[�͎�D����P�������āA��D�Ƃ��Ēu��
            for (var player = 0; player < 2; player++)
            {
                // ��D����ׂ�
                var spanModel = new MoveCardsToHandFromPileModel(
                        player: player,
                        numberOfCards: 1);
                gameManager.ScheduleRegister.AddJustNow(
                    gameManager.ElapsedSeconds,
                    spanModel);
            }
        }
    }
}
