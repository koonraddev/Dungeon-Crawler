using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newUnknownItem", menuName = "Scriptable Objects/Items/Unknown Item ")]
public class UnknownItemSO : ScriptableObject
{
    [SerializeField] private int itemID;
    [SerializeField] private ItemInformationsSO itemInfos;
    [SerializeField] private LootSO lootSO;
    [Range(0, 100)]
    [SerializeField] private float lootChancePercentage;
    [SerializeField] private GameObject lootContainerPrefab;
    public ItemInformationsSO ItemInformations { get => itemInfos; }
    public int ItemID { get => itemID; }
    public bool InspectItem()
    {
        float randomValue = Random.Range(0f, 100f);

        if (randomValue <= lootChancePercentage)
        {
            Container container = lootSO.GetContainer(itemInfos.ItemName);
            List<ContainerSlot> containerSlots = container.Slots;

            if (containerSlots != null && containerSlots.Count > 0)
            {
                LootManager.instance.CreateLoot(Vector3.zero, container, lootContainerPrefab, lootSO.LootTimeExisting);
            }
        }
        return true;
    }
}
