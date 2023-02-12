namespace Assets.Scripts.Vision.UserInterface
{
    using Assets.Scripts.ThinkingEngine;
    using UnityEngine;
    using VisionOfInput = Assets.Scripts.Vision.Input;
    using VisionOfWorld = Assets.Scripts.Vision.World;

    /// <summary>
    /// ���[�U�[�E�C���^�[�t�F�[�X�E�}�l�[�W���[
    /// </summary>
    public class Manager : MonoBehaviour
    {
        // �t�B�[���h

        [SerializeField] GameObject playerSelectBackground;
        [SerializeField] GameObject playerButtons;
        [SerializeField] GameObject p1Keys;
        [SerializeField] GameObject p2Keys;

        VisionOfWorld.GameManager gameManager;
        VisionOfInput.Manager inputManager;

        // - ���\�b�h

        public void On1pVs2p()
        {
            inputManager.Computers = new Computer[] { null, null };
            playerSelectBackground.SetActive(false);
            playerButtons.SetActive(false);
            p1Keys.SetActive(true);
            p2Keys.SetActive(true);
            gameManager.StartGame();
        }

        public void On1pVsCom()
        {
            inputManager.Computers = new Computer[] { null, new Computer(1) };
            playerSelectBackground.SetActive(false);
            playerButtons.SetActive(false);
            p1Keys.SetActive(true);
            gameManager.StartGame();
        }

        public void OnComVs2p()
        {
            inputManager.Computers = new Computer[] { new Computer(0), null };
            playerSelectBackground.SetActive(false);
            playerButtons.SetActive(false);
            p2Keys.SetActive(true);
            gameManager.StartGame();
        }

        public void OnComVsCom()
        {
            inputManager.Computers = new Computer[] { new Computer(0), new Computer(1) };
            playerSelectBackground.SetActive(false);
            playerButtons.SetActive(false);
            gameManager.StartGame();
        }

        // - �C�x���g�n���h��

        // Start is called before the first frame update
        void Start()
        {
            gameManager = GameObject.Find("Game Manager").GetComponent<VisionOfWorld.GameManager>();
            inputManager = GameObject.Find("Input Manager").GetComponent<VisionOfInput.Manager>();

            p1Keys.SetActive(false);
            p2Keys.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
