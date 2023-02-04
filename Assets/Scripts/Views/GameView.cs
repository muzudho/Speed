namespace Assets.Scripts.Views
{
    using Assets.Scripts.Models;
    using Assets.Scripts.Simulators.Timeline;
    using System;
    using UnityEngine;

    /// <summary>
    /// 画面表示関連
    /// 
    /// 西端: -62.0f
    /// 東端: 62.0f
    /// </summary>
    public static class GameView
    {
        // - プロパティー

        // TODO ★ Vector3 のプロパティ自体は readonly ではない

        /// <summary>
        /// カードを積み重ねるときの厚み
        /// </summary>
        internal static readonly Vector3Immutable yOfCardThickness = new Vector3Immutable(0f, 0.2f, 0f);

        /// <summary>
        /// 場札の原点
        /// 
        /// - 扇状を作るため、画面外の遠くにある
        /// </summary>
        internal static readonly Vector3Immutable[] positionOfHandCardsOrigin = new Vector3Immutable[]
        {
            new Vector3Immutable(0f,0.5f,-28.0f),
            new Vector3Immutable(0f,0.5f, 42.0f),
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
        readonly static Vector3[] positionOfCenterStacksOrigin = new Vector3[] {
            new Vector3(15.0f, 0.5f, 0.5f),
            new Vector3(-15.0f, 2.5f, 9.0f),
        };

        // - メソッド

        /// <summary>
        /// 台札の次の天辺の位置
        /// </summary>
        /// <param name="gameModel"></param>
        /// <param name="place"></param>
        /// <param name="getLengthOfCenterStackCards"></param>
        /// <param name="getLastCardOfCenterStack">天辺（最後）のカード</param>
        /// <returns></returns>
        internal static Vector3 GetPositionOfNextCenterStackCard(
            int place,
            LazyArgs.GetValue<ReadonlyList<IdOfPlayingCards>> getCenterStack)
        {
            var centerStack = getCenterStack();

            var length = centerStack.Count;
            if (length < 1)
            {
                // 床上
                return new Vector3(
                    x: positionOfCenterStacksOrigin[place].x,
                    y: positionOfCenterStacksOrigin[place].y,
                    z: positionOfCenterStacksOrigin[place].z);
            }

            // 台札の次の天辺の位置
            var idOfLastCard = centerStack.ElementAt(length - 1);
            var goLastCard = GameObjectStorage.Items[Specification.GetIdOfGameObject(idOfLastCard)];

            var position = new Vector3(
                x: (positionOfCenterStacksOrigin[place].x - goLastCard.transform.position.x) / 2 + positionOfCenterStacksOrigin[place].x,
                y: goLastCard.transform.position.y,
                z: (positionOfCenterStacksOrigin[place].z - goLastCard.transform.position.z) / 2 + positionOfCenterStacksOrigin[place].y);

            // カードの厚み分、上へ
            return yOfCardThickness.Add(position);
        }

        /// <summary>
        /// ぴったり積むと不自然だから、X と Z を少しずらすための仕組み
        /// 
        /// - １プレイヤー、２プレイヤーのどちらも右利きと仮定
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        internal static (float, float, float) MakeShakeForCenterStack(int player)
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
