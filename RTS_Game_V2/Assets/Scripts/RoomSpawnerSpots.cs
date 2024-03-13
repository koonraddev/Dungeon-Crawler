using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawnerSpots : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnerSpots;

    public List<Transform> SpawnerSpots
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
