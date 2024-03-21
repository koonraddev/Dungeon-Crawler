using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "newEnemyConfiguration", menuName = "Scriptable Objects/Enemy/Enemy Configuration", order = 2)]
public class EnemyConfigurationSO : ScriptableObject
{
    [Header("Main section")]
    [SerializeField] private string enemyName;
    [SerializeField] private LootSO enemyLoot;
    [SerializeField] private Sprite enemySprite;
    [SerializeField] private bool boss;
    [SerializeField] private GameObject enemyPrefab;

    [Header("Statistics Section")]
    [SerializeField] private StatisticsSet statisticsSet;

    [Header("Movement section")]
    [Tooltip("Minimum time (in seconds) object waits to move when is not triggered by target")]
    [SerializeField] private int minMoveInterval;
    [Tooltip("Maximum time (in seconds) object waits to move when is not triggered by target")]
    [SerializeField] private int maxMoveInterval;
    [Tooltip("Minimum Distance between target to trigger object")]
    [SerializeField] private float triggerRange;
    [SerializeField] private bool projectileAttack;
    [SerializeField] [HideInInspector] private GameObject projectilePrefab;


    public string EnemyName { get => enemyName; }
    public float Health { get => statisticsSet.maxHealth; }
    public LootSO Loot { get => enemyLoot; }
    public Sprite Sprite { get => enemySprite; }
    public bool Boss { get => boss; }
    public float MovementSpeed { get => statisticsSet.movementSpeed; }
    public int MinMoveInterval { get => minMoveInterval; }
    public int MaxMoveInterval { get => maxMoveInterval; }
    public float Armor { get => statisticsSet.armor; }
    public float MagicResistance { get => statisticsSet.magicResistance; }
    public float AttackSpeed { get => statisticsSet.attackSpeed; }
    public float AttackRange { get => statisticsSet.attackRange; }
    public float TriggerRange { get => triggerRange; }
    public float PhysicalDamage { get => statisticsSet.physicalDamage; }
    public float MagicDamage { get => statisticsSet.magicDamage; }
    public float TrueDamage { get => statisticsSet.trueDamage; }
    public bool ProjectileAttack { get => projectileAttack; }
    public GameObject ProjectilePrefab 
    { 
        get
        {
            if (projectileAttack)
            {
                return projectilePrefab;
            }
            return null;
        }
    }
    public GameObject EnemyPrefab { get => enemyPrefab; }

#if UNITY_EDITOR
    [CustomEditor(typeof(EnemyConfigurationSO))]
    public class EnemyConfigurationSOEditor : Editor
    {
        private EnemyConfigurationSO enemyConfigSO;

        private void OnEnable()
        {
            enemyConfigSO = (EnemyConfigurationSO)target;
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (enemyConfigSO.projectileAttack)
            {
                enemyConfigSO.projectilePrefab = (GameObject)EditorGUILayout.ObjectField("Projectile Prefab", enemyConfigSO.projectilePrefab, typeof(GameObject), true);
            }

            EditorUtility.SetDirty(enemyConfigSO);
        }
    }
#endif
}
