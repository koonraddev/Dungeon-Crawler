using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class PortalSpawner : MonoBehaviour
{
    private List<PortalClass> portalClassList = new();
    private PortalSO portalSO;
    private GameObject portalPrefab;

    private Vector3 colliderSize;

    private Dictionary<SpawnType, bool> portalsDict;

    private Vector3 pointA, pointAx, pointAz;
    private Vector3 pointB, pointBx, pointBz;
    private Vector3 pointC, pointCx, pointCz;
    private Vector3 pointD, pointDx, pointDz;
    private SpawnType? entryPortal;


    bool isStartRoom;
    private void OnEnable()
    {
        GameEvents.instance.OnStartLevel += OnStartLevel;
        ShowPortals();
    }

    private void ShowPortals()
    {
        foreach (var item in portalClassList)
        {
            MapManager.instance.ShowPortal(item.PortalScript.gameObject);
        }
    }

    private void OnStartLevel()
    {
        if (isStartRoom)
        {
            ShowPortals();
        }
    }
    public Transform GetTeleportPoint(SpawnType portalSide)
    {
        try
        {
            PortalClass portalClass = portalClassList.Single(x => x.WallIndex == portalSide);
            return portalClass.PortalScript.PortalPoint;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public PortalSO GetPortal(SpawnType portalSide)
    {
        try
        {
            PortalClass portalClass = portalClassList.Single(x => x.WallIndex == portalSide);
            return portalClass.PortalScript.PortalSO;
        }
        catch (Exception)
        {
            return null;
        }
    }

    private void Awake()
    {
        portalPrefab = Resources.Load("Portal") as GameObject;
    }


    public void SetEssentials(RoomSO roomSO, Dictionary<SpawnType, bool> portalsDict, SpawnType? entryPortal = null)
    {
        this.portalsDict = portalsDict;
        this.portalSO = roomSO.Portal;
        this.portalsDict = portalsDict;
        this.entryPortal = entryPortal;
        isStartRoom = roomSO is StartRoomSO;

        pointA = new Vector3(-1f * gameObject.transform.localScale.x, 0f, 1f * gameObject.transform.localScale.z);
        pointB = new Vector3(1f * gameObject.transform.localScale.x, 0f, 1f * gameObject.transform.localScale.z);
        pointC = new Vector3(1f * gameObject.transform.localScale.x, 0f, -1f * gameObject.transform.localScale.z);
        pointD = new Vector3(-1f * gameObject.transform.localScale.x, 0f, -1f * gameObject.transform.localScale.z);

        pointAx = pointA + new Vector3(colliderSize.x, 0f, 0f);
        pointBx = pointB - new Vector3(colliderSize.x, 0f, 0f);
        pointCx = pointC - new Vector3(colliderSize.x, 0f, 0f);
        pointDx = pointD + new Vector3(colliderSize.x, 0f, 0f);

        pointAz = pointA - new Vector3(0f, 0f, colliderSize.x);
        pointBz = pointB - new Vector3(0f, 0f, colliderSize.x);
        pointCz = pointC + new Vector3(0f, 0f, colliderSize.x);
        pointDz = pointD + new Vector3(0f, 0f, colliderSize.x);


        if (gameObject.TryGetComponent(out RoomPortalSpots portalSpots))
        {

            foreach (var item in portalsDict)
            {
                if (!item.Value)
                {
                    continue;
                }

                if (portalSpots.TryGetPortalSpots(item.Key, out List<Transform> spotsTransformList))
                {
                    SpawnPortalInRandomSpot(spotsTransformList, item.Key);
                }
                else
                {
                    SpawnPortalRandomlyInWall(item.Key);
                }

            }
        }
        else
        {

            foreach (var item in portalsDict)
            {
                SpawnPortalRandomlyInWall(item.Key);
            }
        }
    }

    private void SpawnPortalInRandomSpot(List<Transform> portalSpots, SpawnType wallIndex)
    {
        var rnd = new System.Random();
        Transform tr = portalSpots.OrderBy(i => rnd.Next()).First();
        AddPortal(tr.position, wallIndex, true);
    }

    private void SpawnPortalRandomlyInWall(SpawnType wallIndex)
    {
        if (!portalsDict[wallIndex])
        {
            return;
        }
        
        Vector3 pos = Vector3.zero;
        float positionInCurrentWall = 0;

        switch (wallIndex)
        {
            case SpawnType.NORTH:
                positionInCurrentWall = GetAxisPos(pointAx.x, pointBx.x);
                pos = new Vector3(positionInCurrentWall, 0f, pointA.z);
                break;
            case SpawnType.EAST:
                positionInCurrentWall = GetAxisPos(pointCz.z, pointBz.z);
                pos = new Vector3(pointB.x, 0f, positionInCurrentWall);
                break;
            case SpawnType.SOUTH:
                positionInCurrentWall = GetAxisPos(pointCx.x, pointDx.x);
                pos = new Vector3(positionInCurrentWall, 0f, pointC.z);
                break;
            case SpawnType.WEST:
                positionInCurrentWall = GetAxisPos(pointDz.z, pointAz.z);
                pos = new Vector3(pointD.x, 0f, positionInCurrentWall);
                break;
            default:
                pos = new Vector3(0f, 0f, 0f);
                break;
        }

        AddPortal(pos, wallIndex);
    }
    

    private void AddPortal(Vector3 portalPosition, SpawnType wallIndex, bool worldPosition = false)
    {
        Quaternion rot = wallIndex switch
        {
            SpawnType.NORTH => Quaternion.Euler(new Vector3(0f, 0f, 0f)),
            SpawnType.EAST => Quaternion.Euler(new Vector3(0f, 90f, 0f)),
            SpawnType.SOUTH => Quaternion.Euler(new Vector3(0f, 180, 0f)),
            SpawnType.WEST => Quaternion.Euler(new Vector3(0f, 270f, 0f)),
            _ => Quaternion.Euler(new Vector3(0f, 0f, 0f))
        };

        GameObject door = Instantiate(portalPrefab);

        if (worldPosition)
        {
            door.transform.SetPositionAndRotation(portalPosition, rot);
            door.transform.SetParent(gameObject.transform);
        }
        else
        {
            door.transform.SetParent(gameObject.transform);
            door.transform.SetLocalPositionAndRotation(portalPosition, rot);
        }

        if (entryPortal == wallIndex)
        {
            portalSO = new("Back Portal", "");
        }


        Portal portalScript = door.GetComponentInChildren<Portal>();
        portalScript.SetDoor(portalSO, wallIndex);
        GameController.AddPortal(portalScript.gameObject);
        PortalClass doorClass = new PortalClass(wallIndex, portalScript);
        portalClassList.Add(doorClass);
    }

    private float GetAxisPos(float minValue, float maxValue)
    {
        if (maxValue <= minValue)
        {
            return 0;
        }

        System.Random random = new();
        return (float)random.NextDouble() * (maxValue - minValue) + minValue;
    }
    private void OnDisable()
    {
        GameEvents.instance.OnStartLevel -= OnStartLevel;
    }
}
