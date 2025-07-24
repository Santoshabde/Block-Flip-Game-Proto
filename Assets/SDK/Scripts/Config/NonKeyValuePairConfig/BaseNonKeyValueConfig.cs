using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SNGames.CommonModule
{
    public class BaseNonKeyValueConfig<T> : BaseConfig
    {
        [SerializeField]
        private T data;

        public static T Data;

        [ContextMenu("Refresh")]
        public override void Refresh()
        {
            Data = data;
        }
    }
}
