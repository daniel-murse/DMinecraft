using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Collections.Generic
{
    internal class SparseList<T> : IEnumerable<T> 
    {
        private Queue<int> freeIndices;

        private int[] indices;
        private T?[] elements;
        private int count;

        public SparseList(int capacity)
        {
            indices = new int[capacity];
            elements = new T[capacity];
        }

        public void Remove(int element)
        {
            if (count < 1)
                throw new InvalidOperationException();
            count--;
            //move index of last element
            indices[count] = element;
            //set last element to position being removed
            elements[indices[element]] = elements[count];
            //set position removed to no index
            indices[element] = -1;
            //set new position of moved element

        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in indices)
            {
                if(item > -1)
                    yield return elements[item];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
