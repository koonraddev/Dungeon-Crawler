using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPassiveItem", menuName = "Scriptable Objects/Items/Passive Item")]
public class PassiveItemSO : ScriptableObject
{
    [SerializeField] private int itemID;
    [SerializeField] private ItemInformationsSO itemInfos;
    [SerializeField] private bool multipleUse;

    public ItemInformationsSO ItemInformations { get => itemInfos; }
    public bool MultipleUse { get => multipleUse; }
    public int ItemID { get => itemID; }
}
