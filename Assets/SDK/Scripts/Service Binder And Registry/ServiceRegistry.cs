using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SNGames.CommonModule
{
    public static class ServiceRegistry
    {
        private static Dictionary<Type, BaseService> serviceMap;

        static ServiceRegistry()
        {
            serviceMap = new Dictionary<Type, BaseService>();
        }

        public static void Init()
        {
            foreach (var item in serviceMap)
            {
                item.Value.Init();
            }
        }

        public static void Bind(BaseService baseService)
        {
            if (serviceMap == null)
                serviceMap = new Dictionary<Type, BaseService>();

            serviceMap.Add(baseService.GetType(), baseService);
        }

        public static void UnBind<T>()
        {
            if (serviceMap != null)
                serviceMap.Remove(typeof(T));
        }

        public static Z Get<Z>() where Z : BaseService
        {
            BaseService result = null;
            if (serviceMap.TryGetValue(typeof(Z), out result))
            {
                return result as Z;
            }

            return null;
        }
    }
}