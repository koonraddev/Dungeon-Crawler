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
        if(lootPrefab.GetComponentInChildren<ContainerObject>() != null)
        {
            canCreateLoot = true;
        }
        else
        {
            canCreateLoot = false;
        }
    }
    
    public void CreateLoot(Vector3 position, Container container,float lootExistingTime = 0)
    {
        if (container.Empty)
        {
            return;
        }

        if (canCreateLoot)
        {
            GameObject containerObject = null;
            ContainerObject ltContainer;
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
            ltContainer = containerObject.GetComponentInChildren<ContainerObject>();
            ltContainer.SetContainer(container, lootExistingTime);
            containerObject.SetActive(true);
        }
    }
}
