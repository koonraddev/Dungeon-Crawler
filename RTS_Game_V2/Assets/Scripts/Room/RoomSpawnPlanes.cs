using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawnPlanes : MonoBehaviour
{
    [SerializeField] private List<MeshCollider> spawnPlanes;
    public List<MeshCollider> SpawnSpots
    {
        get
        {
            if (spawnPlanes != null)
            {
                return spawnPlanes;
            }
            else
            {
                return new();
            }
        }
    }
}
