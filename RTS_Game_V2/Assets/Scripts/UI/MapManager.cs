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

    public void AddPortal(GameObject portalObject, bool forceShowPortal = false)
    {
        if (portalObject.TryGetComponent(out Portal portal))
        {
            mapPanel.CreatePortalMark(portalObject, portal.KeyRequired);

            if (forceShowPortal)
            {
                ShowPortal(portalObject);
            }
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
