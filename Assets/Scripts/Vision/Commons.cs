namespace Assets.Scripts.Vision
{
    using Assets.Scripts.ThinkingEngine.Models;
    using Assets.Scripts.Vision.Models.World;
    using System;
    using UnityEngine;

    /// <summary>
    /// 画面表示関連
    /// 
    /// 西端: -62.0f
    /// 東端: 62.0f
    /// </summary>
    public static class Commons
    {
        // - プロパティー

        /// <summary>
        /// カードを積み重ねるときの厚み
        /// </summary>
        internal static readonly Vector3Immutable yOfCardThickness = new Vector3Immutable(0f, 0.2f, 0f);

        /// <summary>
        /// ピックアップしているカードの高さ
        /// </summary>
        internal static readonly Vector3Immutable yOfPickup = new Vector3Immutable(0.0f, 5.0f, 0.0f);

        /// <summary>
        /// ピックアップしているカードの捻り。 -5°
        /// </summary>
        internal static readonly QuaternionImmutable rotationOfPickup = QuaternionImmutable.Euler(0f, -5.0f, -5.0f);

        /// <summary>
        /// 場札の原点
        /// 
        /// - 扇状を作るため、画面外の遠くにある
        /// - y は 0.5 だと、カードを傾けたときに端が盤にめり込んでしまう
        /// </summary>
        internal static readonly Vector3Immutable[] positionOfHandCardsOrigin = new Vector3Immutable[]
        {
            new Vector3Immutable(0f, 1f, -28.0f),
            new Vector3Immutable(0f, 1f,  42.0f),
        };

        /// <summary>
        /// 手札（プレイヤー側で伏せて積んでる札）
        /// </summary>
        internal static readonly Vector3Immutable[] positionOfPileCardsOrigin = new Vector3Immutable[] {
            new Vector3Immutable(40.0f, 0.5f,-6.5f),
            new Vector3Immutable(-40.0f, 0.5f, 16.0f),
        };

        /// <summary>
        /// 台札
        /// 
        /// - 右が 0、左が 1
        /// - 盤Y = 0.0f なので、それより上にある
        /// </summary>
        readonly static Vector3Immutable[] positionOfCenterStacksOrigin = new Vector3Immutable[] {
            new Vector3Immutable(15.0f, 0.5f, 3.0f),
            new Vector3Immutable(-15.0f, 0.5f, 9.0f),
        };

        // - メソッド

        /// <summary>
        /// 台札の次の天辺の位置
        /// </summary>
        /// <param name="gameModel"></param>
        /// <param name="placeObj"></param>
        /// <param name="getLengthOfCenterStackCards"></param>
        /// <param name="getLastCardOfCenterStack">天辺（最後）のカード</param>
        /// <returns></returns>
        internal static Vector3 CreatePositionOfNewCenterStackCard(
            CenterStackPlace placeObj,
            IdOfPlayingCards previousTop)
        {
            if (previousTop == IdOfPlayingCards.None)
            {
                // 床上
                return positionOfCenterStacksOrigin[placeObj.AsInt].ToMutable();
            }

            // 置く前の台札の天辺
            var goLastCard = GameObjectStorage.Items[IdMapping.GetIdOfGameObject(previousTop)];

            var pos = new Vector3(
                x: (positionOfCenterStacksOrigin[placeObj.AsInt].X - goLastCard.transform.position.x) / 2 + positionOfCenterStacksOrigin[placeObj.AsInt].X,
                y: goLastCard.transform.position.y,
                z: (positionOfCenterStacksOrigin[placeObj.AsInt].Z - goLastCard.transform.position.z) / 2 + positionOfCenterStacksOrigin[placeObj.AsInt].Z);

            // カードの厚み分、上へ
            pos = Commons.yOfCardThickness.Add(pos);

            return pos;
        }

        /// <summary>
        /// ぴったり積むと不自然だから、X と Z を少しずらすための仕組み
        /// 
        /// - １プレイヤー、２プレイヤーのどちらも右利きと仮定
        /// </summary>
        /// <param name="playerObj"></param>
        /// <returns></returns>
        internal static Vector3 ShakePosition(CenterStackPlace playerObj)
        {
            // １プレイヤーから見て。左上にずれていくだろう
            var left = -1.5f;
            var right = 0.5f;
            var bottom = -0.5f;
            var top = 1.5f;

            switch (playerObj.AsInt)
            {
                case 0:
                    return new Vector3(UnityEngine.Random.Range(left, right), 0.0f, UnityEngine.Random.Range(bottom, top));

                case 1:
                    return new Vector3(UnityEngine.Random.Range(-right, -left), 0.0f, UnityEngine.Random.Range(-top, -bottom));

                default:
                    throw new Exception();
            }
        }


        /// <summary>
        /// ぴったり積むと不自然だから、X と Z を少しずらすための仕組み
        /// 
        /// - １プレイヤー、２プレイヤーのどちらも右利きと仮定
        /// </summary>
        /// <returns></returns>
        internal static Quaternion ShakeRotation()
        {
            // どのプレヤーも右利きと仮定しているので、回転方向はいずれのプレイヤーも同じ
            var angleY = UnityEngine.Random.Range(-10, 40);

            return Quaternion.Euler(
                x: 0.0f,
                y: angleY,
                z: 0.0f);
        }
    }
}
