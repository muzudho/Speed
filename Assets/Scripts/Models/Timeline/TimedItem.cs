namespace Assets.Scripts.Models.Timeline
{
    using Assets.Scripts.Commands;

    class TimedItem
    {
        // - その他（生成）

        internal TimedItem(float seconds, ICommand command)
        {
            this.Seconds = seconds;
            this.Command = command;
        }

        // - プロパティ

        internal float Seconds { get; private set; }
        internal ICommand Command { get; private set; }
    }
}
