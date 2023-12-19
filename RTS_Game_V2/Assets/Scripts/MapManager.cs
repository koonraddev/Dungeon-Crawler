using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;
    [SerializeField] private MapPanel mapPanel;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ClearMap()
    {
        mapPanel.ClearMap();
    }

    public void AddRoom(GameObject roomObject, RoomMarkType roomMarkType)
    {
        mapPanel.CreateRoomMark(roomObject, roomMarkType);
    }

    public void DeactiveRoom(GameObject roomObject)
    {
        mapPanel.DeactiveRoomMark(roomObject);
    }

    public void ActiveRoom(GameObject roomObject)
    {
        mapPanel.ActiveRoomMark(roomObject);
    }

    public void AddPortal(GameObject portalObject)
    {
        if (portalObject.TryGetComponent(out Door portal))
        {
            mapPanel.CreatePortalMark(portalObject, portal.KeyRequired);
        }
    }
    
    public void ShowPortal(GameObject portalObject)
    {
        mapPanel.ShowPortalMark(portalObject);
    }

    public void ActivatePortal(GameObject portalObject)
    {
        mapPanel.ActivatePortalMark(portalObject);
    }

}
