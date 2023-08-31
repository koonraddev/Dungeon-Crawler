using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyConfigurationSO enemyConfig;
    [SerializeField] List<TreasureSO> treasureList;

    private GameObject parentRoom;

    private float maxHealth;
    private float health;
    private float movementSpeed;

    [SerializeField] EnemyMovement enemyMovement;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    void Start()
    {

    }

    public void SetEnemy(GameObject parentRoom)
    {
        this.parentRoom = parentRoom;
        gameObject.transform.SetParent(parentRoom.transform);
        if (enemyMovement != null)
        {
            enemyMovement.SetEnemyMovement(enemyConfig.MovementSpeed, enemyConfig.MinMoveInterval, enemyConfig.MaxMoveInterval, parentRoom);
        }
    }

    void Update()
    {

    }
}
