﻿namespace Assets.Scripts.Views
{
    using Assets.Scripts.Models;
    using Assets.Scripts.Models.Timeline;
    using Assets.Scripts.Simulators.Timeline;
    using Assets.Scripts.Views.Timeline;
    using System;
    using UnityEngine;

    /// <summary>
    /// 画面表示関連
    /// 
    /// 西端: -62.0f
    /// 東端: 62.0f
    /// </summary>
    public class GameViewModel
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
            var goLastCard = GameObjectStorage.Items[Specification.GetIdOfGameObject(idOfLastCard)];
            var nextTopX = (this.centerStacksX[place] - goLastCard.transform.position.x) / 2 + this.centerStacksX[place];
            var nextTopZ = (this.centerStacksZ[place] - goLastCard.transform.position.z) / 2 + this.centerStacksZ[place];
            return (nextTopX, nextTopZ);
        }

        /// <summary>
        /// 場札を並べる
        /// 
        /// - ２段階モーション
        ///     - １段階目：全ての場札を、少し扇状にカーブさせて整列させる
        ///     - ２段階目：ピックアップしていた場札を下ろしてしまっているので、ピックアップし直す
        /// - 左端は角度で言うと 112.0f
        /// </summary>
        internal void ArrangeHandCards(
            float startSeconds,
            float duration1,
            float duration2,
            GameModel gameModel,
            int player,
            LazyArgs.SetValue<MovementViewModel> setCardMovementModel)
        {
            // 25枚の場札が並べるように調整してある

            int numberOfCards = gameModel.GetLengthOfPlayerHandCards(player); // 場札の枚数
            if (numberOfCards < 1)
            {
                return; // 何もしない
            }

            // ピックアップしている場札は何番目
            int indexOfPickup = gameModel.GetIndexOfFocusedCardOfPlayer(player);

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
            foreach (var idOfCard in gameModel.GetCardsOfPlayerHand(player))
            {
                float x = range * Mathf.Cos(theta + playerTheta) + ox;
                float z = range * Mathf.Sin(theta + playerTheta) + oz + offsetCircleCenterZ;

                var idOfGo = Specification.GetIdOfGameObject(idOfCard);
                var goCard = GameObjectStorage.Items[idOfGo];
                setCardMovementModel(new MovementViewModel(
                    startSeconds: startSeconds,
                    duration: duration1,
                    beginPosition: goCard.transform.position,
                    endPosition: new Vector3(x, this.minY, z),
                    beginRotation: goCard.transform.rotation,
                    endRotation: Quaternion.Euler(0, angleY, cardAngleZ),
                    idOfGameObject: idOfGo));


                // 更新
                theta += thetaStep;
            }

            // 場札を並べなおすと、持ち上げていたカードを下ろしてしまうので、再度、持ち上げる
            {
                Debug.Log($"[ArrangeHandCards] 再度持上げ handIndex:{indexOfPickup}");

                if (0 <= indexOfPickup && indexOfPickup < gameModel.GetLengthOfPlayerHandCards(player)) // 範囲内なら
                {
                    var idOfCard = gameModel.GetCardAtOfPlayerHand(player, indexOfPickup); // ピックアップしている場札

                    // 抜いたカードの右隣のカードを（有れば）ピックアップする
                    setCardMovementModel(MovementGenerator.PickupCardOfHand(
                        startSeconds: startSeconds + duration1,
                        duration: duration2,
                        idOfCard: idOfCard));
                }
            }
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
    }
}
