using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LootItem
{
    public ScriptableObject item;
    [Range(0,100)]
    public float lootChancePercentage;
}
