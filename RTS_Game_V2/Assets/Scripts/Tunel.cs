using UnityEngine;

public class Tunel : MonoBehaviour
{
    [SerializeField] private Transform destination;
    [SerializeField] private BoxCollider destinationBoxTrigger;

    private Bounds collBounds;

    private void Awake()
    {
        collBounds = destinationBoxTrigger.bounds;
        destinationBoxTrigger.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerMovement playerMov))
        {
            if (collBounds.Contains(playerMov.CurrentDestination))
            {

                playerMov.MoveTo(destination.position);
            }
            else
            {
                playerMov.MoveTo(playerMov.CurrentDestination);
            }
        }
    }
}
