using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SNGames.CommonModule
{
    public class UIManager : SerializeSingleton<UIManager>
    {
        [SerializeField] protected List<BaseUIDialog> inGameDialogs;

        protected Dictionary<Type, BaseUIDialog> dialogs;

        protected virtual void Awake()
        {
            if (dialogs == null)
            {
                dialogs = new Dictionary<Type, BaseUIDialog>();
                inGameDialogs.ForEach(dialog => dialogs.Add(dialog.GetType(), dialog));
            }
        }

        public T GetDialog<T>() where T: BaseUIDialog
        {
            return (T)dialogs[typeof(T)];
        }

        public void OpenDialog<T>()
        {
            Type dialogType = typeof(T);
            if (dialogs.ContainsKey(dialogType))
            {
                dialogs[dialogType].OnOpenDialog();
            }
            else
                Debug.LogError("Dialog: " + dialogType + " not defined");
        }

        public void CloseDialog<T>(Action OnDialogClosed = null)
        {
            Type dialogType = typeof(T);
            if (dialogs.ContainsKey(dialogType))
            {
                dialogs[dialogType].OnCloseDialog();
                OnDialogClosed?.Invoke();
            }
            else
                Debug.LogError("Dialog: " + dialogType + " not defined");
        }

        public void CloseAllDialogs()
        {
            foreach (var item in inGameDialogs)
            {
                if (item.gameObject.activeSelf)
                    item.OnCloseDialog();
            }
        }
    }
}