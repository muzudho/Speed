namespace Assets.Scripts.Vision.Behaviours
{
    using Assets.Scripts.ThinkingEngine;
    using Assets.Scripts.ThinkingEngine.Models;
    using Assets.Scripts.ThinkingEngine.Models.CommandArgs;
    using System.Collections;
    using TMPro;
    using UnityEngine;
    using ModelOfGame = Assets.Scripts.ThinkingEngine.Models.Game;
    using ModelOfTimelineO1stElement = Assets.Scripts.Vision.Models.Timeline.O1stElements;
    using ModelOfTimelineO2ndTimedCommandArgs = Assets.Scripts.Vision.Models.Timeline.O2ndTimedCommandArgs;

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
        internal void Init(ModelOfTimelineO1stElement.ScheduleRegister scheduleRegister)
        {
            this.scheduleRegister = scheduleRegister;
        }

        // - フィールド

        ModelOfTimelineO1stElement.ScheduleRegister scheduleRegister;

        TMP_Text countDownText;

        // - プロパティ

        /// <summary>
        /// ステールメートしているか？
        /// </summary>
        internal bool IsStalemate { get; private set; }

        // - メソッド

        /// <summary>
        /// ステールメートしているか確認します
        /// </summary>
        internal void CheckStalemate(ModelOfGame.Default gameModel)
        {
            if (gameModel.IsGameActive &&  // 対局が開始しており
                !this.IsStalemate)              // まだ、ステールメートしていないとき
            {
                bool isStalemateTemp = true;
                // 反例を探す
                foreach (var playerObj in Commons.Players)
                {
                    foreach (var centerStackPlace in Commons.CenterStacks)
                    {
                        var max = gameModel.GetCardsOfPlayerHand(playerObj).Count;
                        for (int i = 0; i < max; i++)
                        {
                            if (LegalMove.CanPutToCenterStack(
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
        internal void Reopening()
        {
            StartCoroutine(WorkingOfReopening());
        }

        // - コルーチン

        /// <summary>
        /// コルーチン
        /// </summary>
        internal IEnumerator WorkingOfReopening()
        {
            // カウントダウン
            // ==============
            Debug.Log("再開 3");
            this.countDownText.text = "3";
            yield return new WaitForSeconds(1f);

            Debug.Log("再開 2");
            this.countDownText.text = "2";
            yield return new WaitForSeconds(1f);

            Debug.Log("再開 1");
            this.countDownText.text = "1";
            yield return new WaitForSeconds(1f);

            Debug.Log("強制 カード発射");
            this.countDownText.text = "";
            {
                // １プレイヤーが、ピックアップ中の場札を抜いて、（１プレイヤーから見て）右の台札へ積み上げる
                var timedCommandArg = new ModelOfTimelineO2ndTimedCommandArgs.Model(new MoveCardToCenterStackFromHandModel(
                    playerObj: Commons.Player1,      // １プレイヤーが
                    placeObj: Commons.RightCenterStack)); // 右の

                this.scheduleRegister.AddJustNow(timedCommandArg);
            }
            {
                // ２プレイヤーが、ピックアップ中の場札を抜いて、（１プレイヤーから見て）左の台札へ積み上げる
                var timedCommandArg = new ModelOfTimelineO2ndTimedCommandArgs.Model(new MoveCardToCenterStackFromHandModel(
                    playerObj: Commons.Player2,      // ２プレイヤーが
                    placeObj: Commons.LeftCenterStack)); // 左の

                this.scheduleRegister.AddJustNow(timedCommandArg);
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
            this.countDownText = GameObject.Find("Count Down Text").GetComponent<TMP_Text>();
            this.countDownText.text = "";
        }
    }
}
