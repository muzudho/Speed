namespace Assets.Scripts.Models.Timeline.Commands
{
    using Assets.Scripts.Views;

    internal abstract class AbstractCommand : ICommand
    {
        abstract public void DoIt(GameModelBuffer gameModelBuffer, GameViewModel gameViewModel);

        virtual public void Lerp(float progress)
        {

        }

        /// <summary>
        /// 持続時間が切れたとき
        /// </summary>
        virtual public void OnLeave()
        {

        }
    }
}
