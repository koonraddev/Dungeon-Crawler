using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    public static LootManager instance;

    [SerializeField] private GameObject lootPrefab;
    private List<GameObject> createdContainers = new();
    private bool canCreateLoot;
    private void Awake()
    {
        instance = this;
        if(lootPrefab.GetComponentInChildren<LootContainer>() != null)
        {
            canCreateLoot = true;
            Debug.Log("true");
        }
        else
        {
            canCreateLoot = false;
            Debug.Log("false");
        }
    }
    
    public void CreateLoot(Vector3 position,List<ContainerSlot> lootSlots, string lootName,float lootExistingTime ,string lootDescription = "")
    {
        if (canCreateLoot)
        {
            GameObject containerObject = null;
            LootContainer ltContainer;
            if (createdContainers.Count > 0)
            {
                foreach (var item in createdContainers)
                {
                    if (!item.activeSelf)
                    {
                        containerObject = item;
                        break;
                    }
                }

            }
            if (containerObject == null)
            {
                containerObject = Instantiate(lootPrefab);
                createdContainers.Add(containerObject);
            }
            containerObject.transform.position = position;
            ltContainer = containerObject.GetComponentInChildren<LootContainer>();
            ltContainer.SetLoot(lootSlots, lootName, lootDescription, lootExistingTime);
            containerObject.SetActive(true);
        }
    }
}
