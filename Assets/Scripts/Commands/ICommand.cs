namespace Assets.Scripts.Commands
{
    using Assets.Scripts.Models;
    using Assets.Scripts.Views;

    interface ICommand
    {
        void DoIt(GameModelBuffer gameModelBuffer, GameViewModel gameViewModel);
    }
}
