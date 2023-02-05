namespace Assets.Scripts.Gui.Models
{
    using System.Collections.Generic;

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
