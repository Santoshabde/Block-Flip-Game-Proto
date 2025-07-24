using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SNGames.CommonModule
{
    public class BaseUIDialog : MonoBehaviour
    {
        public virtual void OnOpenDialog()
        {
            gameObject.SetActive(true);
        }

        public virtual void OnCloseDialog()
        {
            gameObject.SetActive(false);
        }
    }
}