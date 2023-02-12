namespace Assets.Scripts.Vision.UserInterface
{
    using Assets.Scripts.ThinkingEngine;
    using UnityEngine;
    using VisionOfInput = Assets.Scripts.Vision.Input;

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

        VisionOfInput.Manager inputManager;

        // - ���\�b�h

        public void On1pVs2p()
        {
            inputManager.Computers = new Computer[] { null, null };
            playerSelectBackground.SetActive(false);
            playerButtons.SetActive(false);
            p1Keys.SetActive(true);
            p2Keys.SetActive(true);
        }

        public void On1pVsCom()
        {
            inputManager.Computers = new Computer[] { null, new Computer(1) };
            playerSelectBackground.SetActive(false);
            playerButtons.SetActive(false);
            p1Keys.SetActive(true);
        }

        public void OnComVs2p()
        {
            inputManager.Computers = new Computer[] { new Computer(0), null };
            playerSelectBackground.SetActive(false);
            playerButtons.SetActive(false);
            p2Keys.SetActive(true);
        }

        public void OnComVsCom()
        {
            inputManager.Computers = new Computer[] { new Computer(0), new Computer(1) };
            playerSelectBackground.SetActive(false);
            playerButtons.SetActive(false);
        }

        // - �C�x���g�n���h��

        // Start is called before the first frame update
        void Start()
        {
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
