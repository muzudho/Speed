namespace Assets.Scripts.Scheduler.AnalogCommands.O6thDACommandMapping
{
    using Assets.Scripts.Coding;
    using Assets.Scripts.Vision.Models;
    using System.Collections.Generic;
    using ModelOfAnalogCommand4thComplex = Assets.Scripts.Scheduler.AnalogCommands.O4thComplex;
    using ModelOfDigitalCommands = Assets.Scripts.ThinkingEngine.DigitalCommands;

    /// <summary>
    /// 思考エンジンのコマンドと、画面のコマンドが１対１対応
    /// 
    /// - べつに、このマッピングを使わなければいけないというわけではない
    /// </summary>
    internal class Model
    {
        static Model()
        {
            DACommandMapping.Add(typeof(ModelOfDigitalCommands.MoveCardsToHandFromPile).GetHashCode(), (args) => new ModelOfAnalogCommand4thComplex.MoveCardsToHandFromPile(args.Item1, args.Item2));
            DACommandMapping.Add(typeof(ModelOfDigitalCommands.MoveCardsToPileFromCenterStacks).GetHashCode(), (args) => new ModelOfAnalogCommand4thComplex.MoveCardsToPileFromCenterStacks(args.Item1, args.Item2));
            DACommandMapping.Add(typeof(ModelOfDigitalCommands.MoveCardToCenterStackFromHand).GetHashCode(), (args) => new ModelOfAnalogCommand4thComplex.MoveCardToCenterStackFromHand(args.Item1, args.Item2));
            DACommandMapping.Add(typeof(ModelOfDigitalCommands.MoveFocusToNextCard).GetHashCode(), (args) => new ModelOfAnalogCommand4thComplex.MoveFocusToNextCard(args.Item1, args.Item2));
            DACommandMapping.Add(typeof(ModelOfDigitalCommands.SetGameActive).GetHashCode(), (args) => new ModelOfAnalogCommand4thComplex.SetGameActive(args.Item1, args.Item2));
            DACommandMapping.Add(typeof(ModelOfDigitalCommands.SetIdling).GetHashCode(), (args) => new ModelOfAnalogCommand4thComplex.SetIdling(args.Item1, args.Item2));
        }

        // - プロパティ

        /// <summary>
        /// Degital to Analog
        /// </summary>
        static Dictionary<int, LazyArgs.ConvertValue<
            (GameSeconds startTimeObj, ModelOfDigitalCommands.IModel),
            ModelOfAnalogCommand4thComplex.IModel>> DACommandMapping = new();

        // - メソッド

        /// <summary>
        /// 命令を包む
        /// </summary>
        /// <param name="digitalCommand"></param>
        /// <returns></returns>
        internal static ModelOfAnalogCommand4thComplex.IModel WrapCommand(
            GameSeconds startObj,
            ModelOfDigitalCommands.IModel digitalCommand)
        {
            return DACommandMapping[digitalCommand.GetType().GetHashCode()]((startObj, digitalCommand));
        }
    }
}
