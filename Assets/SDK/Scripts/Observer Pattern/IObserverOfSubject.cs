using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SNGames.CommonModule
{
    public interface IObserverOfSubject<T>
    {
        public void OnNotify(T data);
    }
}