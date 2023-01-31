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
        // - プロパティー

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
        /// 台札の次の天辺の位置
        /// </summary>
        /// <param name="place"></param>
        /// <returns></returns>
        internal (float, float) GetXZOfNextCenterStackCard(GameModel gameModel, int place)
        {
            var length = gameModel.GetLengthOfCenterStackCards(place);
            if (length < 1)
            {
                // 床上
                var nextTopX2 = this.centerStacksX[place];
                var nextTopZ2 = this.centerStacksZ[place];
                return (nextTopX2, nextTopZ2);
            }

            // 台札の次の天辺の位置
            var idOfLastCard = gameModel.GetLastCardOfCenterStack(place); // 天辺（最後）のカード
            var goLastCard = ViewStorage.PlayingCards[idOfLastCard];
            var nextTopX = (this.centerStacksX[place] - goLastCard.transform.position.x) / 2 + this.centerStacksX[place];
            var nextTopZ = (this.centerStacksZ[place] - goLastCard.transform.position.z) / 2 + this.centerStacksZ[place];
            return (nextTopX, nextTopZ);
        }

        /// <summary>
        /// 今回フォーカスするカードを持ち上げる
        /// </summary>
        /// <param name="player"></param>
        /// <param name="handIndesx"></param>
        internal void SetFocusCardOfPlayerHand(GameModel gameModel, int player, int handIndesx)
        {
            var idOfFocusedHandCard = gameModel.GetCardAtOfPlayerHand(player, handIndesx);
            Debug.Log($"[GameViewModel SetFocusCardOfPlayerHand] idOfFocusedHandCard:{idOfFocusedHandCard}");
            this.SetFocusHand(idOfFocusedHandCard);
        }

        /// <summary>
        /// 前にフォーカスしていたカードを、盤に下ろす
        /// </summary>
        /// <param name="player"></param>
        /// <param name="handIndex"></param>
        internal void ResetFocusCardOfPlayerHand(GameModel gameModel, int player, int handIndex)
        {
            var goPreviousCard = gameModel.GetCardAtOfPlayerHand(player, handIndex);
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

            if (0 <= handIndex && handIndex < gameModel.GetLengthOfPlayerHandCards(player)) // 範囲内なら
            {
                // 抜いたカードの右隣のカードを（有れば）ピックアップする
                this.SetFocusCardOfPlayerHand(gameModel, player, handIndex);
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

            int numberOfCards = gameModel.GetLengthOfPlayerHandCards(player); // 場札の枚数
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
            foreach (var goCard in gameModel.GetCardsOfPlayerHand(player))
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

        /// <summary>
        /// 隣のカードへフォーカスを移します
        /// </summary>
        /// <param name="player"></param>
        /// <param name="direction">後ろ:0, 前:1</param>
        internal void MoveFocusToNextCard(GameModel gameModel, int player, int direction, int indexOfFocusedHandCard, LazyArgs.SetValue<int> setIndexOfNextFocusedHandCard)
        {
            int current;
            var length = gameModel.GetLengthOfPlayerHandCards(player);

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

            if (0 <= indexOfFocusedHandCard && indexOfFocusedHandCard < gameModel.GetLengthOfPlayerHandCards(player)) // 範囲内なら
            {
                // 前にフォーカスしていたカードを、盤に下ろす
                this.ResetFocusCardOfPlayerHand(gameModel, player, indexOfFocusedHandCard);
            }

            if (0 <= current && current < gameModel.GetLengthOfPlayerHandCards(player)) // 範囲内なら
            {
                // 今回フォーカスするカードを持ち上げる
                this.SetFocusCardOfPlayerHand(gameModel, player, current);
            }
        }
    }
}
