using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    DISTANCE,
    MELEE
}
[CreateAssetMenu(fileName = "newEnemyAttackConfiguration", menuName = "Scriptable Objects/Enemy/Enemy Attack Configuration", order = 3)]
public class EnemyAttackConfigurationSO : ScriptableObject
{

    [SerializeField] private AttackType attackType;
    [SerializeField] private float effectSpeed;
    [SerializeField] private GameObject effeectPrefab;

    public AttackType AttackType { get => attackType; }
    public float EffectSpeed { get => effectSpeed; }
    public GameObject EffectPrefab { get => effeectPrefab; }
}
