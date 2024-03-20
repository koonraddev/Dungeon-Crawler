using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagersSpawner : MonoBehaviour
{
    [SerializeField] private GameObject canvasManagerPrefab;
    [SerializeField] private GameObject sceneLoaderPrefab;
    [SerializeField] private GameObject saveManagerPrefab;

    private static bool managersSpawned = false;

    private void Awake()
    {
        if (!managersSpawned)
        {
            Instantiate(canvasManagerPrefab);
            Instantiate(sceneLoaderPrefab);
            Instantiate(saveManagerPrefab);
            managersSpawned = true;
        }

        Destroy(this);
    }
}
