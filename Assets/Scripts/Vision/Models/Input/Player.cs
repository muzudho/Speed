namespace Assets.Scripts.Vision.Models.Input
{
    using UnityEngine;
    using ModelOfGame = Assets.Scripts.ThinkingEngine.Models.Game;
    using ModelOfInputOfPlayer = Assets.Scripts.Vision.Models.Input.Players;
    using ModelOfThinkingEngine = Assets.Scripts.ThinkingEngine.Models;
    using ScriptOfThinkingEngine = Assets.Scripts.ThinkingEngine;

    /// <summary>
    /// プレイヤーの入力
    /// 
    /// - 編集可
    /// </summary>
    internal class Player
    {
        // - 生成

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="meaning"></param>
        internal Player(ModelOfInputOfPlayer.Meaning meaning)
        {
            this.Meaning = meaning;
        }

        // - プロパティ

        /// <summary>
        /// Id
        /// </summary>
        internal ModelOfThinkingEngine.Player PlayerObj { get; private set; }

        /// <summary>
        /// もう入力できないなら真
        /// 
        /// - 編集可
        /// </summary>
        internal bool Handled { get; set; }

        /// <summary>
        /// 入力の権利
        /// </summary>
        internal Rights Rights { get; private set; } = new Rights();

        /// <summary>
        /// コンピューター・プレイヤーか？
        /// 
        /// - 編集可
        /// - コンピューターなら Computer インスタンス
        /// - コンピューターでなければヌル
        /// </summary>
        internal ScriptOfThinkingEngine.Computer Computer { get; set; } = null;    // new Computer(0), new Computer(1)

        /// <summary>
        /// 入力の意味
        /// 
        /// - プレイヤー別
        /// </summary>
        internal ModelOfInputOfPlayer.Meaning Meaning { get; private set; }

        // - メソッド

        /// <summary>
        /// 入力を翻訳
        /// </summary>
        internal void Translate(
            ModelOfGame.Default gameModel)
        {
            // キー入力の解析：クリアー
            this.Meaning.Clear();

            // 前判定：もう入力できないなら真
            //
            // - スパム中
            // - 対局停止中
            this.Handled = 0.0f < this.Rights.TimeOfRestObj.AsFloat || !gameModel.IsGameActive;

            if (!this.Handled)
            {
                if (this.Computer == null)
                {
                    // キー入力の解析：人間の入力を受付
                    this.Meaning.UpdateFromInput();
                }
                else
                {
                    // コンピューター・プレイヤーが思考して、操作を決める
                    this.Computer.Think(gameModel);

                    // キー入力の解析：コンピューターからの入力を受付
                    this.Meaning.Overwrite(
                        playerObj: this.PlayerObj,
                        moveCardToCenterStackNearMe: this.Computer.MoveCardToCenterStackNearMe,
                        moveCardToFarCenterStack: this.Computer.MoveCardToFarCenterStack,
                        pickupCardToForward: this.Computer.PickupCardToForward,
                        pickupCardToBackward: this.Computer.PickupCardToBackward,
                        drawing: this.Computer.Drawing);
                }
            }

            // スパン時間消化
            if (0.0f < this.Rights.TimeOfRestObj.AsFloat)
            {
                // 負数になっても気にしない
                this.Rights.TimeOfRestObj = new GameSeconds(this.Rights.TimeOfRestObj.AsFloat - Time.deltaTime);
            }
        }
    }
}
