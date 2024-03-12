using UnityEngine;

[System.Serializable]
public class PortalClass
{
    [SerializeField] private SpawnType wallIndex;
    [SerializeField] private Portal portalScript;
    public PortalClass(SpawnType wallIndex,  Portal portalScript)
    {
        this.wallIndex = wallIndex;
        this.portalScript = portalScript;
    }
    public SpawnType WallIndex { get => wallIndex; }

    public Portal PortalScript { get => portalScript; }

}