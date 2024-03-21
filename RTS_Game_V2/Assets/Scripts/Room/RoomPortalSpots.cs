using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class PortalSpot
{
    [SerializeField] private Transform spotTransform;
    [SerializeField] private SpawnType spotCategory;

    public Transform SpotTransform { get => spotTransform; }
    public SpawnType SpotCategory { get => spotCategory; }
}
public class RoomPortalSpots : MonoBehaviour
{
    [SerializeField] private List<PortalSpot> portalSpots;

    public bool TryGetPortalSpots(SpawnType spotCategory, out List<Transform> spotsTransformList)
    {
        spotsTransformList = new();

        if(portalSpots == null)
        {
            return false;
        }

        spotsTransformList = portalSpots.Where(spot => spot.SpotCategory == spotCategory).Select(spot => spot.SpotTransform).OrderBy(spot => Guid.NewGuid()).ToList();

        return spotsTransformList.Count > 0;
    }
}
