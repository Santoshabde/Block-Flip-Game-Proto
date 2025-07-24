using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SNGames.CommonModule
{
    public static class SN_SDKExtensionMethods
    {
        public static bool IsType<T>(this State state)
        {
            return (typeof(T) == state.GetType());
        }
    }
}