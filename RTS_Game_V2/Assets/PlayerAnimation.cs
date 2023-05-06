using UnityEngine;
using UnityEngine.AI;

public class PlayerAnimation : MonoBehaviour
{
    private NavMeshAgent playerAgent;
    private Animator playerAnimator;
    void Start()
    {
        playerAgent = gameObject.GetComponent<NavMeshAgent>();
        playerAnimator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerAgent.velocity == Vector3.zero)
        {
            playerAnimator.SetBool("isRunning", false);
        }
        else
        {
            playerAnimator.SetBool("isRunning", true);
        }
    }
}
