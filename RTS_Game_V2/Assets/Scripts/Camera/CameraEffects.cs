using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraEffects : MonoBehaviour
{
    GameObject endTeleport;
    GameObject startTeleport;
    [SerializeField] private Camera camera;
    [SerializeField] private CameraFollow cameraFollow;
    private void Awake()
    {

    }
    private void OnEnable()
    {
        GameEvents.instance.OnLastRoomReady += FindTeleportObject;
        GameEvents.instance.OnSavedPlayerData += SavedData;
        GameEvents.instance.OnStartLevel += StartLevel;
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    GameEvents.instance.StartLevel();
        //}

        if (Input.GetKeyDown(KeyCode.B))
        {
            GameEvents.instance.ActivateTeleport();
        }
    }

    private IEnumerator MoveToTeleport()
    {
        cameraFollow.enabled = false;
        Sequence seqShow = DOTween.Sequence()
            .Append(transform.DOMove(endTeleport.transform.position, 1f))
            .Join(camera.DOFieldOfView(120f, 1f));
        yield return seqShow.WaitForCompletion();
        GameEvents.instance.SwitchScene();
    }

    private void SavedData()
    {
        StartCoroutine(MoveToTeleport());
    }
    
    private void StartLevel()
    {
        StartCoroutine(MooveOut());
    }
    private IEnumerator MooveOut()
    {
        Sequence seqShow = DOTween.Sequence()
            .Append(camera.DOFieldOfView(cameraFollow.BaseFov, 3f))
            .Join(transform.DOLocalMove(cameraFollow.BaseLocalOffset, 2f)); 


        yield return seqShow.WaitForCompletion();
        cameraFollow.enabled = true;
    }


    private void FindTeleportObject()
    {
        endTeleport = GameObject.FindGameObjectWithTag("EndTeleport");
        startTeleport = GameObject.FindGameObjectWithTag("StartTeleport");
        cameraFollow.enabled = false;
        
        if(startTeleport != null)
        {
            transform.position = startTeleport.transform.position;
            camera.fieldOfView = 120f;
        }
    }

    private void OnDisable()
    {
        GameEvents.instance.OnLastRoomReady -= FindTeleportObject;
        GameEvents.instance.OnSavedPlayerData -= SavedData;
        GameEvents.instance.OnStartLevel -= StartLevel;
    }
}
