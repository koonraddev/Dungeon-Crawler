using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newFightRoom", menuName = "Scriptable Objects/Room/Fight Room", order = 4)]
public class FightRoomSO : RoomSO
{
    [Header("Room section")]
    [SerializeField] private GameObject roomPlane;
    [SerializeField] private Material roomWallMaterial;
    [SerializeField] private Material roomFloorMaterial;
    [Header("Door section")]
    [SerializeField] private GameObject doorPrefab;
    [SerializeField] private List<DoorSO> doorsList;
    [SerializeField] private int maxDoorsInWall;
    [Header("Enemy section")]
    [SerializeField] private GameObject enemySpawnerObject;
    [SerializeField] private EnemySpawnerConfigurationSO enemySpawnerConfigurationSO;


    public override GameObject DoorPrefab()
    {
        return doorPrefab;
    }

    public override int MaxDoorsInWall()
    {
        return maxDoorsInWall;
    }

    public override Material RoomFloorMaterial()
    {
        return roomFloorMaterial;
    }

    public override void RoomBehavoiur(GameObject roomGameObject, bool isLastRoom = false)
    {
        roomGameObject.GetComponent<Renderer>().material = roomFloorMaterial;
        GameObject spawner = Instantiate(enemySpawnerObject,  roomGameObject.transform.position, roomGameObject.transform.rotation);
        spawner.SetActive(false);
        spawner.transform.SetParent(roomGameObject.transform);
        if(spawner.TryGetComponent<EnemySpawner>(out EnemySpawner enemySpawner))
        {
            enemySpawner.SetSpawner(enemySpawnerConfigurationSO, roomGameObject);
            spawner.SetActive(true);
        }

        if (isLastRoom)
        {
            GameEvents.instance.LastRoomReady();
        }
    }


    public override List<DoorSO> RoomDoors()
    {
        return doorsList;
    }

    public override GameObject RoomPlane()
    {
        return roomPlane;
    }

    public override Material RoomWallMaterial()
    {
        return roomWallMaterial;
    }
}
