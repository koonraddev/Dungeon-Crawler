using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyConfigurationSO enemyConf;
    [SerializeField] List<TreasureSO> treasureList;

    private float maxHealth;
    private float health;
    private float movementSpeed;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
