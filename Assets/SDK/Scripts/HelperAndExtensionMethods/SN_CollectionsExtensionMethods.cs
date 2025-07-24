using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SNGames.CommonModule
{
    public static class SN_CollectionsExtensionMethods
    {
        public static Stack<T> ShuffleStack<T>(this Stack<T> stack)
        {
            // Convert the stack to a list
            List<T> list = new List<T>(stack);

            // Shuffle the list
            System.Random rng = new System.Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }

            // Convert the list back to a stack
            return (new Stack<T>(list));
        }
    }
}
