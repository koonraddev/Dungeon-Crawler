using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CreateAssetMenu(fileName = "newEnemySpanwerConfiguration", menuName = "Scriptable Objects/Enemy/Spawner Configuration",order = 1)]
public class EnemySpawnerConfigurationSO : ScriptableObject
{
    [SerializeField] private List<EnemyConfigurationSO> enemyList;
    [SerializeField] private bool manySpawns;
    [SerializeField] [HideInInspector] private float spawnerInterval;
    [SerializeField] [HideInInspector] private bool waitForAll;

    public List<EnemyConfigurationSO> EnemyList { get => enemyList; }
    public bool ManySpawns { get => manySpawns; }
    public float SpawnerInterval 
    { 
        get
        {
            if (manySpawns) return spawnerInterval;
            return 0;
        }
    }
    public bool WaitForAll 
    { 
        get
        {
            if (manySpawns) return waitForAll;
            return false;
        }
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(EnemySpawnerConfigurationSO))]
    private class EnemySpawnerConigurationSOEditor : Editor
    {
        private EnemySpawnerConfigurationSO enemySpawnerSO;

        private void OnEnable()
        {
            enemySpawnerSO = (EnemySpawnerConfigurationSO)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (enemySpawnerSO.manySpawns)
            {
                enemySpawnerSO.waitForAll = EditorGUILayout.Toggle("Wait for all", enemySpawnerSO.waitForAll);
                enemySpawnerSO.spawnerInterval = EditorGUILayout.FloatField("Spawner interval", enemySpawnerSO.spawnerInterval);
            }

            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }
    }
#endif
}
