namespace Assets.Scripts.Models.Timeline.Motions
{
    using UnityEngine;

    /// <summary>
    /// カードの動き
    /// 
    /// - Leapに使うもの
    /// </summary>
    internal class CardMovement
    {
        // - その他（生成）

        public CardMovement(Vector3 beginPosition, Vector3 endPosition, Quaternion beginRotation, Quaternion endRotation, GameObject goCard)
        {
            this.BeginPosition = beginPosition;
            this.EndPosition = endPosition;
            this.BeginRotation = beginRotation;
            this.EndRotation = endRotation;
            this.GoCard = goCard;
        }

        // - プロパティ

        internal Vector3 BeginPosition { get; private set; }
        internal Vector3 EndPosition { get; private set; }
        internal Quaternion BeginRotation { get; private set; }
        internal Quaternion EndRotation { get; private set; }
        internal GameObject GoCard { get; private set; }
    }
}
