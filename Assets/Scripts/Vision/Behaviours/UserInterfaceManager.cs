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

        InputManager inputManager;
        TimelineManager timelineManager;

        // - メソッド

        public void On1pVs2p()
        {
            inputManager.InputOfPlayers[Commons.Player1.AsInt].Computer = null;
            inputManager.InputOfPlayers[Commons.Player2.AsInt].Computer = null;
            playerSelectBackground.SetActive(false);
            playerButtons.SetActive(false);
            p1Keys.SetActive(true);
            p2Keys.SetActive(true);
            timelineManager.StartGame();
        }

        public void On1pVsCom()
        {
            inputManager.InputOfPlayers[Commons.Player1.AsInt].Computer = null;
            inputManager.InputOfPlayers[Commons.Player2.AsInt].Computer = new Computer(Commons.Player2.AsInt);
            playerSelectBackground.SetActive(false);
            playerButtons.SetActive(false);
            p1Keys.SetActive(true);
            timelineManager.StartGame();
        }

        public void OnComVs2p()
        {
            inputManager.InputOfPlayers[Commons.Player1.AsInt].Computer = new Computer(Commons.Player1.AsInt);
            inputManager.InputOfPlayers[Commons.Player2.AsInt].Computer = null;
            playerSelectBackground.SetActive(false);
            playerButtons.SetActive(false);
            p2Keys.SetActive(true);
            timelineManager.StartGame();
        }

        public void OnComVsCom()
        {
            inputManager.InputOfPlayers[Commons.Player1.AsInt].Computer = new Computer(Commons.Player1.AsInt);
            inputManager.InputOfPlayers[Commons.Player2.AsInt].Computer = new Computer(Commons.Player2.AsInt);
            playerSelectBackground.SetActive(false);
            playerButtons.SetActive(false);
            timelineManager.StartGame();
        }

        // - イベントハンドラ

        // Start is called before the first frame update
        void Start()
        {
            inputManager = GameObject.Find("Input Manager").GetComponent<InputManager>();
            timelineManager = GameObject.Find("Timeline Manager").GetComponent<TimelineManager>();

            p1Keys.SetActive(false);
            p2Keys.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
