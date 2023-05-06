using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    //private Animator anim;
    private LayerMask groundMask;
    private NavMeshAgent playerAgent;
    void Start()
    {
        playerAgent = gameObject.GetComponent<NavMeshAgent>();
        groundMask = LayerMask.NameToLayer("Ground");
        //anim = gameObject.GetComponent<Animator>();
    }
    void Update()
    {

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitPoint;
            if (Physics.Raycast(ray, out hitPoint, Mathf.Infinity))
            {
                if (hitPoint.collider.gameObject.layer == groundMask)
                {
                    MoveTo(hitPoint.point);
                }
            }
        }

    }

    public void MoveTo(Vector3 destination) //Move player to the set destination
    {
        //SoundManager.PlaySound(pointDestinationSound, 1f);
        playerAgent.SetDestination(destination);
    }
}
