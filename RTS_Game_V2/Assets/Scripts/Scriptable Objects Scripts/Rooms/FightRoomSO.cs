using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "newFightRoom", menuName = "Scriptable Objects/Room/Fight Room")]
public class FightRoomSO : RoomSO
{
    [Header("Room section")]
    [SerializeField] private GameObject roomPlane;
    [Header("Portal section")]
    [SerializeField] private PortalSO portalSO;
    [Header("Spawner section")]
    [SerializeField] private List<EnemySpawnerConfigurationSO> spawnerConfigsList;

    public override PortalSO Portal => portalSO;

    public override GameObject Plane => roomPlane;


    public override void RoomBehavoiur(GameObject roomGameObject, bool isLastRoom = false)
    {
        MapManager.instance.AddRoom(roomGameObject, RoomMarkType.FIGHT);
        if (spawnerConfigsList.Count == 0 || roomGameObject == null)
        {
            return;
        }

        if (roomGameObject.TryGetComponent(out RoomSpawnerSpots roomSpawnerSpots))
        {
            Debug.Log("jest room spwner spots");
            if(roomSpawnerSpots.SpawnerSpots.Count > 0)
            {
                InstantiateSpawnersInSpots(roomGameObject, roomSpawnerSpots.SpawnerSpots);
            }
            else
            {
                InstantiateRandomSpawnerInCenter(roomGameObject);
            }
        }
        else
        {
            InstantiateRandomSpawnerInCenter(roomGameObject);
        }
    }

    private void InstantiateSpawnersInSpots(GameObject room, List<Transform> spots)
    {
        Debug.Log("sa spoty");
        int min = (int)MathF.Min(spawnerConfigsList.Count, spots.Count);

        for (int i = 0; i < min; i++)
        {
            EnemySpawnerConfigurationSO config = spawnerConfigsList[i];
            Transform spot = spots[i];

            InstantiateSpawner(room, spot, config);
        }
    }

    private void InstantiateRandomSpawnerInCenter(GameObject room)
    {
        Debug.Log("brak spotow");
        EnemySpawnerConfigurationSO spawnerConfig = spawnerConfigsList.OrderBy(spot => Guid.NewGuid()).Single();
        InstantiateSpawner(room, room.transform, spawnerConfig);
    }

    private void InstantiateSpawner(GameObject room, Transform spot, EnemySpawnerConfigurationSO spawnerConfig)
    {
        Debug.Log("tworze");
        GameObject spawnerObject = new("Spawner", typeof(EnemySpawner));
        EnemySpawner spawner = spawnerObject.GetComponent<EnemySpawner>();
        //spawnerObject.SetActive(false);
        spawnerObject.transform.SetPositionAndRotation(spot.position, spot.rotation);
        spawnerObject.transform.SetParent(spot);

        spawner.SetSpawner(spawnerConfig, room);
        //spawnerObject.SetActive(true);
    }
}
