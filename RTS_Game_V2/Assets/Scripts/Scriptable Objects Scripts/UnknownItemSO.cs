using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newUnknownItem", menuName = "Scriptable Objects/Items/Unknown Item ")]
public class UnknownItemSO : ScriptableObject
{
    [SerializeField] private int itemID;
    [SerializeField] private ItemInformationsSO itemInfos;
    //[SerializeField] private EnemySpawnerConfigurationSO enemySpawnerSO;
    [SerializeField] private LootSO lootSO;
    [SerializeField] private bool oneLoot;
    [Range(0, 100)]
    [SerializeField] private float lootChancePercentage;
    [SerializeField] private GameObject lootContainerPrefab;
    public ItemInformationsSO ItemInformations { get => itemInfos; }
    public int ItemID { get => itemID; }
    public void InspectItem()
    {

        float randomValue = Random.Range(0f, 100f);

        if (randomValue <= lootChancePercentage)
        {
            Debug.Log("Loot!");
            List<ContainerSlot> containerSlots = lootSO.GetLoot(oneLoot);

            if (containerSlots != null && containerSlots.Count > 0)
            {
                LootManager.instance.CreateLoot(Vector3.zero, containerSlots, itemInfos.ItemName, itemInfos.ItemDescription);
            }
        }
        else
        {
            //SpawnerLogic
            Debug.Log("SPAWN!");
        }

    }
}
