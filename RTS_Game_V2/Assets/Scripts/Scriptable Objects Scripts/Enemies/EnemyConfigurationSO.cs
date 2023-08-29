using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEnemyConfiguration", menuName = "Scriptable Objects/Enemy Configuration")]
public class EnemyConfigurationSO : ScriptableObject
{
    [Header("Main section")]
    [SerializeField] private float health;

    [Header("Movement section")]
    [SerializeField] private float movementSpeed;
    [Tooltip("Minimum time (in seconds) object waits to move when is not triggered by target")]
    [SerializeField] private int minMoveInterval;
    [Tooltip("Maximum time (in seconds) object waits to move when is not triggered by target")]
    [SerializeField] private int maxMoveInterval;

    [Header("Deff section")]
    [Tooltip("Armor reduce physical damages acording: Damage Multiplier = 100/(100 - Armor)")]
    [SerializeField] private float armor;
    [Tooltip("Magic Resistance reduce magic damages acording: Damage Multiplier = 100/(100 - Magic Resistance)")]
    [SerializeField] private float magicResistance;

    [Header("Attack section")]
    [Tooltip("Attacks per second")]
    [SerializeField] private float attackSpeed;
    [Tooltip("Minimum Distance between target to attack")]
    [SerializeField] private float attackRange;
    [SerializeField] private float physicalDamage;
    [SerializeField] private float magicDamage;

    public float Helath { get => health; }
    public float MovementSpeed { get => movementSpeed; }
    public int MinMoveInterval { get => minMoveInterval; }
    public int MaxMoveInterval { get => maxMoveInterval; }
    public float Armor { get => armor; }
    public float MagicResistance { get => magicResistance; }
    public float AttackSpeed { get => attackSpeed; }
    public float AttackRange { get => attackRange; }
    public float PhysicalDamage { get => physicalDamage; }
    public float MagicDamage { get => magicDamage; }
}
