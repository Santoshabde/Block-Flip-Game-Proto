using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SNGames.CommonModule
{
    public abstract class Subject<T> : MonoBehaviour
    {
        private static List<IObserverOfSubject<T>> observers = new List<IObserverOfSubject<T>>();

        public static void AddOberver(IObserverOfSubject<T> observer)
        {
            observers.Add(observer);
        }

        public static void RemoveObserver(IObserverOfSubject<T> observer)
        {
            observers.Remove(observer);
        }

        protected void NotifyAllObservers(T data)
        {
            observers.ForEach(observer => observer.OnNotify(data));
        }
    }
}
