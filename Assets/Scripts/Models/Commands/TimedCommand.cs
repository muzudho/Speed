namespace Assets.Scripts.Models.Commands
{
    using Assets.Scripts.Commands;

    class TimedCommand
    {
        // - その他（生成）

        internal TimedCommand(float seconds, ICommand command)
        {
            this.Seconds = seconds;
            this.Command = command;
        }

        // - プロパティ

        internal float Seconds { get; private set; }
        internal ICommand Command { get; private set; }
    }
}
