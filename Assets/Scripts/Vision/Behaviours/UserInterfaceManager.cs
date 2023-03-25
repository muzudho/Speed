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
        SchedulerManager schedulerManager;

        // - メソッド

        public void On1pVs2p()
        {
            inputManager.Model.Players[Commons.Player1.AsInt].Computer = null;
            inputManager.Model.Players[Commons.Player2.AsInt].Computer = null;
            playerSelectBackground.SetActive(false);
            playerButtons.SetActive(false);
            p1Keys.SetActive(true);
            p2Keys.SetActive(true);
            schedulerManager.StartGame();
        }

        public void On1pVsCom()
        {
            inputManager.Model.Players[Commons.Player1.AsInt].Computer = null;
            inputManager.Model.Players[Commons.Player2.AsInt].Computer = new Computer(Commons.Player2.AsInt);
            playerSelectBackground.SetActive(false);
            playerButtons.SetActive(false);
            p1Keys.SetActive(true);
            schedulerManager.StartGame();
        }

        public void OnComVs2p()
        {
            inputManager.Model.Players[Commons.Player1.AsInt].Computer = new Computer(Commons.Player1.AsInt);
            inputManager.Model.Players[Commons.Player2.AsInt].Computer = null;
            playerSelectBackground.SetActive(false);
            playerButtons.SetActive(false);
            p2Keys.SetActive(true);
            schedulerManager.StartGame();
        }

        public void OnComVsCom()
        {
            inputManager.Model.Players[Commons.Player1.AsInt].Computer = new Computer(Commons.Player1.AsInt);
            inputManager.Model.Players[Commons.Player2.AsInt].Computer = new Computer(Commons.Player2.AsInt);
            playerSelectBackground.SetActive(false);
            playerButtons.SetActive(false);
            schedulerManager.StartGame();
        }

        // - イベントハンドラ

        // Start is called before the first frame update
        void Start()
        {
            inputManager = GameObject.Find("Input Manager").GetComponent<InputManager>();
            schedulerManager = GameObject.Find("Scheduler Manager").GetComponent<SchedulerManager>();

            p1Keys.SetActive(false);
            p2Keys.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
