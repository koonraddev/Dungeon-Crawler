using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPassiveItem", menuName = "Scriptable Objects/Items/Passive Item")]
public class PassiveItemSO : ScriptableObject
{
    [SerializeField] private int itemID;
    [SerializeField] private ItemInformationsSO itemInfos;
    [SerializeField] private bool isReusable;

    public ItemInformationsSO ItemInformations { get => itemInfos; }
    public bool IsReusable { get => itemInfos; }
    public int ItemID { get => itemID; }
}
