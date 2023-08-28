using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class NavigationController : MonoBehaviour
{

    public static NavigationController instance;

    private List<NavMeshSurface> surfacesList;

    private void Awake()
    {
        surfacesList = new List<NavMeshSurface>();
        instance = this;
    }
    public void AddNavSurface(NavMeshSurface surface)
    {
        if (!surfacesList.Contains(surface))
        {
            surfacesList.Add(surface);
        }
    }

    private void Start()
    {
        GameEvents.instance.OnLastRoomReady += LastRoomReady;
    }

    private void LastRoomReady()
    {
        int i = 1;
        foreach(NavMeshSurface surface in surfacesList)
        {
            Debug.Log("Build: " + i);
            i++;
            surface.BuildNavMesh();
        }
        Debug.Log("Done");
    }

    private void OnDisable()
    {
        GameEvents.instance.OnLastRoomReady -= LastRoomReady;
    }
}
