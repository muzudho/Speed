namespace Assets.Scripts.Scheduler.AnalogCommands.O4thComplex
{
    using Assets.Scripts.Coding;
    using Assets.Scripts.ThinkingEngine.Models;
    using Assets.Scripts.Vision.Models;
    using ModelOfAnalogCommand1stTimelineSpan = Assets.Scripts.Scheduler.AnalogCommands.O1stTimelineSpan;
    using ModelOfAnalogCommand3rdSimplex = Assets.Scripts.Scheduler.AnalogCommands.O3rdSimplex;
    using ModelOfAnalogCommands = Assets.Scripts.Scheduler.AnalogCommands;
    using ModelOfDigitalCommands = Assets.Scripts.ThinkingEngine.DigitalCommands;
    using ModelOfGameBuffer = Assets.Scripts.ThinkingEngine.Models.Game.Buffer;
    using ModelOfGameWriter = Assets.Scripts.ThinkingEngine.Models.Game.Writer;
    using ModelOfInput = Assets.Scripts.Vision.Models.Input;

    /// <summary>
    /// ｎプレイヤーの手札から場札へ、ｍ枚のカードを移動
    /// </summary>
    class MoveCardsToHandFromPile : ItsAbstract
    {
        // - その他

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="startObj"></param>
        /// <param name="digitalCommand"></param>
        public MoveCardsToHandFromPile(
            GameSeconds startObj,
            ModelOfDigitalCommands.IModel digitalCommand)
            : base(startObj, digitalCommand)
        {
        }

        // - メソッド

        /// <summary>
        /// ゲーム画面の同期を始めます
        /// 
        /// - 手札の上の方からｎ枚抜いて、場札の後ろへ追加する
        /// - 画面上の場札は位置調整される
        /// </summary>
        public override void GenerateSpan(
            ModelOfGameBuffer.Model gameModelBuffer,
            ModelOfGameWriter.Model gameModelWriter,
            ModelOfInput.Init inputModel,
            ModelOfAnalogCommands.Model schedulerModel,
            LazyArgs.SetValue<ModelOfAnalogCommand1stTimelineSpan.IModel> setTimespan)
        {
            var digitalCommand = (ModelOfDigitalCommands.MoveCardsToHandFromPile)this.DigitalCommand;
            var playerObj = digitalCommand.PlayerObj;

            // 確定：手札の枚数
            var length = gameModelBuffer.GetPlayer(digitalCommand.PlayerObj).IdOfCardsOfPile.Count;

            // 手札がないのに、手札を引こうとしたとき
            if (length < digitalCommand.NumberOfCards)
            {
                // TODO ★ なぜここにくる？
                // できない指示は無視
                // Debug.Log("[MoveCardsToHandFromPileView OnEnter] できない指示は無視");

                // 制約の解除
                inputModel.Players[playerObj.AsInt].Rights.IsPileCardDrawing = false;
                return;
            }

            // モデル更新：場札への移動
            // ========================
            gameModelWriter.GetPlayer(playerObj).MoveCardsToHandFromPile(
                startIndexObj: new PlayerPileCardIndex(length - digitalCommand.NumberOfCards),
                numberOfCards: digitalCommand.NumberOfCards);
            // 場札は１枚以上になる

            // モデル更新：もし、ピックアップ場札がなかったら、先頭の場札をピックアップする
            // ============================================================================
            //
            // - 初回配布のケース
            // - 場札無しの勝利後に配ったケース
            if (gameModelBuffer.GetPlayer(playerObj).FocusedHandCardObj.Index == HandCardIndex.Empty)
            {
                gameModelWriter.GetPlayer(playerObj).UpdateFocusedHandCardObj(FocusedHandCard.PickupFirst);
            }

            // 確定：場札の枚数
            int numberOfCards = gameModelWriter.GetPlayer(playerObj).GetLengthOfHandCards();

            // ビュー：場札の位置の再調整（をしないと、手札から移動しない）
            if (0 < numberOfCards)
            {
                var timespanList = ModelOfAnalogCommand3rdSimplex.ArrangeHandCards.CreateTimespanList(
                    timeRange: this.TimeRangeObj,
                    playerObj: playerObj,
                    indexOfPickupObj: gameModelWriter.GetPlayer(playerObj).GetFocusedHandCardObj().Index,
                    idOfHandCards: gameModelWriter.GetPlayer(playerObj).GetCardsOfHand(),
                    keepPickup: true,
                    onProgressOrNull: (progress) =>
                    {
                        if (1.0f <= progress)
                        {
                            // 制約の解除
                            inputModel.Players[playerObj.AsInt].Rights.IsPileCardDrawing = false;
                        }
                    });

                foreach (var timespan in timespanList)
                {
                    setTimespan(timespan);
                }
            }
            else
            {
                // 制約の解除
                inputModel.Players[playerObj.AsInt].Rights.IsPileCardDrawing = false;
            }
        }
    }
}

