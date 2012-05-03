using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ExtendedQueue<T> : Queue<T> where T : class
{
    public ExtendedQueue() 
        : base() {
        // Default constructor
    }

    public ExtendedQueue(Int32 capacity)
        : base(capacity) {
        // Default constructor
    }

    /// <summary>
    /// Removes the item at the specified index and returns a new ExtendedQueue
    /// </summary>
    public ExtendedQueue<T> Remove(T element)
    {
        if (Count == 0)
        {
            return new ExtendedQueue<T>(0);
        }

        ExtendedQueue<T> retVal = new ExtendedQueue<T>(Count - 1);

        for (Int32 i = 0; i < this.Count - 1; i++)
        {
            if (this.ElementAt(i) != element)
            {
                retVal.Enqueue(this.ElementAt(i));
            }
        }

        return retVal;
    }
}
