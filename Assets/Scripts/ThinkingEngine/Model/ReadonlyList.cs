namespace Assets.Scripts.ThinkingEngine.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// 読取専用リスト
    /// 
    /// - 要素は、Immutable なものにしてください
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class ReadonlyList<T>
    {
        internal ReadonlyList(List<T> list)
        {
            this.list = list;
        }

        List<T> list;

        internal int Count
        {
            get
            {
                return this.list.Count;
            }
        }

        internal T ElementAt(int index)
        {
            return this.list[index];
        }
    }
}
