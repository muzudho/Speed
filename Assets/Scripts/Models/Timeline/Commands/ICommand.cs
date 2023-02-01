namespace Assets.Scripts.Models.Timeline.Commands
{
    using Assets.Scripts.Models;
    using Assets.Scripts.Views;

    interface ICommand
    {
        void DoIt(GameModelBuffer gameModelBuffer, GameViewModel gameViewModel);
    }
}
