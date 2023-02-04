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
        /// 底端
        /// 
        /// - `0.0f` は盤
        /// </summary>
        readonly float minY = 0.5f;

        readonly float[] handCardsZ = new[] { -28.0f, 42.0f };

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

        internal LazyArgs.GetValue<float> GetYOfMinOfCards()
        {
            return () => minY;
        }

        internal LazyArgs.GetValue<float[]> GetZOfHandCardsOrgin()
        {
            return () => handCardsZ;
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
            LazyArgs.GetValue<int> getLengthOfCenterStackCards,
            LazyArgs.GetValue<IdOfPlayingCards> getLastCardOfCenterStack)
        {
            var length = getLengthOfCenterStackCards();
            if (length < 1)
            {
                // 床上
                return new Vector3(
                    x: this.centerStacksX[place],
                    y: this.centerStacksY[place],
                    z: this.centerStacksZ[place]);
            }

            // 台札の次の天辺の位置
            var idOfLastCard = getLastCardOfCenterStack();
            var goLastCard = GameObjectStorage.Items[Specification.GetIdOfGameObject(idOfLastCard)];
            return new Vector3(
                x: (this.centerStacksX[place] - goLastCard.transform.position.x) / 2 + this.centerStacksX[place],
                y: this.centerStacksY[place],
                z: (this.centerStacksZ[place] - goLastCard.transform.position.z) / 2 + this.centerStacksZ[place]);
        }
    }
}
