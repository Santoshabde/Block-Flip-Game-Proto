using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SNGames.CommonModule
{
    /// <summary>
    /// Binds all services into Service Registry pool!!
    /// </summary>
    public abstract class ServiceBinder : MonoBehaviour
    {
        private void Awake()
        {
            BindAllServicesInGame();
        }

        /// <summary>
        /// Binds All Services at the start of the game
        /// </summary>
        protected abstract void BindAllServicesInGame();

        protected void BindService(BaseService service)
        {
            ServiceRegistry.Bind(service);
        }
    }
}
