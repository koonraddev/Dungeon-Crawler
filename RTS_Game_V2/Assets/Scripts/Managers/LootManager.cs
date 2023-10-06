using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    public static LootManager instance;

    [SerializeField] private GameObject lootPrefab;

    private void Awake()
    {
        instance = this;
    }
    
    public void CreateLoot(Vector3 position,List<ContainerSlot> lootSlots, string lootName, string lootDescription = "")
    {
        GameObject newLtContainer = Instantiate(lootPrefab);
        LootContainer ltContainer = newLtContainer.GetComponentInChildren<LootContainer>();
        ltContainer.SetLoot(lootSlots, lootName, lootDescription);
    }
}
