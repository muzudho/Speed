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
            CommandMapping.Add(typeof(ModelOfThinkingEngineCommand.MoveCardsToHandFromPile).GetHashCode(), (command) => new ModelOfSchedulerO4thCommand.MoveCardsToHandFromPile(command));
            CommandMapping.Add(typeof(ModelOfThinkingEngineCommand.MoveCardsToPileFromCenterStacks).GetHashCode(), (command) => new ModelOfSchedulerO4thCommand.MoveCardsToPileFromCenterStacks(command));
            CommandMapping.Add(typeof(ModelOfThinkingEngineCommand.MoveCardToCenterStackFromHand).GetHashCode(), (command) => new ModelOfSchedulerO4thCommand.MoveCardToCenterStackFromHand(command));
            CommandMapping.Add(typeof(ModelOfThinkingEngineCommand.MoveFocusToNextCard).GetHashCode(), (command) => new ModelOfSchedulerO4thCommand.MoveFocusToNextCard(command));
            CommandMapping.Add(typeof(ModelOfThinkingEngineCommand.SetGameActive).GetHashCode(), (command) => new ModelOfSchedulerO4thCommand.SetGameActive(command));
            CommandMapping.Add(typeof(ModelOfThinkingEngineCommand.SetIdling).GetHashCode(), (command) => new ModelOfSchedulerO4thCommand.SetIdling(command));
        }

        // - プロパティ

        static Dictionary<int, LazyArgs.ConvertValue<ModelOfThinkingEngineCommand.IModel, ModelOfSchedulerO4thCommand.IModel>> CommandMapping = new();

        // - メソッド

        /// <summary>
        /// 命令を包む
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        internal static ModelOfSchedulerO4thCommand.IModel WrapCommand(
            ModelOfThinkingEngineCommand.IModel command)
        {
            return CommandMapping[command.GetType().GetHashCode()](command);
        }
    }
}
