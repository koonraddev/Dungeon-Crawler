using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField]RoomFader roomFader;

    private void OnTriggerEnter(Collider other)
    {
        if(roomFader != null)
        {
            if (other.gameObject.CompareTag("RoomPlane"))
            {
                roomFader.DestinationObject = other.gameObject;
                gameObject.SetActive(false);
            }
        }
    }
}
