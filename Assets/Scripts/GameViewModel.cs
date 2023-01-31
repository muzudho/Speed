namespace Assets.Scripts
{
    using Assets.Scripts.Models;
    using Assets.Scripts.Views;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    /// <summary>
    /// 西端: -62.0f
    /// 東端: 62.0f
    /// </summary>
    class GameViewModel
    {
        // - 初期化系

        internal void Init(GameModel gameModel)
        {
            // 全てのカードのゲーム・オブジェクトを、IDに紐づける
            ViewStorage.Add(IdOfPlayingCards.Clubs1, GameObject.Find($"Clubs 1"));
            ViewStorage.Add(IdOfPlayingCards.Clubs2, GameObject.Find($"Clubs 2"));
            ViewStorage.Add(IdOfPlayingCards.Clubs3, GameObject.Find($"Clubs 3"));
            ViewStorage.Add(IdOfPlayingCards.Clubs4, GameObject.Find($"Clubs 4"));
            ViewStorage.Add(IdOfPlayingCards.Clubs5, GameObject.Find($"Clubs 5"));
            ViewStorage.Add(IdOfPlayingCards.Clubs6, GameObject.Find($"Clubs 6"));
            ViewStorage.Add(IdOfPlayingCards.Clubs7, GameObject.Find($"Clubs 7"));
            ViewStorage.Add(IdOfPlayingCards.Clubs8, GameObject.Find($"Clubs 8"));
            ViewStorage.Add(IdOfPlayingCards.Clubs9, GameObject.Find($"Clubs 9"));
            ViewStorage.Add(IdOfPlayingCards.Clubs10, GameObject.Find($"Clubs 10"));
            ViewStorage.Add(IdOfPlayingCards.Clubs11, GameObject.Find($"Clubs 11"));
            ViewStorage.Add(IdOfPlayingCards.Clubs12, GameObject.Find($"Clubs 12"));
            ViewStorage.Add(IdOfPlayingCards.Clubs13, GameObject.Find($"Clubs 13"));

            ViewStorage.Add(IdOfPlayingCards.Diamonds1, GameObject.Find($"Diamonds 1"));
            ViewStorage.Add(IdOfPlayingCards.Diamonds2, GameObject.Find($"Diamonds 2"));
            ViewStorage.Add(IdOfPlayingCards.Diamonds3, GameObject.Find($"Diamonds 3"));
            ViewStorage.Add(IdOfPlayingCards.Diamonds4, GameObject.Find($"Diamonds 4"));
            ViewStorage.Add(IdOfPlayingCards.Diamonds5, GameObject.Find($"Diamonds 5"));
            ViewStorage.Add(IdOfPlayingCards.Diamonds6, GameObject.Find($"Diamonds 6"));
            ViewStorage.Add(IdOfPlayingCards.Diamonds7, GameObject.Find($"Diamonds 7"));
            ViewStorage.Add(IdOfPlayingCards.Diamonds8, GameObject.Find($"Diamonds 8"));
            ViewStorage.Add(IdOfPlayingCards.Diamonds9, GameObject.Find($"Diamonds 9"));
            ViewStorage.Add(IdOfPlayingCards.Diamonds10, GameObject.Find($"Diamonds 10"));
            ViewStorage.Add(IdOfPlayingCards.Diamonds11, GameObject.Find($"Diamonds 11"));
            ViewStorage.Add(IdOfPlayingCards.Diamonds12, GameObject.Find($"Diamonds 12"));
            ViewStorage.Add(IdOfPlayingCards.Diamonds13, GameObject.Find($"Diamonds 13"));

            ViewStorage.Add(IdOfPlayingCards.Hearts1, GameObject.Find($"Hearts 1"));
            ViewStorage.Add(IdOfPlayingCards.Hearts2, GameObject.Find($"Hearts 2"));
            ViewStorage.Add(IdOfPlayingCards.Hearts3, GameObject.Find($"Hearts 3"));
            ViewStorage.Add(IdOfPlayingCards.Hearts4, GameObject.Find($"Hearts 4"));
            ViewStorage.Add(IdOfPlayingCards.Hearts5, GameObject.Find($"Hearts 5"));
            ViewStorage.Add(IdOfPlayingCards.Hearts6, GameObject.Find($"Hearts 6"));
            ViewStorage.Add(IdOfPlayingCards.Hearts7, GameObject.Find($"Hearts 7"));
            ViewStorage.Add(IdOfPlayingCards.Hearts8, GameObject.Find($"Hearts 8"));
            ViewStorage.Add(IdOfPlayingCards.Hearts9, GameObject.Find($"Hearts 9"));
            ViewStorage.Add(IdOfPlayingCards.Hearts10, GameObject.Find($"Hearts 10"));
            ViewStorage.Add(IdOfPlayingCards.Hearts11, GameObject.Find($"Hearts 11"));
            ViewStorage.Add(IdOfPlayingCards.Hearts12, GameObject.Find($"Hearts 12"));
            ViewStorage.Add(IdOfPlayingCards.Hearts13, GameObject.Find($"Hearts 13"));

            ViewStorage.Add(IdOfPlayingCards.Spades1, GameObject.Find($"Spades 1"));
            ViewStorage.Add(IdOfPlayingCards.Spades2, GameObject.Find($"Spades 2"));
            ViewStorage.Add(IdOfPlayingCards.Spades3, GameObject.Find($"Spades 3"));
            ViewStorage.Add(IdOfPlayingCards.Spades4, GameObject.Find($"Spades 4"));
            ViewStorage.Add(IdOfPlayingCards.Spades5, GameObject.Find($"Spades 5"));
            ViewStorage.Add(IdOfPlayingCards.Spades6, GameObject.Find($"Spades 6"));
            ViewStorage.Add(IdOfPlayingCards.Spades7, GameObject.Find($"Spades 7"));
            ViewStorage.Add(IdOfPlayingCards.Spades8, GameObject.Find($"Spades 8"));
            ViewStorage.Add(IdOfPlayingCards.Spades9, GameObject.Find($"Spades 9"));
            ViewStorage.Add(IdOfPlayingCards.Spades10, GameObject.Find($"Spades 10"));
            ViewStorage.Add(IdOfPlayingCards.Spades11, GameObject.Find($"Spades 11"));
            ViewStorage.Add(IdOfPlayingCards.Spades12, GameObject.Find($"Spades 12"));
            ViewStorage.Add(IdOfPlayingCards.Spades13, GameObject.Find($"Spades 13"));

            // ゲーム開始時、とりあえず、すべてのカードは、いったん右の台札という扱いにする
            const int right = 0;// 台札の右
            // const int left = 1;// 台札の左
            foreach (var idOfCard in ViewStorage.PlayingCards.Keys)
            {
                // 右の台札
                this.goCenterStacksCards[right].Add(idOfCard);
            }

            // 右の台札をシャッフル
            this.goCenterStacksCards[right] = this.goCenterStacksCards[right].OrderBy(i => Guid.NewGuid()).ToList();

            // 右の台札をすべて、色分けして、黒色なら１プレイヤーの、赤色なら２プレイヤーの、手札に乗せる
            while (0 < this.GetLengthOfCenterStackCards(right))
            {
                this.MoveCardsToPileFromCenterStacks(right);
            }

            // １，２プレイヤーについて、手札から５枚抜いて、場札として置く（画面上の場札の位置は調整される）
            this.MoveCardsToHandFromPile(gameModel: gameModel, player: 0, numberOfCards: 5);
            this.MoveCardsToHandFromPile(gameModel: gameModel, player: 1, numberOfCards: 5);
        }

        // - プロパティー

        /// <summary>
        /// 手札
        /// 0: １プレイヤー（黒色）
        /// 1: ２プレイヤー（黒色）
        /// </summary>
        internal List<List<IdOfPlayingCards>> goPlayersPileCards = new() { new(), new() };

        /// <summary>
        /// 場札（プレイヤー側でオープンしている札）
        /// 0: １プレイヤー（黒色）
        /// 1: ２プレイヤー（黒色）
        /// </summary>
        internal List<List<IdOfPlayingCards>> goPlayersHandCards = new() { new(), new() };

        /// <summary>
        /// 台札（画面中央に積んでいる札）
        /// 0: 右
        /// 1: 左
        /// </summary>
        internal List<List<IdOfPlayingCards>> goCenterStacksCards = new() { new(), new() };

        /// <summary>
        /// 底端
        /// 
        /// - `0.0f` は盤
        /// </summary>
        internal readonly float minY = 0.5f;

        internal readonly float[] handCardsZ = new[] { -28.0f, 42.0f };

        // 手札（プレイヤー側で伏せて積んでる札）
        internal readonly float[] pileCardsX = new[] { 40.0f, -40.0f }; // 端っこは 62.0f, -62.0f
        internal readonly float[] pileCardsY = new[] { 0.5f, 0.5f };
        internal readonly float[] pileCardsZ = new[] { -6.5f, 16.0f };

        // 台札
        internal float[] centerStacksX = { 15.0f, -15.0f };

        /// <summary>
        /// 台札のY座標
        /// 
        /// - 右が 0、左が 1
        /// - 0.0f は盤なので、それより上にある
        /// </summary>
        internal float[] centerStacksY = { 0.5f, 0.5f };
        internal float[] centerStacksZ = { 2.5f, 9.0f };

        // - メソッド

        /// <summary>
        /// 台札の枚数
        /// </summary>
        /// <param name="place">右:0, 左:1</param>
        internal int GetLengthOfCenterStackCards(int place)
        {
            return this.goCenterStacksCards[place].Count;
        }

        internal IdOfPlayingCards GetCardOfCenterStack(int place, int startIndex)
        {
            return this.goCenterStacksCards[place].ElementAt(startIndex);
        }

        /// <summary>
        /// 天辺の台札
        /// </summary>
        /// <param name="place"></param>
        /// <returns></returns>
        internal IdOfPlayingCards GetLastCardOfCenterStack(int place)
        {
            var length = this.GetLengthOfCenterStackCards(place);
            return this.GetCardOfCenterStack(place, length - 1); // 最後のカード
        }

        internal void RemoveCardAtOfCenterStack(int place, int startIndex)
        {
            this.goCenterStacksCards[place].RemoveAt(startIndex);
        }

        internal void AddCardOfCenterStack(int place, IdOfPlayingCards idOfCard)
        {
            this.goCenterStacksCards[place].Add(idOfCard);
        }

        /// <summary>
        /// 台札の次の天辺の位置
        /// </summary>
        /// <param name="place"></param>
        /// <returns></returns>
        internal (float, float) GetXZOfNextCenterStackCard(int place)
        {
            var length = this.GetLengthOfCenterStackCards(place);
            if (length < 1)
            {
                // 床上
                var nextTopX2 = this.centerStacksX[place];
                var nextTopZ2 = this.centerStacksZ[place];
                return (nextTopX2, nextTopZ2);
            }

            // 台札の次の天辺の位置
            var idOfLastCard = this.GetLastCardOfCenterStack(place); // 天辺（最後）のカード
            var goLastCard = ViewStorage.PlayingCards[idOfLastCard];
            var nextTopX = (this.centerStacksX[place] - goLastCard.transform.position.x) / 2 + this.centerStacksX[place];
            var nextTopZ = (this.centerStacksZ[place] - goLastCard.transform.position.z) / 2 + this.centerStacksZ[place];
            return (nextTopX, nextTopZ);
        }

        /// <summary>
        /// 手札を追加
        /// </summary>
        /// <param name="player"></param>
        /// <param name="idOfCard"></param>
        internal void AddCardOfPlayersPile(int player, IdOfPlayingCards idOfCard)
        {
            this.goPlayersPileCards[player].Add(idOfCard);
        }

        /// <summary>
        /// 手札の枚数
        /// </summary>
        /// <returns></returns>
        internal int GetLengthOfPlayerPileCards(int player)
        {
            return this.goPlayersPileCards[player].Count;
        }

        internal List<IdOfPlayingCards> GetRangeCardsOfPlayerPile(int player, int startIndex, int numberOfCards)
        {
            return this.goPlayersPileCards[player].GetRange(startIndex, numberOfCards);
        }

        internal void RemoveRangeCardsOfPlayerPile(int player, int startIndex, int numberOfCards)
        {
            this.goPlayersPileCards[player].RemoveRange(startIndex, numberOfCards);
        }

        /// <summary>
        /// 場札を追加
        /// </summary>
        /// <param name="player"></param>
        /// <param name="idOfCards"></param>
        internal void AddRangeCardsOfPlayerHand(int player, List<IdOfPlayingCards> idOfCards)
        {
            this.goPlayersHandCards[player].AddRange(idOfCards);
        }

        /// <summary>
        /// 場札の枚数
        /// </summary>
        /// <returns></returns>
        internal int GetLengthOfPlayerHandCards(int player)
        {
            return this.goPlayersHandCards[player].Count;
        }

        internal List<IdOfPlayingCards> GetCardsOfPlayerHand(int player)
        {
            return this.goPlayersHandCards[player];
        }

        internal IdOfPlayingCards GetCardAtOfPlayerHand(int player, int handIndex)
        {
            return this.goPlayersHandCards[player].ElementAt(handIndex);
        }

        internal void RemoveCardAtOfPlayerHand(int player, int handIndex)
        {
            this.goPlayersHandCards[player].RemoveAt(handIndex);
        }

        /// <summary>
        /// 今回フォーカスするカードを持ち上げる
        /// </summary>
        /// <param name="player"></param>
        /// <param name="handIndesx"></param>
        internal void SetFocusCardOfPlayerHand(int player, int handIndesx)
        {
            var idOfFocusedHandCard = this.GetCardAtOfPlayerHand(player, handIndesx);
            Debug.Log($"[GameViewModel SetFocusCardOfPlayerHand] idOfFocusedHandCard:{idOfFocusedHandCard}");
            this.SetFocusHand(idOfFocusedHandCard);
        }

        /// <summary>
        /// 前にフォーカスしていたカードを、盤に下ろす
        /// </summary>
        /// <param name="player"></param>
        /// <param name="handIndex"></param>
        internal void ResetFocusCardOfPlayerHand(int player, int handIndex)
        {
            var goPreviousCard = this.GetCardAtOfPlayerHand(player, handIndex);
            this.ResetFocusHand(goPreviousCard);
        }

        /// <summary>
        /// 場札カードを持ち上げる
        /// </summary>
        /// <param name="card"></param>
        void SetFocusHand(IdOfPlayingCards idOfCard)
        {
            var liftY = 5.0f; // 持ち上げる（パースペクティブがかかっていて、持ち上げすぎると北へ移動したように見える）
            var rotateY = -5; // -5°傾ける
            var rotateZ = -5; // -5°傾ける

            var goCard = ViewStorage.PlayingCards[idOfCard];
            goCard.transform.position = new Vector3(goCard.transform.position.x, goCard.transform.position.y + liftY, goCard.transform.position.z);
            goCard.transform.rotation = Quaternion.Euler(goCard.transform.rotation.eulerAngles.x, goCard.transform.rotation.eulerAngles.y + rotateY, goCard.transform.eulerAngles.z + rotateZ);
        }

        /// <summary>
        /// 持ち上げたカードを場に戻す
        /// </summary>
        /// <param name="card"></param>
        void ResetFocusHand(IdOfPlayingCards idOfCard)
        {
            var liftY = 5.0f; // 持ち上げる（パースペクティブがかかっていて、持ち上げすぎると北へ移動したように見える）
            var rotateY = -5; // -5°傾ける
            var rotateZ = -5; // -5°傾ける

            // 逆をする
            liftY = -liftY;
            rotateY = -rotateY;
            rotateZ = -rotateZ;

            var goCard = ViewStorage.PlayingCards[idOfCard];
            goCard.transform.position = new Vector3(goCard.transform.position.x, goCard.transform.position.y + liftY, goCard.transform.position.z);
            goCard.transform.rotation = Quaternion.Euler(goCard.transform.rotation.eulerAngles.x, goCard.transform.rotation.eulerAngles.y + rotateY, goCard.transform.eulerAngles.z + rotateZ);
        }

        /// <summary>
        /// 場札を並べなおすと、持ち上げていたカードを下ろしてしまうので、再度、持ち上げる
        /// </summary>
        internal void ResumeCardPickup(GameModel gameModel, int player)
        {
            int handIndex = gameModel.GetIndexOfFocusedCardOfPlayer(player);

            if (0 <= handIndex && handIndex < this.GetLengthOfPlayerHandCards(player)) // 範囲内なら
            {
                // 抜いたカードの右隣のカードを（有れば）ピックアップする
                this.SetFocusCardOfPlayerHand(player, handIndex);
            }
        }

        /// <summary>
        /// 手札の上の方からｎ枚抜いて、場札の後ろへ追加する
        /// 
        /// - 画面上の場札は位置調整される
        /// </summary>
        internal void MoveCardsToHandFromPile(GameModel gameModel, int player, int numberOfCards)
        {
            // 手札の上の方からｎ枚抜いて、場札へ移動する
            var length = this.GetLengthOfPlayerPileCards(player); // 手札の枚数
            if (numberOfCards <= length)
            {
                var startIndex = length - numberOfCards;
                var goCards = this.GetRangeCardsOfPlayerPile(player, startIndex, numberOfCards);
                this.RemoveRangeCardsOfPlayerPile(player, startIndex, numberOfCards);
                this.AddRangeCardsOfPlayerHand(player, goCards);

                ArrangeHandCards(gameModel, player);
            }
        }

        /// <summary>
        /// 場札を並べる
        /// 
        /// - 左端は角度で言うと 112.0f
        /// </summary>
        internal void ArrangeHandCards(GameModel gameModel, int player)
        {
            int handIndex = gameModel.GetIndexOfFocusedCardOfPlayer(player);

            // 25枚の場札が並べるように調整してある

            int numberOfCards = this.GetLengthOfPlayerHandCards(player); // 場札の枚数
            if (numberOfCards < 1)
            {
                return; // 何もしない
            }

            float cardAngleZ = -5; // カードの少しの傾き

            int range = 200; // 半径。大きな円にするので、中心を遠くに離したい
            int offsetCircleCenterZ; // 中心位置の調整

            float angleY;
            float playerTheta;
            float angleStep = -1.83f;
            float startTheta = (numberOfCards * Mathf.Abs(angleStep) / 2 - Mathf.Abs(angleStep) / 2 + 90.0f) * Mathf.Deg2Rad;
            float thetaStep = angleStep * Mathf.Deg2Rad; ; // 時計回り

            float ox = 0.0f;
            float oz = this.handCardsZ[player];

            switch (player)
            {
                case 0:
                    // １プレイヤー
                    angleY = 180.0f;
                    playerTheta = 0;
                    offsetCircleCenterZ = -190;
                    break;

                case 1:
                    // ２プレイヤー
                    angleY = 0.0f;
                    playerTheta = 180 * Mathf.Deg2Rad;
                    offsetCircleCenterZ = 188;  // カメラのパースペクティブが付いているから、目視で調整
                    break;

                default:
                    throw new Exception();
            }

            float theta = startTheta;
            foreach (var goCard in this.GetCardsOfPlayerHand(player))
            {
                float x = range * Mathf.Cos(theta + playerTheta) + ox;
                float z = range * Mathf.Sin(theta + playerTheta) + oz + offsetCircleCenterZ;

                SetPosRot(goCard, x, this.minY, z, angleY: angleY, angleZ: cardAngleZ);
                theta += thetaStep;
            }

            // 場札を並べなおすと、持ち上げていたカードを下ろしてしまうので、再度、持ち上げる
            this.ResumeCardPickup(gameModel, player);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="card"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="angleY"></param>
        /// <param name="angleZ"></param>
        /// <param name="motionProgress">Update関数の中でないと役に立たない</param>
        internal void SetPosRot(IdOfPlayingCards idOfCard, float x, float y, float z, float angleY = 180.0f, float angleZ = 0.0f, float motionProgress = 1.0f)
        {
            var goCard = ViewStorage.PlayingCards[idOfCard];
            var beginPos = goCard.transform.position;
            var endPos = new Vector3(x, y, z);
            goCard.transform.position = Vector3.Lerp(beginPos, endPos, motionProgress);

            goCard.transform.rotation = Quaternion.Euler(0, angleY, angleZ);
        }

        /// <summary>
        /// 台札を抜く
        /// </summary>
        /// <param name="player"></param>
        /// <param name="indexOfHandCardToRemove"></param>
        /// <param name="setIndexOfNextFocusedHandCard"></param>
        internal void RemoveAtOfHandCard(int player, int place, int indexOfHandCardToRemove, LazyArgs.SetValue<int> setIndexOfNextFocusedHandCard)
        {
            // 抜く前の場札の数
            var lengthBeforeRemove = this.GetLengthOfPlayerHandCards(player);
            if (indexOfHandCardToRemove < 0 || lengthBeforeRemove <= indexOfHandCardToRemove)
            {
                // 抜くのに失敗
                return;
            }

            // 抜いた後の場札の数
            var lengthAfterRemove = lengthBeforeRemove - 1;

            // 抜いた後の次のピックアップするカードが先頭から何枚目か、先に算出
            int indexOfNextFocusedHandCard;
            if (lengthAfterRemove <= indexOfHandCardToRemove) // 範囲外アクセス防止対応
            {
                // 一旦、最後尾へ
                indexOfNextFocusedHandCard = lengthAfterRemove - 1;
            }
            else
            {
                // そのまま
                indexOfNextFocusedHandCard = indexOfHandCardToRemove;
            }

            var goCard = this.GetCardAtOfPlayerHand(player, indexOfHandCardToRemove); // 場札を１枚抜いて
            this.RemoveCardAtOfPlayerHand(player, indexOfHandCardToRemove);

            this.AddCardOfCenterStack2(goCard, place); // 台札
            setIndexOfNextFocusedHandCard(indexOfNextFocusedHandCard);
        }

        /// <summary>
        /// ぴったり積むと不自然だから、X と Z を少しずらすための仕組み
        /// 
        /// - １プレイヤー、２プレイヤーのどちらも右利きと仮定
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        internal (float, float, float) MakeShakeForCenterStack(int player)
        {
            // １プレイヤーから見て。左上にずれていくだろう
            var left = -1.5f;
            var right = 0.5f;
            var bottom = -0.5f;
            var top = 1.5f;
            var angleY = UnityEngine.Random.Range(-10, 40); // 反時計回りに大きく捻りそう

            switch (player)
            {
                case 0:
                    return (UnityEngine.Random.Range(left, right), UnityEngine.Random.Range(bottom, top), angleY);

                case 1:
                    return (UnityEngine.Random.Range(-right, -left), UnityEngine.Random.Range(-top, -bottom), angleY);

                default:
                    throw new Exception();
            }
        }

        internal void AddCardOfCenterStack2(IdOfPlayingCards idOfCard, int place)
        {
            // 手ぶれ
            var (shakeX, shakeZ, shakeAngleY) = this.MakeShakeForCenterStack(place);

            // 台札の次の天辺（一番後ろ）のカードの中心座標 X, Z
            var (nextTopX, nextTopZ) = this.GetXZOfNextCenterStackCard(place);

            // 台札の捻り
            var goCard = ViewStorage.PlayingCards[idOfCard];
            float nextAngleY = goCard.transform.rotation.eulerAngles.y;
            var length = this.GetLengthOfCenterStackCards(place);
            if (length < 1)
            {
            }
            else
            {
                nextAngleY += shakeAngleY;
            }

            this.AddCardOfCenterStack(place, idOfCard); // 台札として置く

            // 台札の位置をセット
            this.SetPosRot(idOfCard, nextTopX + shakeX, this.centerStacksY[place], nextTopZ + shakeZ, angleY: nextAngleY);

            // 次に台札に積むカードの高さ
            this.centerStacksY[place] += 0.2f;
        }

        /// <summary>
        /// 隣のカードへフォーカスを移します
        /// </summary>
        /// <param name="player"></param>
        /// <param name="direction">後ろ:0, 前:1</param>
        internal void MoveFocusToNextCard(int player, int direction, int indexOfFocusedHandCard, LazyArgs.SetValue<int> setIndexOfNextFocusedHandCard)
        {
            int current;
            var length = this.GetLengthOfPlayerHandCards(player);

            if (length < 1)
            {
                // 場札が無いなら、何もピックアップされていません
                current = -1;
            }
            else
            {
                switch (direction)
                {
                    // 後ろへ
                    case 0:
                        if (indexOfFocusedHandCard == -1 || length <= indexOfFocusedHandCard + 1)
                        {
                            // （ピックアップしているカードが無いとき）先頭の外から、先頭へ入ってくる
                            current = 0;
                        }
                        else
                        {
                            current = indexOfFocusedHandCard + 1;
                        }
                        break;

                    // 前へ
                    case 1:
                        if (indexOfFocusedHandCard == -1 || indexOfFocusedHandCard - 1 < 0)
                        {
                            // （ピックアップしているカードが無いとき）最後尾の外から、最後尾へ入ってくる
                            current = length - 1;
                        }
                        else
                        {
                            current = indexOfFocusedHandCard - 1;
                        }
                        break;

                    default:
                        throw new Exception();
                }
            }

            setIndexOfNextFocusedHandCard(current);

            if (0 <= indexOfFocusedHandCard && indexOfFocusedHandCard < this.GetLengthOfPlayerHandCards(player)) // 範囲内なら
            {
                // 前にフォーカスしていたカードを、盤に下ろす
                this.ResetFocusCardOfPlayerHand(player, indexOfFocusedHandCard);
            }

            if (0 <= current && current < this.GetLengthOfPlayerHandCards(player)) // 範囲内なら
            {
                // 今回フォーカスするカードを持ち上げる
                this.SetFocusCardOfPlayerHand(player, current);
            }
        }

        /// <summary>
        /// 台札を、手札へ移動する
        /// </summary>
        /// <param name="place">右:0, 左:1</param>
        internal void MoveCardsToPileFromCenterStacks(int place)
        {
            // 台札の一番上（一番後ろ）のカードを１枚抜く
            var numberOfCards = 1;
            var length = this.GetLengthOfCenterStackCards(place); // 台札の枚数
            if (1 <= length)
            {
                var startIndex = length - numberOfCards;
                var idOfCard = this.GetCardOfCenterStack(place, startIndex);
                this.RemoveCardAtOfCenterStack(place, startIndex);

                // 黒いカードは１プレイヤー、赤いカードは２プレイヤー
                int player;
                float angleY;
                var goCard = ViewStorage.PlayingCards[idOfCard];
                if (goCard.name.StartsWith("Clubs") || goCard.name.StartsWith("Spades"))
                {
                    player = 0;
                    angleY = 180.0f;
                }
                else if (goCard.name.StartsWith("Diamonds") || goCard.name.StartsWith("Hearts"))
                {
                    player = 1;
                    angleY = 0.0f;
                }
                else
                {
                    throw new Exception();
                }

                // プレイヤーの手札を積み上げる
                this.AddCardOfPlayersPile(player, idOfCard);
                this.SetPosRot(idOfCard, this.pileCardsX[player], this.pileCardsY[player], this.pileCardsZ[player], angleY: angleY, angleZ: 180.0f);
                this.pileCardsY[player] += 0.2f;
            }
        }
    }
}
