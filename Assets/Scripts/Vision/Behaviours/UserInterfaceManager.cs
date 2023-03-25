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
            // コンピューター設定
            inputManager.Model.Players[Commons.Player1.AsInt].Computer = null;
            inputManager.Model.Players[Commons.Player2.AsInt].Computer = null;

            // UI非表示
            playerSelectBackground.SetActive(false);
            playerButtons.SetActive(false);

            // UI表示
            p1Keys.SetActive(true);
            p2Keys.SetActive(true);

            // 対局開始
            schedulerManager.StartGame();
        }

        public void On1pVsCom()
        {
            // コンピューター設定
            inputManager.Model.Players[Commons.Player1.AsInt].Computer = null;
            inputManager.Model.Players[Commons.Player2.AsInt].Computer = new Computer(Commons.Player2.AsInt);

            // UI非表示
            playerSelectBackground.SetActive(false);
            playerButtons.SetActive(false);

            // UI表示
            p1Keys.SetActive(true);

            // 対局開始
            schedulerManager.StartGame();
        }

        public void OnComVs2p()
        {
            // コンピューター設定
            inputManager.Model.Players[Commons.Player1.AsInt].Computer = new Computer(Commons.Player1.AsInt);
            inputManager.Model.Players[Commons.Player2.AsInt].Computer = null;

            // UI非表示
            playerSelectBackground.SetActive(false);
            playerButtons.SetActive(false);

            // UI表示
            p2Keys.SetActive(true);

            // 対局開始
            schedulerManager.StartGame();
        }

        public void OnComVsCom()
        {
            // コンピューター設定
            inputManager.Model.Players[Commons.Player1.AsInt].Computer = new Computer(Commons.Player1.AsInt);
            inputManager.Model.Players[Commons.Player2.AsInt].Computer = new Computer(Commons.Player2.AsInt);

            // UI非表示
            playerSelectBackground.SetActive(false);
            playerButtons.SetActive(false);

            // 対局開始
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
