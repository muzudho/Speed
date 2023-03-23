namespace Assets.Scripts.Coding
{
    /// <summary>
    /// コーディングのテクニックのための仕込み
    /// </summary>
    internal class LazyArgs
    {
        public delegate void Action();
        public delegate void SetValue<T>(T value);
        public delegate T GetValue<T>();
        public delegate OUT ConvertValue<IN, OUT>(IN value);
    }
}
