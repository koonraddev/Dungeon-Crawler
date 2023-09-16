using System.Collections;
using UnityEngine;


public abstract class TreasureSO : ScriptableObject
{
    public abstract string GetName();
    public abstract string GetDescription();
    public abstract Sprite GetThumbnail();
}
