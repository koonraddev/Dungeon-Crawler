using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navAgent;
    [Tooltip("Object make some moves when is not triggered by target")]
    [SerializeField] private bool randomMovement;

    private int minMoveInterval;
    private int maxMoveInterval;

    private float randInterval;

    private float spawnPlaneSizeX;
    private float spawnPlaneSizeZ;
    private Vector3 planePos;
    System.Random rand;
    private void Awake()
    {
        rand = new System.Random();
    }

    private void OnEnable()
    {
        gameObject.transform.position = GetRandomPosition();
        randInterval = rand.Next(minMoveInterval, maxMoveInterval);
    }
    void Update()
    {
        if (randomMovement && navAgent.velocity == Vector3.zero)
        {
            randInterval -= Time.deltaTime;
            if(randInterval <= 0)
            {
                MoveTo(GetRandomPosition());
                randInterval = rand.Next(minMoveInterval, maxMoveInterval);
            }
        }           
    }


    public void SetEnemyMovement(float movementSpeed,int minMoveInterval ,int maxMoveInterval, GameObject parentRoom)
    {
        navAgent.speed = movementSpeed;
        this.minMoveInterval = minMoveInterval;
        this.maxMoveInterval = maxMoveInterval;

        MeshCollider mColl = parentRoom.GetComponent<MeshCollider>();
        spawnPlaneSizeX = mColl.bounds.size.x;
        spawnPlaneSizeZ = mColl.bounds.size.z;
        planePos = parentRoom.transform.position;
    }

    public void MoveTo(Vector3 destination)
    {
        navAgent.SetDestination(destination);
    }
    public void StopMovement()
    {
        navAgent.isStopped = true;
    }

    private Vector3 GetRandomPosition()
    {
        float spawnPosX = Random.Range(1, spawnPlaneSizeX / 2 - 1) * (Random.Range(0, 2) * 2 - 1);
        float spawnPosZ = Random.Range(1, spawnPlaneSizeZ / 2 - 1) * (Random.Range(0, 2) * 2 - 1);
        Vector3 randomPos = new(spawnPosX, planePos.y, spawnPosZ);
        randomPos += planePos;
        return randomPos;
    }
}
