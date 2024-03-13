using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private bool manySpawns;
    private bool waitForAll;

    private List<GameObject> enemyList;
    private GameObject parentRoom;

    private float spawnerInterval, timeLeft, frozenTime;
    private List<GameObject> enemyPool;

    private void Awake()
    {
        enemyPool = new();
        enemyList = new();
    }
    void Start()
    {
        //CreateObjects();
        timeLeft = spawnerInterval;
    }

    private void OnEnable()
    {
        float timePassedWhileInactiveState = Time.time - frozenTime;
        timeLeft -= timePassedWhileInactiveState;
        if (timeLeft <= 0)
        {
            //Debug.Log("enabled and ready");
            TimePassed();
        }
    }

    void Update()
    {
        if (manySpawns)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {
                TimePassed();
            }
        }
    }

    private void TimePassed()
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

    public void SetSpawner(EnemySpawnerConfigurationSO spawnerConfigs, GameObject parentRoom)
    {
        this.parentRoom = parentRoom;
        manySpawns = spawnerConfigs.ManySpawns;
        waitForAll = spawnerConfigs.WaitForAll;
        spawnerInterval = spawnerConfigs.SpawnerInterval;

        foreach (var item in spawnerConfigs.EnemyList)
        {
            if(item.EnemyPrefab.TryGetComponent(out Enemy enemy))
            {
                GameObject newEnemy = Instantiate(item.EnemyPrefab, transform.position,Quaternion.identity);
                newEnemy.transform.SetParent(parentRoom.transform);
                newEnemy.SetActive(false);
                enemy = newEnemy.GetComponent<Enemy>();
                enemy.SetEnemy(item, parentRoom);
                enemyPool.Add(newEnemy);
            }
        }
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
            tmp.transform.SetParent(parentRoom.transform);
            tmp.SetActive(true);
            enemyPool.Add(tmp);
        }
    }

    private void OnDisable()
    {
        frozenTime = Time.time;
    }
}
