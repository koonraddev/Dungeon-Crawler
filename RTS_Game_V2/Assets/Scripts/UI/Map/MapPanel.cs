using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapPanel : MonoBehaviour
{
    [SerializeField] private GameObject roomMarkPrefab, portalMarkPrefab;
    [SerializeField] private Image mapObject;
    [SerializeField] private UIMapZoom mapZoom;

    Dictionary<GameObject, RoomMark> roomMarksDict = new();
    Dictionary<GameObject, PortalMark> portalMarksDict = new();
    private Vector2 roomMarkPrefabSize, portalMarkPrefabSize;

    private void Awake()
    {
        RectTransform tr = roomMarkPrefab.GetComponent<RectTransform>();
        RectTransform tr2 = portalMarkPrefab.GetComponent<RectTransform>();
        roomMarkPrefabSize = tr.sizeDelta;
        portalMarkPrefabSize = tr2.sizeDelta;
    }

    public void CreateRoomMark(GameObject roomObject, RoomMarkType markType)
    {
        float newPosX, newPosY;
        newPosX = roomMarkPrefabSize.x * roomObject.transform.position.x / 40;
        newPosY = roomMarkPrefabSize.y * roomObject.transform.position.z / 40;
        Vector3 newPos = new(newPosX, newPosY, 0f);

        GameObject newMarkObject = Instantiate(roomMarkPrefab);
        newMarkObject.transform.SetParent(mapObject.transform);
        newMarkObject.transform.localPosition = newPos;

        RoomMark newRoomMark = newMarkObject.GetComponent<RoomMark>();
        newRoomMark.SetRoom(markType);
        roomMarksDict.Add(roomObject, newRoomMark);
    }

    public void CreatePortalMark(GameObject portalObject, bool locked)
    {
        float newPosX, newPosY;
        //Debug.Log("Portal size: " + portalMarkPrefabSize.x + " X " + portalMarkPrefabSize.y);
        //Debug.Log("Portal obj pos: " + portalObject.transform.position.x + " X " + portalObject.transform.position.z);
        newPosX = portalMarkPrefabSize.x * portalObject.transform.position.x /6;
        newPosY = portalMarkPrefabSize.x * portalObject.transform.position.z /6;

        //Debug.Log("Portal canvas pos: " + newPosY + " X " + newPosY);
        Vector3 newPos = new(newPosX, newPosY, 0f);
        Vector3 newRot = new(0f, 0f, portalObject.transform.eulerAngles.y);

        GameObject newPortalObject = Instantiate(portalMarkPrefab);
        newPortalObject.transform.SetParent(mapObject.transform);
        newPortalObject.transform.localPosition = newPos;
        newPortalObject.transform.localEulerAngles = newRot;

        PortalMark newRoomMark = newPortalObject.GetComponent<PortalMark>();
        newRoomMark.SetPortal(locked);
        portalMarksDict.Add(portalObject, newRoomMark);
    }

    public void ClearMap()
    {
        foreach (var item in portalMarksDict)
        {
            Destroy(item.Value.gameObject);
        }

        foreach (var item in roomMarksDict)
        {
            Destroy(item.Value.gameObject);
        }

        portalMarksDict.Clear();
        roomMarksDict.Clear();
    }

    public void ShowPortalMark(GameObject portalObject)
    {
        foreach (var item in portalMarksDict)
        {
            if(item.Key == portalObject)
            {
                item.Value.ShowPortal();
                Debug.Log("POKAZ PORTAL");
                break;
            }
        }
    }

    public void ActivatePortalMark(GameObject portalObject)
    {
        foreach (var item in portalMarksDict)
        {
            if (item.Key == portalObject)
            {
                item.Value.ActivatePortal();
                break;
            }
        }
    }


    public void ActiveRoomMark(GameObject roomGameObject)
    {
        foreach (var item in roomMarksDict)
        {
            if(item.Key == roomGameObject)
            {
                item.Value.ActivateRoom();
                mapZoom.TargetRoomPos = item.Value.transform.localPosition;
                break;
            }
        }
    }

    public void DeactiveRoomMark(GameObject roomGameObject)
    {
        foreach (var item in roomMarksDict)
        {
            if (item.Key == roomGameObject)
            {
                item.Value.DeactivateRoom();
                break;
            }
        }
    }
}
