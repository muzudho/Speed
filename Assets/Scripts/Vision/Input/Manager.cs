namespace Assets.Scripts.Vision.Input
{
    using Assets.Scripts.ThinkingEngine;
    using Assets.Scripts.ThinkingEngine.Model;
    using Assets.Scripts.ThinkingEngine.Model.CommandArgs;
    using Assets.Scripts.Vision.World;
    using Assets.Scripts.Vision.World.SpanOfLerp.TimedGenerator;
    using UnityEngine;
    using GuiOfTimedCommandArgs = Assets.Scripts.Vision.World.TimedCommandArgs;
    using VisionOfInput = Assets.Scripts.Vision.Input;

    /// <summary>
    /// ���̓}�l�[�W���[
    /// </summary>
    public class Manager : MonoBehaviour
    {
        // - �t�B�[���h

        ScheduleRegister scheduleRegister;

        /// <summary>
        /// �R���s���[�^�[�E�v���C���[�p
        /// </summary>
        GameModel gameModel;

        float[] spamSeconds = new[] { 0f, 0f };

        /// <summary>
        /// �R���s���[�^�[�E�v���C���[���H
        /// 
        /// - �R���s���[�^�[�Ȃ� Computer �C���X�^���X
        /// - �R���s���[�^�[�łȂ���΃k��
        /// </summary>
        internal Computer[] Computers { get; set; } = new Computer[] { null, null, };
        // internal Computer[] Computers { get; set; } = new Computer[] { new Computer(0), new Computer(1), };

        VisionOfInput.ToMeaning inputToMeaning = new VisionOfInput.ToMeaning();

        // - �C�x���g�n���h��

        // Start is called before the first frame update
        void Start()
        {
            var gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
            scheduleRegister = gameManager.ScheduleRegister;
            gameModel = gameManager.Model;
        }

        /// <summary>
        /// Update is called once per frame
        /// 
        /// - ���͂́A�����Ɏ��s�́A���܂���
        /// - ���͂́A�R�}���h�ɕϊ����āA�^�C�����C���֓o�^���܂�
        /// </summary>
        void Update()
        {
            // �L�[���͂̉�́F�N���A�[
            inputToMeaning.Clear();

            // �������͂ł��Ȃ��Ȃ�^
            bool[] handled = { false, false };

            for (var player = 0; player < 2; player++)
            {
                // �O����F�������͂ł��Ȃ��Ȃ�^
                //
                // - �X�p����
                // - �΋ǒ�~��
                handled[player] = 0 < spamSeconds[player] || !gameModel.IsGameActive;

                if (!handled[player])
                {
                    if (Computers[player] == null)
                    {
                        // �L�[���͂̉�́F�l�Ԃ̓��͂���t
                        inputToMeaning.UpdateFromInput(player);
                    }
                    else
                    {
                        // �R���s���[�^�[�E�v���C���[���v�l���āA��������߂�
                        Computers[player].Think(gameModel);

                        // �L�[���͂̉�́F�R���s���[�^�[����̓��͂���t
                        inputToMeaning.Overwrite(
                            player: player,
                            moveCardToCenterStackNearMe: Computers[player].MoveCardToCenterStackNearMe,
                            moveCardToFarCenterStack: Computers[player].MoveCardToFarCenterStack,
                            pickupCardToForward: Computers[player].PickupCardToForward,
                            pickupCardToBackward: Computers[player].PickupCardToBackward,
                            drawing: Computers[player].Drawing);
                    }
                }

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

            // - �P�v���C���[
            // - �����̋߂����̑�D�֒u��
            {
                var player = 0;
                if (!handled[player] && inputToMeaning.MoveCardToCenterStackNearMe[player] && LegalMove.CanPutToCenterStack(
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

            // - �Q�v���C���[
            // - �������牓�����̑�D�֒u��
            {
                var player = 1;
                if (!handled[player] && inputToMeaning.MoveCardToFarCenterStack[player] && LegalMove.CanPutToCenterStack(
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

            // - �Q�v���C���[
            // - �����̋߂����̑�D�֒u��
            {
                var player = 1;
                if (!handled[player] && inputToMeaning.MoveCardToCenterStackNearMe[player] && LegalMove.CanPutToCenterStack(
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

            // - �P�v���C���[
            // - �������牓�����̑�D�֒u��
            {
                var player = 0;
                if (!handled[player] && inputToMeaning.MoveCardToFarCenterStack[player] && LegalMove.CanPutToCenterStack(
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
                // �s���F
                //      �P�v���C���[�̃s�b�N�A�b�v���Ă���J�[�h���猩�āA�i�P�v���C���[���猩�āj
                //      ���ׂ̃J�[�h���s�b�N�A�b�v����悤�ɕς��܂�
                else if (inputToMeaning.PickupCardToBackward[player])
                {
                    // ����F
                    //      ��D���Q���ȏ゠��Ƃ��Ɍ���
                    if (2 <= this.gameModel.GetCardsOfPlayerHand(player).Count)
                    {
                        var timedCommandArg = new GuiOfTimedCommandArgs.Model(new MoveFocusToNextCardModel(
                            player: player,
                            direction: 1));

                        spamSeconds[player] = timedCommandArg.Duration;
                        scheduleRegister.AddJustNow(timedCommandArg);
                    }
                }
                // �s���F
                //      �P�v���C���[�̃s�b�N�A�b�v���Ă���J�[�h���猩�āA�i�P�v���C���[���猩�āj
                //      �E�ׂ̃J�[�h���s�b�N�A�b�v����悤�ɕς��܂�
                else if (inputToMeaning.PickupCardToForward[player])
                {
                    // ����F
                    //      ��D���Q���ȏ゠��Ƃ��Ɍ���
                    if (2 <= this.gameModel.GetCardsOfPlayerHand(player).Count)
                    {
                        var timedCommandArg = new GuiOfTimedCommandArgs.Model(new MoveFocusToNextCardModel(
                            player: player,
                            direction: 0));

                        spamSeconds[player] = timedCommandArg.Duration;
                        scheduleRegister.AddJustNow(timedCommandArg);
                    }
                }
            }

            // �Q�v���C���[
            {
                var player = 1;

                if (handled[player])
                {

                }
                // �s���F
                //      �Q�v���C���[�̃s�b�N�A�b�v���Ă���J�[�h���猩�āA�i�Q�v���C���[���猩�āj
                //      ���ׂ̃J�[�h���s�b�N�A�b�v����悤�ɕς��܂�
                else if (inputToMeaning.PickupCardToBackward[player])
                {
                    // ����F
                    //      ��D���Q���ȏ゠��Ƃ��Ɍ���
                    if (2 <= this.gameModel.GetCardsOfPlayerHand(player).Count)
                    {
                        var timedCommandArg = new GuiOfTimedCommandArgs.Model(new MoveFocusToNextCardModel(
                            player: player,
                            direction: 1));

                        spamSeconds[player] = timedCommandArg.Duration;
                        scheduleRegister.AddJustNow(timedCommandArg);
                    }
                }
                // �s���F
                //      �Q�v���C���[�̃s�b�N�A�b�v���Ă���J�[�h���猩�āA�i�Q�v���C���[���猩�āj
                //      �E�ׂ̃J�[�h���s�b�N�A�b�v����悤�ɕς��܂�
                else if (inputToMeaning.PickupCardToForward[player])
                {
                    // ����F
                    //      ��D���Q���ȏ゠��Ƃ��Ɍ���
                    if (2 <= this.gameModel.GetCardsOfPlayerHand(player).Count)
                    {
                        var timedCommandArg = new GuiOfTimedCommandArgs.Model(new MoveFocusToNextCardModel(
                            player: player,
                            direction: 0));

                        spamSeconds[player] = timedCommandArg.Duration;
                        scheduleRegister.AddJustNow(timedCommandArg);
                    }
                }
            }

            // �f�o�b�O�p
            if (inputToMeaning.Drawing)
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
}