namespace Assets.Scripts.Vision.Behaviours
{
    using Assets.Scripts.ThinkingEngine;
    using UnityEngine;

    /// <summary>
    /// ユーザー・インターフェース・マネージャー
    /// </summary>
    public class UserInterfaceManager : MonoBehaviour
    {
        // フィールド

        [SerializeField] GameObject playerSelectBackground;
        [SerializeField] GameObject playerButtons;
        [SerializeField] GameObject p1Keys;
        [SerializeField] GameObject p2Keys;

        GameManager gameManager;
        InputManager inputManager;

        // - メソッド

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

        // - イベントハンドラ

        // Start is called before the first frame update
        void Start()
        {
            gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
            inputManager = GameObject.Find("Input Manager").GetComponent<InputManager>();

            p1Keys.SetActive(false);
            p2Keys.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
