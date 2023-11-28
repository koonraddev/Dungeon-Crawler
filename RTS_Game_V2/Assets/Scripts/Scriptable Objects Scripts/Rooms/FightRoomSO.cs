using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newFightRoom", menuName = "Scriptable Objects/Room/Fight Room")]
public class FightRoomSO : RoomSO
{
    [Header("Room section")]
    [SerializeField] private GameObject roomPlane;
    [Header("Door section")]
    [SerializeField] private List<DoorSO> doorsList;
    [SerializeField] private int maxDoorsInWall;
    [Header("Enemy section")]
    [SerializeField] private GameObject enemySpawnerObject;
    [SerializeField] private EnemySpawnerConfigurationSO enemySpawnerConfigurationSO;

    public override int MaxDoorsInWall => maxDoorsInWall;

    public override List<DoorSO> RoomDoors => doorsList;

    public override GameObject RoomPlane => roomPlane;


    public override void RoomBehavoiur(GameObject roomGameObject, bool isLastRoom = false)
    {
        //roomGameObject.GetComponent<Renderer>().material = roomFloorMaterial;
        GameObject spawner = Instantiate(enemySpawnerObject,  roomGameObject.transform.position, roomGameObject.transform.rotation);
        spawner.SetActive(false);
        spawner.transform.SetParent(roomGameObject.transform);
        if(spawner.TryGetComponent<EnemySpawner>(out EnemySpawner enemySpawner))
        {
            enemySpawner.SetSpawner(enemySpawnerConfigurationSO, roomGameObject);
            spawner.SetActive(true);
        }
    }
}
