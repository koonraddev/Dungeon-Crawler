using System.Collections.Generic;
using UnityEngine;

public class RoomSpawnSpots : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnerSpots;
    public List<Transform> SpawnSpots
    {
        get
        {
            if(spawnerSpots != null)
            {
                return spawnerSpots;
            }
            else
            {
                return new();
            }
        }
    }
}
