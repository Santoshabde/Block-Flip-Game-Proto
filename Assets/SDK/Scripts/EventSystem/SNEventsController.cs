using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SNGames.CommonModule
{
    public static class SNEventsController<T> where T : Enum
    {
        public static Dictionary<T, Action<object>> eventsInGame = new Dictionary<T, Action<object>>();

        public static void RegisterEvent(T eventName, Action<object> actionCallBack)
        {
            if (eventsInGame == null)
                eventsInGame = new Dictionary<T, Action<object>>();

            if (!eventsInGame.ContainsKey(eventName))
                eventsInGame.Add(eventName, actionCallBack);
            else
                eventsInGame[eventName] += actionCallBack;
        }

        public static void TriggerEvent(T eventName, object data = null)
        {
            if (!eventsInGame.ContainsKey(eventName))
                return;

            eventsInGame[eventName]?.Invoke(data);
        }

        public static void DeregisterEvent(T eventName, Action<object> listerner)
        {
            eventsInGame[eventName] -= listerner;
        }
    }
}
