using UnityEngine;

public class PortalTrigger : MonoBehaviour
{
    public PortalBehaviour teleportLogic;
    public SpawnType teleportSide;
    [SerializeField] private BoxCollider coll;

    private void OnEnable()
    {
        GameEvents.instance.OnLastRoomReady += CheckForPortalSpawner;
    }

    private void OnDisable()
    {
        GameEvents.instance.OnLastRoomReady -= CheckForPortalSpawner;
    }

    private void CheckForPortalSpawner()
    {
        if (teleportLogic != null)
        {
            Vector3 size = coll.bounds.size;
            Collider[] colls = Physics.OverlapBox(gameObject.transform.position, size / 2);
            foreach (var item in colls)
            {
                if (item.gameObject.TryGetComponent(out PortalSpawner portalSpawner))
                {
                    Transform teleportDestination = teleportSide switch
                    {
                        SpawnType.NORTH => portalSpawner.GetTeleportPoint(SpawnType.SOUTH),
                        SpawnType.EAST => portalSpawner.GetTeleportPoint(SpawnType.WEST),
                        SpawnType.SOUTH => portalSpawner.GetTeleportPoint(SpawnType.NORTH),
                        SpawnType.WEST => portalSpawner.GetTeleportPoint(SpawnType.EAST),
                        _ => item.gameObject.transform,
                    };

                    if (teleportDestination == null)
                    {
                        teleportDestination = item.gameObject.transform;
                    }

                    teleportLogic.SetDestinationStuff(item.gameObject, teleportDestination);
                    break;
                }
            }
        }
    }
}
