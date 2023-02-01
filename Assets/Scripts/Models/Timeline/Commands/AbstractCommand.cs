namespace Assets.Scripts.Models.Timeline.Commands
{
    using Assets.Scripts.Views;

    internal abstract class AbstractCommand : ICommand
    {
        abstract public void DoIt(GameModelBuffer gameModelBuffer, GameViewModel gameViewModel);

        virtual public void Leap()
        {

        }
    }
}
