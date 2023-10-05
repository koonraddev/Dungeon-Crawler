using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newUnknownItem", menuName = "Scriptable Objects/Items/Unknown Item ")]
public class UnknownItemSO : ScriptableObject
{
    [SerializeField] private ItemInformationsSO itemInfos;
    //[SerializeField] private EnemySpawnerConfigurationSO enemySpawnerSO;
    [SerializeField] private LootSO lootSO;
    [SerializeField] private bool oneLoot;
    [Range(0, 100)]
    [SerializeField] private float lootChancePercentage;
    public ItemInformationsSO ItemInformations { get => itemInfos; }

    public void UseItem()
    {

        float randomValue = Random.Range(0f, 100f);

        if (randomValue <= lootChancePercentage)
        {
            List<Item> lootItems = lootSO.LootLogic(oneLoot);

            if (lootItems != null && lootItems.Count > 0)
            {
                foreach (Item item in lootItems)
                {
                    //Create loot box
                }  
            }
        }
        else
        {
            //SpawnerLogic
        }

    }
}
