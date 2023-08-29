using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navAgent;
    [Tooltip("Object make some moves when is not triggered by target")]
    [SerializeField] private bool randomMovement;

    private int minMoveInterval;
    private int maxMoveInterval;

    private float randInterval;
    private bool isMoving;

    System.Random rand;
    private void Awake()
    {
        rand = new System.Random();
    }

    void Update()
    {
        if (randomMovement && navAgent.velocity == Vector3.zero)
        {
            randInterval -= Time.deltaTime;
            if(randInterval <= 0)
            {
                //Get Position in room
                Vector3 moveToPos = Vector3.zero;
                MoveTo(moveToPos);
                randInterval = rand.Next(minMoveInterval, maxMoveInterval);
            }
        }           
    }


    public void SetEnemyMovement(float movementSpeed,int minMoveInterval ,int maxMoveInterval)
    {
        navAgent.speed = movementSpeed;
        this.minMoveInterval = minMoveInterval;
        this.maxMoveInterval = maxMoveInterval;
    }

    public void MoveTo(Vector3 destination)
    {
        navAgent.SetDestination(destination);
    }
}
