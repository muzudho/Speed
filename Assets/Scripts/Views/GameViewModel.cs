namespace Assets.Scripts.Views
{
    using Assets.Scripts.Models;
    using Assets.Scripts.Simulators.Timeline;
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
        /// カードを積み重ねるときの厚み
        /// </summary>
        internal static readonly Vector3 yOfCardThickness = new Vector3(0f, 0.2f, 0f);

        /// <summary>
        /// 場札の原点
        /// 
        /// - 扇状を作るため、画面外の遠くにある
        /// </summary>
        internal static readonly Vector3[] positionOfHandCardsOrigin = new Vector3[]
        {
            new Vector3(0f,0.5f,-28.0f),
            new Vector3(0f,0.5f, 42.0f),
        };

        /// <summary>
        /// 手札（プレイヤー側で伏せて積んでる札）
        /// </summary>
        readonly Vector3[] positionOfPileCardsOrigin = new Vector3[] {
            new Vector3(40.0f, 0.5f,-6.5f),
            new Vector3(-40.0f, 0.5f, 16.0f),
        };

        /// <summary>
        /// 台札
        /// 
        /// - 右が 0、左が 1
        /// - 盤Y = 0.0f なので、それより上にある
        /// </summary>
        Vector3[] positionOfCenterStacksOrigin = new Vector3[] {
            new Vector3(15.0f, 0.5f, 0.5f),
            new Vector3(-15.0f, 2.5f, 9.0f),
        };

        // - メソッド

        internal LazyArgs.GetValue<Vector3[]> GetPositionOfPileCardsOrigin()
        {
            return () => positionOfPileCardsOrigin;
        }

        internal LazyArgs.GetValue<Vector3> GetPositionOfCards(IdOfPlayingCards idOfLastCard)
        {
            // 手札の天辺のカード
            var goTopCardOfPile = GameObjectStorage.Items[Specification.GetIdOfGameObject(idOfLastCard)];
            return () => goTopCardOfPile.transform.position;
        }

        /// <summary>
        /// 台札の次の天辺の位置
        /// </summary>
        /// <param name="gameModel"></param>
        /// <param name="place"></param>
        /// <param name="getLengthOfCenterStackCards"></param>
        /// <param name="getLastCardOfCenterStack">天辺（最後）のカード</param>
        /// <returns></returns>
        internal Vector3 GetPositionOfNextCenterStackCard(
            int place,
            LazyArgs.GetValue<ReadonlyList<IdOfPlayingCards>> getCenterStack)
        {
            var centerStack = getCenterStack();

            var length = centerStack.Count;
            if (length < 1)
            {
                // 床上
                return new Vector3(
                    x: this.positionOfCenterStacksOrigin[place].x,
                    y: this.positionOfCenterStacksOrigin[place].y,
                    z: this.positionOfCenterStacksOrigin[place].z);
            }

            // 台札の次の天辺の位置
            var idOfLastCard = centerStack.ElementAt(length - 1);
            var goLastCard = GameObjectStorage.Items[Specification.GetIdOfGameObject(idOfLastCard)];

            var position = new Vector3(
                x: (this.positionOfCenterStacksOrigin[place].x - goLastCard.transform.position.x) / 2 + this.positionOfCenterStacksOrigin[place].x,
                y: goLastCard.transform.position.y,
                z: (this.positionOfCenterStacksOrigin[place].z - goLastCard.transform.position.z) / 2 + this.positionOfCenterStacksOrigin[place].y);

            // カードの厚み分、上へ
            position += yOfCardThickness;

            return position;

        }
    }
}
