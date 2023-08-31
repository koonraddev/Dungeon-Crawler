using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private bool manySpawns;
    private bool waitForAll;
    private float spawnerInterval;
    private List<GameObject> enemyList;
    private GameObject parentRoom;

    private float timeLeft;
    private List<GameObject> enemyPool;

    private void Awake()
    {
        enemyPool = new();
    }
    void Start()
    {
        CreateObjects();
        timeLeft = spawnerInterval;
    }

    void Update()
    {
        if (manySpawns)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {
                if (waitForAll)
                {
                    if (AllObjectsAreUnactive())
                    {
                        ActivateObjects();
                        timeLeft = spawnerInterval;
                    }
                }
                else
                {
                    ActivateObjects();
                    timeLeft = spawnerInterval;
                }
            }
        }
    }

    public void SetSpawner(EnemySpawnerConfigurationSO spawnerConfigs, GameObject parentRoom)
    {
        this.parentRoom = parentRoom;
        manySpawns = spawnerConfigs.ManySpawns;
        waitForAll = spawnerConfigs.WaitForAll;
        spawnerInterval = spawnerConfigs.SpawnerInterval;
        enemyList = spawnerConfigs.EnemyList;
    }

    private void ActivateObjects()
    {
        List<GameObject> enemiesToActivate = UnactiveObjects();
        foreach (GameObject enemy in enemiesToActivate)
        {
            enemy.SetActive(true);
        }
    }

    private bool AllObjectsAreUnactive()
    {
        foreach (GameObject enemy in enemyPool)
        {
            if (enemy.activeInHierarchy)
            {
                return false;
            }
        }
        return true;
    }

    private List<GameObject> UnactiveObjects()
    {
        List<GameObject> pool = new();
        foreach (GameObject enemy in enemyPool)
        {
            if (!enemy.activeInHierarchy)
            {
                pool.Add(enemy);
            }
        }
        return pool;
    }

    private void CreateObjects()
    {
        GameObject tmp;
        foreach (GameObject enemy in enemyList)
        {

            tmp = Instantiate(enemy,transform.position,transform.rotation);
            tmp.GetComponent<Enemy>().SetEnemy(parentRoom);
            tmp.SetActive(true);
            enemyPool.Add(tmp);
        }
    }
}
