namespace Assets.Scripts.Simulators.Timeline
{
    using ModelsOfTimeline = Assets.Scripts.Models.Timeline;

    internal class Simulator
    {
        // - その他（生成）

        public Simulator(ModelsOfTimeline.Model model)
        {
            this.Model = model;
        }

        // - プロパティ

        internal ModelsOfTimeline.Model Model { get; private set; }
    }
}
