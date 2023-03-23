namespace Assets.Scripts.Vision.Models.Scheduler.O6thCommandMapping
{
    using Assets.Scripts.Coding;
    using System.Collections.Generic;
    using ModelOfSchedulerO4thCommand = Assets.Scripts.Vision.Models.Scheduler.O4thCommands;
    using ModelOfThinkingEngineCommand = Assets.Scripts.ThinkingEngine.Models.Commands;

    /// <summary>
    /// 思考エンジンのコマンドと、画面のコマンドが１対１対応
    /// </summary>
    internal class Model
    {
        static Model()
        {
            CommandMapping.Add(typeof(ModelOfThinkingEngineCommand.MoveCardsToHandFromPile).GetHashCode(), (args) => new ModelOfSchedulerO4thCommand.MoveCardsToHandFromPile(args.Item1, args.Item2));
            CommandMapping.Add(typeof(ModelOfThinkingEngineCommand.MoveCardsToPileFromCenterStacks).GetHashCode(), (args) => new ModelOfSchedulerO4thCommand.MoveCardsToPileFromCenterStacks(args.Item1, args.Item2));
            CommandMapping.Add(typeof(ModelOfThinkingEngineCommand.MoveCardToCenterStackFromHand).GetHashCode(), (args) => new ModelOfSchedulerO4thCommand.MoveCardToCenterStackFromHand(args.Item1, args.Item2));
            CommandMapping.Add(typeof(ModelOfThinkingEngineCommand.MoveFocusToNextCard).GetHashCode(), (args) => new ModelOfSchedulerO4thCommand.MoveFocusToNextCard(args.Item1, args.Item2));
            CommandMapping.Add(typeof(ModelOfThinkingEngineCommand.SetGameActive).GetHashCode(), (args) => new ModelOfSchedulerO4thCommand.SetGameActive(args.Item1, args.Item2));
            CommandMapping.Add(typeof(ModelOfThinkingEngineCommand.SetIdling).GetHashCode(), (args) => new ModelOfSchedulerO4thCommand.SetIdling(args.Item1, args.Item2));
        }

        // - プロパティ

        static Dictionary<int, LazyArgs.ConvertValue<
            (GameSeconds startTimeObj, ModelOfThinkingEngineCommand.IModel),
            ModelOfSchedulerO4thCommand.IModel>> CommandMapping = new();

        // - メソッド

        /// <summary>
        /// 命令を包む
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        internal static ModelOfSchedulerO4thCommand.IModel WrapCommand(
            GameSeconds startObj,
            ModelOfThinkingEngineCommand.IModel command)
        {
            return CommandMapping[command.GetType().GetHashCode()]((startObj, command));
        }
    }
}
