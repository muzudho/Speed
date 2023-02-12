namespace Assets.Scripts.Vision.World
{
    /// <summary>
    /// コーディングのテクニックのための仕込み
    /// </summary>
    internal class LazyArgs
    {
        public delegate void Action();
        public delegate void SetValue<T>(T value);
        public delegate T GetValue<T>();
    }
}
