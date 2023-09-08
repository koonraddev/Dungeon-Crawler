using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "newEnemySpanwerConfiguration", menuName = "Scriptable Objects/Enemy/Spawner Configuration",order = 1)]
public class EnemySpawnerConfigurationSO : ScriptableObject
{
    [SerializeField] private bool manySpawns;
    [SerializeField] private bool waitForAll;
    [SerializeField] private float spawnerInterval;
    [SerializeField] private List<GameObject> enemyList;

    public float SpawnerInterval { get => spawnerInterval; }
    public bool ManySpawns { get => manySpawns; }
    public List<GameObject> EnemyList { get => enemyList; }
    public bool WaitForAll { get => waitForAll; }
}
