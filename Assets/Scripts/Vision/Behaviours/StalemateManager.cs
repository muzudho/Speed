namespace Assets.Scripts.Vision.Behaviours
{
    using Assets.Scripts.ThinkingEngine;
    using Assets.Scripts.ThinkingEngine.Models;
    using System.Collections;
    using TMPro;
    using UnityEngine;
    using ModelOfObservableGame = Assets.Scripts.ThinkingEngine.Models.Game.Observable;
    using ModelOfScheduler = Assets.Scripts.Vision.Models.Scheduler;
    using ModelOfThinkingEngineCommand = Assets.Scripts.ThinkingEngine.Models.Commands;

    /// <summary>
    /// 両プレイヤーが置けるカードがなくなってしまったとき、
    /// カウントダウンを開始して、
    /// ピックアップ中の場札を　強制的に　台札へ置かせます
    /// </summary>
    internal class StalemateManager : MonoBehaviour
    {
        // - その他

        /// <summary>
        /// 初期化
        /// </summary>
        internal void Init(ModelOfScheduler.Model schedulerModel)
        {
            this.SchedulerModel = schedulerModel;
        }

        // - フィールド

        /// <summary>
        /// スケジューラー・モデル
        /// </summary>
        ModelOfScheduler.Model SchedulerModel { get; set; }

        /// <summary>
        /// ゲーム・モデル
        /// </summary>
        ModelOfObservableGame.Model gameModel;

        /// <summary>
        /// キャンバス・マネージャー
        /// </summary>
        CanvasManager canvasManager;

        // - プロパティ

        /// <summary>
        /// ステールメートしているか？
        /// </summary>
        internal bool IsStalemate { get; private set; }

        // - メソッド

        /// <summary>
        /// ステールメートしているか確認します
        /// </summary>
        internal void CheckStalemate(ModelOfObservableGame.Model gameModel)
        {
            this.gameModel = gameModel;

            if (gameModel.IsGameActive &&  // 対局が開始しており
                !this.IsStalemate)              // まだ、ステールメートしていないとき
            {
                bool isStalemateTemp = true;
                // 反例を探す
                foreach (var playerObj in Commons.Players)
                {
                    foreach (var centerStackPlace in Commons.CenterStacks)
                    {
                        var max = gameModel.GetPlayer(playerObj).GetCardsOfHand().Count;
                        for (int i = 0; i < max; i++)
                        {
                            if (LegalMove.CanPutCardToCenterStack(
                                gameModel,
                                playerObj,
                                new HandCardIndex(i),
                                centerStackPlace))
                            {
                                isStalemateTemp = false;
                                goto end_loop;
                            }
                        }
                    }
                }
            end_loop:

                // 反例がなければ、ステールメート
                if (isStalemateTemp)
                {
                    this.IsStalemate = true;

                    // TODO ★ カウントダウン・タイマーを表示。０になったら、ピックアップ中の場札を強制的に台札へ置く
                    this.Reopening();
                }
            }
        }

        /// <summary>
        /// 両プレイヤーが置けるカードがなくなってしまったとき、
        /// カウントダウンを開始して、
        /// ピックアップ中の場札を　強制的に　台札へ置かせます
        /// </summary>
        void Reopening()
        {
            StartCoroutine(WorkingOfReopening());
        }

        // - コルーチン

        /// <summary>
        /// コルーチン
        /// 
        /// - 再開
        /// </summary>
        IEnumerator WorkingOfReopening()
        {
            // カウントダウン
            // ==============
            Debug.Log("再開 3");
            this.canvasManager.SetVisibleOfCountDownText(true);
            this.canvasManager.SetCountDownText("3");
            yield return new WaitForSeconds(1f);

            Debug.Log("再開 2");
            this.canvasManager.SetCountDownText("2");
            yield return new WaitForSeconds(1f);

            Debug.Log("再開 1");
            this.canvasManager.SetCountDownText("1");
            yield return new WaitForSeconds(1f);

            Debug.Log("強制 カード発射");
            this.canvasManager.SetCountDownText("");
            this.canvasManager.SetVisibleOfCountDownText(false);

            {
                // １プレイヤーが
                var playerObj = Commons.Player1;
                // 右の台札へ
                var placeObj = Commons.RightCenterStack;

                //
                // （ステールメート時は）ブーメラン判定しません。強制的にカードを置きます
                //
                var digitalCommand = new ModelOfThinkingEngineCommand.MoveCardToCenterStackFromHand(
                playerObj: playerObj,
                placeObj: placeObj);

                // １プレイヤーが、ピックアップ中の場札を抜いて、（１プレイヤーから見て）右の台札へ積み上げる
                this.SchedulerModel.Timeline.AddCommand(
                    startObj: this.gameModel.ElapsedSeconds,
                    command: digitalCommand);
            }
            {
                // ２プレイヤーが
                var playerObj = Commons.Player2;
                // 左の台札へ
                var placeObj = Commons.LeftCenterStack;

                //
                // （ステールメート時は）ブーメラン判定しません。強制的にカードを置きます
                //
                // ２プレイヤーが、ピックアップ中の場札を抜いて、（１プレイヤーから見て）左の台札へ積み上げる
                var digitalCommand = new ModelOfThinkingEngineCommand.MoveCardToCenterStackFromHand(
                    playerObj: playerObj,
                    placeObj: placeObj);

                this.SchedulerModel.Timeline.AddCommand(
                    startObj: this.gameModel.ElapsedSeconds,
                    command: digitalCommand);
            }
            yield return new WaitForSeconds(1f);

            Debug.Log("ステールメート解除");
            this.IsStalemate = false;
            yield return null;
        }

        // - イベントハンドラ

        // Start is called before the first frame update
        void Start()
        {
            canvasManager = GameObject.Find("Canvas Manager").GetComponent<CanvasManager>();
        }
    }
}
