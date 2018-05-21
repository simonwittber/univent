using System;
using System.Collections.Generic;

namespace DifferentMethods.Univents
{
    public class PriorityQueue<T> where T : IComparable<T>
    {
        List<T> items;

        public int Count
        {
            get
            {
                return items.Count;
            }
        }

        public void Clear() => items.Clear();

        public bool Contains(T item) => items.Contains(item);

        public PriorityQueue()
        {
            items = new List<T>();
        }

        public bool IsEmpty
        {
            get { return items.Count == 0; }
        }

        public T First
        {
            get
            {
                return items[0];
            }
        }

        public void Push(T item)
        {
            items.Add(item);
            BubbleDown(0, items.Count - 1);
        }

        public void Remove(T item) => items.Remove(item);

        public T Peek()
        {
            T item;
            var last = items[items.Count - 1];
            if (items.Count > 1)
            {
                item = items[0];
            }
            else
            {
                item = last;
            }
            return item;
        }

        public T Pop()
        {
            T item;
            var last = items[items.Count - 1];
            items.RemoveAt(items.Count - 1);
            if (items.Count > 0)
            {
                item = items[0];
                items[0] = last;
                BubbleUp(0);
            }
            else
            {
                item = last;
            }
            return item;
        }

        int Compare(T A, T B) => A.CompareTo(B);

        void BubbleDown(int startpos, int pos)
        {
            var newitem = items[pos];
            while (pos > startpos)
            {
                var parentpos = (pos - 1) >> 1;
                var parent = items[parentpos];
                if (Compare(parent, newitem) <= 0)
                    break;
                items[pos] = parent;
                pos = parentpos;
            }
            items[pos] = newitem;
        }

        void BubbleUp(int pos)
        {
            var endpos = items.Count;
            var startpos = pos;
            var newitem = items[pos];
            var childpos = 2 * pos + 1;
            while (childpos < endpos)
            {
                var rightpos = childpos + 1;
                if (rightpos < endpos && Compare(items[rightpos], items[childpos]) <= 0)
                    childpos = rightpos;
                items[pos] = items[childpos];
                pos = childpos;
                childpos = 2 * pos + 1;
            }
            items[pos] = newitem;
            BubbleDown(startpos, pos);
        }

    }

}