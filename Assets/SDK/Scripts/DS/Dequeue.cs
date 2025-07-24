using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SNGames.CommonModule
{
    public class Dequeue<T> : LinkedList<T>
    {
        public void AddFront(T value)
        {
            AddFirst(value);
        }

        public void AddBack(T value)
        {
            AddLast(value);
        }

        public T RemoveFront()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("Deque is empty");
            }

            T front = First.Value;
            RemoveFirst();
            return front;
        }

        public T RemoveBack()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("Deque is empty");
            }

            T back = Last.Value;
            RemoveLast();
            return back;
        }
    }
}