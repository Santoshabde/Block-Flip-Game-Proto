using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SNGames.CommonModule
{
    public class BaseKeyValueConfig<T, K> : BaseConfig where T: IKeyValueConfigData<K>
    {
        [SerializeField]
        private List<T> data;

        public static Dictionary<K, T> Data;

        [ContextMenu("Refresh")]
        public override void Refresh()
        {
            Data = new Dictionary<K, T>();
            foreach (var item in data)
            {
                Data.Add(item.ID, item);
            }
        }
    }

    public interface IKeyValueConfigData<T>
    {
        public T ID { get; }
    }
}