namespace Assets.Scripts.Vision.Models.Input
{
    using Assets.Scripts.ThinkingEngine;
    using ModelOfInputOfPlayer = Assets.Scripts.Vision.Models.Input.Players;

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
        internal Computer Computer { get; set; } = null;    // new Computer(0), new Computer(1)

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
        internal void Translate()
        {

        }
    }
}
