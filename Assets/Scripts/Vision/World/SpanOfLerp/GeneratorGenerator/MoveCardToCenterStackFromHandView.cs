namespace Assets.Scripts.Vision.World.SpanOfLerp.GeneratorGenerator
{
    using Assets.Scripts.Coding;
    using Assets.Scripts.ThinkingEngine.Model;
    using Assets.Scripts.ThinkingEngine.Model.CommandArgs;
    using Assets.Scripts.Vision.World.SpanOfLerp.Generator;
    using SimulatorsOfTimeline = Assets.Scripts.Vision.World.SpanOfLerp.TimedGenerator;
    using SpanOfLeap = Assets.Scripts.Vision.World.SpanOfLerp;

    /// <summary>
    /// ｎプレイヤーがピックアップしている場札を、右（または左）の台札へ移動する
    /// </summary>
    class MoveCardToCenterStackFromHandView : AbstractSpanGenerator
    {
        // - 生成

        /// <summary>
        /// 生成
        /// </summary>
        /// <returns></returns>
        public override ISpanGenerator Spawn()
        {
            return new MoveCardToCenterStackFromHandView();
        }

        // - プロパティ

        MoveCardToCenterStackFromHandModel GetModel(SimulatorsOfTimeline.TimedGenerator timedGenerator)
        {
            return (MoveCardToCenterStackFromHandModel)timedGenerator.TimedCommandArg.CommandArg;
        }

        // - メソッド

        /// <summary>
        /// ゲーム画面の同期を始めます
        /// 
        /// - ｎプレイヤーがピックアップしている場札を、右（または左）の台札へ移動する
        /// </summary>
        /// <param name="player">何番目のプレイヤー</param>
        /// <param name="place">右なら0、左なら1</param>
        public override void CreateSpanToLerp(
            SimulatorsOfTimeline.TimedGenerator timedGenerator,
            GameModelBuffer gameModelBuffer,
            LazyArgs.SetValue<SpanOfLeap.Model> setViewMovement)
        {
            var gameModel = new GameModel(gameModelBuffer);
            var player = GetModel(timedGenerator).Player;

            // ピックアップしているカードは、場札から抜くカード
            GetIndexOfFocusedHandCard(
                gameModelBuffer: gameModelBuffer,
                player: player,
                (indexToRemove) =>  // 確定：場札から抜くのは何枚目
                {
                    var place = GetModel(timedGenerator).Place;

                    // 確定：（抜いた後に）次にピックアップするカード（が先頭から何枚目か）
                    int indexOfNextPick;
                    {
                        // 確定：抜いた後の場札の数
                        int lengthAfterRemove;
                        {
                            // 抜く前の場札の数
                            var lengthBeforeRemove = gameModelBuffer.IdOfCardsOfPlayersHand[player].Count;
                            lengthAfterRemove = lengthBeforeRemove - 1;
                        }

                        if (lengthAfterRemove <= indexToRemove) // 範囲外アクセス防止対応
                        {
                            // 一旦、最後尾へ
                            indexOfNextPick = lengthAfterRemove - 1;
                        }
                        else
                        {
                            // そのまま
                            indexOfNextPick = indexToRemove;
                        }
                    }

                    // 確定：場札から台札へ移動するカード
                    var targetToRemove = gameModelBuffer.IdOfCardsOfPlayersHand[player][indexToRemove];

                    // モデル更新：場札を１枚抜く
                    gameModelBuffer.RemoveCardAtOfPlayerHand(player, indexToRemove);

                    // 確定：場札の枚数
                    var lengthOfHandCards = gameModel.GetLengthOfPlayerHandCards(player);

                    // 確定：抜いたあとの場札リスト
                    var idOfHandCardsAfterRemove = gameModel.GetCardsOfPlayerHand(player);

                    // モデル更新：何枚目の場札をピックアップしているか
                    gameModelBuffer.IndexOfFocusedCardOfPlayers[player] = indexOfNextPick;

                    // 確定：前の台札の天辺のカード
                    IdOfPlayingCards idOfPreviousTop = gameModel.GetTopOfCenterStack(place);

                    // モデル更新：次に、台札として置く
                    gameModelBuffer.AddCardOfCenterStack(place, targetToRemove);

                    // 台札へ置く
                    setViewMovement(PutCardToCenterStack.Generate(
                        startSeconds: timedGenerator.StartSeconds,
                        duration: timedGenerator.TimedCommandArg.Duration / 2.0f,
                        player: player,
                        place: place,
                        target: targetToRemove,
                        idOfPreviousTop));

                    // 場札の位置調整（をしないと歯抜けになる）
                    ArrangeHandCards.Generate(
                        startSeconds: timedGenerator.StartSeconds + timedGenerator.TimedCommandArg.Duration / 2.0f,
                        duration: timedGenerator.TimedCommandArg.Duration / 2.0f,
                        player: player,
                        indexOfPickup: indexOfNextPick, // 抜いたカードではなく、次にピックアップするカードを指定。 × indexToRemove
                        idOfHandCards: idOfHandCardsAfterRemove,
                        keepPickup: true,
                        setSpanToLerp: setViewMovement); // 場札

                    // TODO ★ ピックアップしている場札を持ち上げる
                    {

                    }

                });
        }

        private void GetIndexOfFocusedHandCard(GameModelBuffer gameModelBuffer, int player, LazyArgs.SetValue<int> setIndex)
        {
            int handIndex = gameModelBuffer.IndexOfFocusedCardOfPlayers[player]; // 何枚目の場札をピックアップしているか
            if (handIndex < 0 || gameModelBuffer.IdOfCardsOfPlayersHand[player].Count <= handIndex) // 範囲外は無視
            {
                return;
            }

            setIndex(handIndex);
        }

        private bool CanRemoveHandCardAt(
            GameModelBuffer gameModelBuffer,
            int player,
            int indexToRemove)
        {
            // 抜く前の場札の数
            var lengthBeforeRemove = gameModelBuffer.IdOfCardsOfPlayersHand[player].Count;
            if (indexToRemove < 0 || lengthBeforeRemove <= indexToRemove)
            {
                // 抜くのに失敗
                return false;
            }


            return true;
        }
    }
}
