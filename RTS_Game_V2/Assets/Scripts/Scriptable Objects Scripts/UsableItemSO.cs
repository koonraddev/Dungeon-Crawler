using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newUsableItem", menuName = "Scriptable Objects/Items/Usable Item ")]
public class UsableItemSO : ScriptableObject
{
    [SerializeField] private ItemInformationsSO itemInfos;
    [SerializeField] private StatisticsSO itemStatistics;
    [SerializeField] private float duration;
    public ItemInformationsSO ItemInformations { get => itemInfos; }


    public void UseItem()
    {
        //to do logic; example: potion logic
    }
}
