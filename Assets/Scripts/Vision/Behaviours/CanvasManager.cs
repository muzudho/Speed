namespace Assets.Scripts.Vision.Behaviours
{
    using Assets.Scripts.ThinkingEngine;
    using TMPro;
    using UnityEngine;

    /// <summary>
    /// キャンバス・マネージャー
    /// </summary>
    public class CanvasManager : MonoBehaviour
    {
        // - フィールド

        [SerializeField] GameObject playerSelectBackground;
        [SerializeField] GameObject playerButtons;
        [SerializeField] GameObject o1PKeys;
        [SerializeField] GameObject o2PKeys;
        [SerializeField] GameObject o1PWin;
        [SerializeField] GameObject o2PWin;
        [SerializeField] GameObject restartButton;
        [SerializeField] GameObject draw1;
        [SerializeField] GameObject draw2;
        [SerializeField] GameObject countDownText;

        GameManager gameManager;
        InputManager inputManager;
        SchedulerManager schedulerManager;

        /// <summary>
        /// カウントダウン・テキスト
        /// </summary>
        TMP_Text countDownTextTMP;

        // - その他

        #region その他（初期化）
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
            draw1.SetActive(false);
            draw2.SetActive(false);
            countDownText.SetActive(false);

            // UI表示
            playerSelectBackground.SetActive(true);
            playerButtons.SetActive(true);
        }
        #endregion

        // - メソッド

        /// <summary>
        /// カウントダウン・テキストの文字設定
        /// </summary>
        /// <param name="text"></param>
        public void SetCountDownText(string text)
        {
            this.countDownTextTMP.text = text;
        }

        /// <summary>
        /// カウントダウン・テキストの非表示
        /// </summary>
        public void SetVisibleOfCountDownText(bool visible)
        {
            this.countDownText.SetActive(visible);
        }

        public void OnStalemate()
        {
            Debug.Log("Stalemate");
            countDownText.SetActive(true);
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

        public void Draw()
        {
            Debug.Log("Draw");
            draw1.SetActive(true);
            draw2.SetActive(true);
            restartButton.SetActive(true);
        }

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
            this.schedulerManager.CleanUp();
            this.inputManager.CleanUp();
        }

        // - イベントハンドラ

        // Start is called before the first frame update
        void Start()
        {
            gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
            inputManager = GameObject.Find("Input Manager").GetComponent<InputManager>();
            schedulerManager = GameObject.Find("Scheduler Manager").GetComponent<SchedulerManager>();

            this.countDownTextTMP = this.countDownText.GetComponent<TMP_Text>();
            this.countDownTextTMP.text = "";

            this.Init();
        }
    }
}
