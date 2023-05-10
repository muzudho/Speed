namespace Assets.Scripts.Vision.Behaviours
{
    using Assets.Scripts.ThinkingEngine;
    using UnityEngine;

    /// <summary>
    /// キャンバス・マネージャー
    /// </summary>
    public class CanvasManager : MonoBehaviour
    {
        // フィールド

        [SerializeField] GameObject playerSelectBackground;
        [SerializeField] GameObject playerButtons;
        [SerializeField] GameObject o1PKeys;
        [SerializeField] GameObject o2PKeys;
        [SerializeField] GameObject o1PWin;
        [SerializeField] GameObject o2PWin;
        [SerializeField] GameObject restartButton;

        GameManager gameManager;
        InputManager inputManager;
        SchedulerManager schedulerManager;

        // - メソッド

        /// <summary>
        /// 初期化
        /// </summary>
        public void Init()
        {
            // UI非表示
            o1PKeys.SetActive(false);
            o2PKeys.SetActive(false);
            o1PWin.SetActive(false);
            o2PWin.SetActive(false);
            restartButton.SetActive(false);
        }

        public void Won1P()
        {
            Debug.Log("1P win");
            o1PWin.SetActive(true);
            restartButton.SetActive(true);
        }

        public void Won2P()
        {
            Debug.Log("2P win");
            o2PWin.SetActive(true);
            restartButton.SetActive(true);
        }

        // - イベントハンドラ

        public void On1pVs2p()
        {
            // コンピューター設定
            inputManager.Model.Players[Commons.Player1.AsInt].Computer = null;
            inputManager.Model.Players[Commons.Player2.AsInt].Computer = null;

            // UI非表示
            playerSelectBackground.SetActive(false);
            playerButtons.SetActive(false);

            // UI表示
            o1PKeys.SetActive(true);
            o2PKeys.SetActive(true);

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
            o1PKeys.SetActive(true);

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
            o2PKeys.SetActive(true);

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

        public void OnRestart()
        {
            Debug.Log("Restart");

            // Start イベントが発生しない
            // SceneManager.LoadScene(SceneManager.GetActiveScene().name);

            // 初期化
            this.Init();
            this.gameManager.Init();
        }

        // - イベントハンドラ

        // Start is called before the first frame update
        void Start()
        {
            gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
            inputManager = GameObject.Find("Input Manager").GetComponent<InputManager>();
            schedulerManager = GameObject.Find("Scheduler Manager").GetComponent<SchedulerManager>();

            this.Init();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
