namespace Assets.Scripts.Vision.Models.Scheduler.O6thCommandMapping
{
    using Assets.Scripts.Coding;
    using System.Collections.Generic;
    using ModelOfSchedulerO4thCommand = Assets.Scripts.Vision.Models.Scheduler.O4thComplexCommands;
    using ModelOfThinkingEngineDigitalCommands = Assets.Scripts.ThinkingEngine.DigitalCommands;

    /// <summary>
    /// 思考エンジンのコマンドと、画面のコマンドが１対１対応
    /// 
    /// - べつに、このマッピングを使わなければいけないというわけではない
    /// </summary>
    internal class Model
    {
        static Model()
        {
            CommandMapping.Add(typeof(ModelOfThinkingEngineDigitalCommands.MoveCardsToHandFromPile).GetHashCode(), (args) => new ModelOfSchedulerO4thCommand.MoveCardsToHandFromPile(args.Item1, args.Item2));
            CommandMapping.Add(typeof(ModelOfThinkingEngineDigitalCommands.MoveCardsToPileFromCenterStacks).GetHashCode(), (args) => new ModelOfSchedulerO4thCommand.MoveCardsToPileFromCenterStacks(args.Item1, args.Item2));
            CommandMapping.Add(typeof(ModelOfThinkingEngineDigitalCommands.MoveCardToCenterStackFromHand).GetHashCode(), (args) => new ModelOfSchedulerO4thCommand.MoveCardToCenterStackFromHand(args.Item1, args.Item2));
            CommandMapping.Add(typeof(ModelOfThinkingEngineDigitalCommands.MoveFocusToNextCard).GetHashCode(), (args) => new ModelOfSchedulerO4thCommand.MoveFocusToNextCard(args.Item1, args.Item2));
            CommandMapping.Add(typeof(ModelOfThinkingEngineDigitalCommands.SetGameActive).GetHashCode(), (args) => new ModelOfSchedulerO4thCommand.SetGameActive(args.Item1, args.Item2));
            CommandMapping.Add(typeof(ModelOfThinkingEngineDigitalCommands.SetIdling).GetHashCode(), (args) => new ModelOfSchedulerO4thCommand.SetIdling(args.Item1, args.Item2));
        }

        // - プロパティ

        static Dictionary<int, LazyArgs.ConvertValue<
            (GameSeconds startTimeObj, ModelOfThinkingEngineDigitalCommands.IModel),
            ModelOfSchedulerO4thCommand.IModel>> CommandMapping = new();

        // - メソッド

        /// <summary>
        /// 命令を包む
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        internal static ModelOfSchedulerO4thCommand.IModel WrapCommand(
            GameSeconds startObj,
            ModelOfThinkingEngineDigitalCommands.IModel command)
        {
            return CommandMapping[command.GetType().GetHashCode()]((startObj, command));
        }
    }
}
