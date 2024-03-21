using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[CreateAssetMenu(fileName = "newChestRoom", menuName = "Scriptable Objects/Room/Chest Room")]
public class ChestRoomSO : RoomSO
{
    [Header("Room section")]
    [SerializeField] private GameObject roomPlane;
    [Header("Door section")]
    [SerializeField] private PortalSO teleportSO;
    [Header("Chest section")]
    [SerializeField] private List<ChestSO> chestList;
    [SerializeField] private GameObject chestPrefab;

    public override PortalSO Portal => teleportSO;

    public override GameObject Plane => roomPlane;

    float chestCheckRadius = 0f;

    public override void RoomBehavoiur(GameObject roomGameObject, bool isLastRoom = false)
    {
        MapManager.instance.AddRoom(roomGameObject, RoomMarkType.CHEST);
        if(chestList.Count == 0)
        {
            return;
        }

        List<ChestSO> shuffledChestList = chestList.OrderBy(chest => System.Guid.NewGuid()).ToList();


        int spotsNumber = 0;
        bool spawnPlanesExists = false;

        if (roomGameObject.TryGetComponent(out RoomSpawnSpots roomSpawnSpots))
        {
            spotsNumber = roomSpawnSpots.SpawnSpots.Count;
        }

        if(roomGameObject.TryGetComponent(out RoomSpawnPlanes spawnPlanes))
        {
            spawnPlanesExists = true;

            Collider chestColl = chestPrefab.GetComponentInChildren<Collider>();

            Vector3 collSize = chestColl.bounds.size;
            chestCheckRadius = Mathf.Max(collSize.x, collSize.z);
        }

        if(spotsNumber == 0 && !spawnPlanesExists)
        {
            return;
        }

        Transform parentTransform = roomGameObject.transform;


        var rnd = new System.Random();
        int chestToSpots = rnd.Next(1, shuffledChestList.Count);
        int chestToPlanes = shuffledChestList.Count - chestToSpots;

        List<Transform> pickedSpots = new();
        List<MeshCollider> pickedPlanes = new();
        if (chestToSpots > 0)
        {
            pickedSpots = roomSpawnSpots.SpawnSpots.OrderBy(spot => System.Guid.NewGuid()).Take(chestToSpots).ToList();
        }

        if(chestToPlanes > 0)
        {
            pickedPlanes = spawnPlanes.SpawnSpots;
        }


        for (int i = 0; i < shuffledChestList.Count; i++)
        {
            if (chestToSpots > 0)
            {
                Transform newSpot = pickedSpots[i];
                SpawnChest(parentTransform, shuffledChestList[i], newSpot.position, newSpot.rotation);
                chestToSpots--;
                continue;
            }

            MeshCollider newPlane = pickedPlanes[rnd.Next(0, pickedPlanes.Count)];
            if(!TryGetChestSpawnPosition(newPlane, out Vector3 newPosition))
            {
                continue;
            }
            else
            {
                SpawnChest(parentTransform, shuffledChestList[i], newPosition, GetChestSpawnRotation());
            }
        }
    }

    private void SpawnChest(Transform parentTransform ,ChestSO chestSO, Vector3 position, Quaternion rotaion)
    {
        Vector3 newPost = new(position.x, 0.7f, position.z);
        GameObject newChest = Instantiate(chestPrefab, newPost, rotaion);
        ContainerObject chestScript = newChest.GetComponentInChildren<ContainerObject>();
        var clone = Instantiate(chestSO);
        chestScript.SetContainer(clone.Container);
        newChest.transform.SetParent(parentTransform);
    }

    private bool TryGetChestSpawnPosition(MeshCollider planeMesh, out Vector3 newPosition,int attempt = 1)
    {
        float spawnPlaneSizeX = planeMesh.bounds.size.x;
        float spawnPlaneSizeZ = planeMesh.bounds.size.z;
        Vector3 planePos = planeMesh.transform.position;

        float spawnPosX = Random.Range(1, spawnPlaneSizeX / 2 - 1) * (Random.Range(0, 2) * 2 - 1);
        float spawnPosZ = Random.Range(1, spawnPlaneSizeZ / 2 - 1) * (Random.Range(0, 2) * 2 - 1);
        newPosition = new(spawnPosX, roomPlane.transform.position.y + 0.7f, spawnPosZ);
        newPosition += planePos;


        if (attempt < 10)
        {
            int layerNumber = LayerMask.NameToLayer("InteractiveObject");
            int layerMask = 1 << layerNumber;
            Collider[] colliders = Physics.OverlapSphere(newPosition, chestCheckRadius, layerMask);

            if (colliders.Length > 0)
            {
                return TryGetChestSpawnPosition(planeMesh, out newPosition, attempt + 1);
                
            }
            return true;
        }
        return false;
    }

    private Quaternion GetChestSpawnRotation()
    {
        return Quaternion.Euler(0f, Random.Range(0, 360), 0f);
    }
}
