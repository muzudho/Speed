namespace Assets.Scripts.Models.Timeline
{
    using Assets.Scripts.Views;

    /// <summary>
    /// タイム・スパン
    /// </summary>
    internal abstract class AbstractSpan : ISpan
    {
        virtual public void OnEnter(GameModelBuffer gameModelBuffer, GameViewModel gameViewModel)
        {
            // Ignored
        }

        virtual public void Lerp(float progress)
        {
            // Ignored
        }

        /// <summary>
        /// 持続時間が切れたとき
        /// </summary>
        virtual public void OnLeave()
        {
            // Ignored
        }
    }
}
