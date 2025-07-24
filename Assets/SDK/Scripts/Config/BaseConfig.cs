using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class BaseConfig : ScriptableObject
{
    public abstract void Refresh();
}
