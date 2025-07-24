using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SNGames.CommonModule
{
    public class ConfigRegistry : MonoBehaviour
    {
        [SerializeField] private List<BaseConfig> configsInGame;

        private void Awake()
        {
            InitialiseConfigsInGame();
        }

        [ContextMenu("Refresh")]
        private void InitialiseConfigsInGame()
        {
            foreach (BaseConfig config in configsInGame)
            {
                config.Refresh();
            }
        }
    }
}
