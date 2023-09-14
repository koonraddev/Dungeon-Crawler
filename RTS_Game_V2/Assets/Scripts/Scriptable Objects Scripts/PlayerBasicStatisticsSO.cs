using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerBasicStatistics", menuName = "Scriptable Objects/Player/Basic Statistics", order = 3)]
public class PlayerBasicStatisticsSO : ScriptableObject
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float armor;
    [SerializeField] private float magicResistance;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float attackRange;
    [SerializeField] private float physicalDamage;
    [SerializeField] private float magicDamage;
    [SerializeField] private float trueDamage;
    [SerializeField] private float healthPointsRegen;
    [SerializeField] private float healthPercentsRegen;
    [SerializeField] private float movementSpeed;

    public float MaxHealth { get => maxHealth; }
    public float Armor { get => armor; }
    public float MagicResistance { get => magicResistance; }
    public float AttackSpeed { get => attackSpeed; }
    public float AttackRange { get => attackRange; }
    public float PhysicalDamage { get => physicalDamage; }
    public float MagicDamage { get => magicDamage; }
    public float TrueDamage { get => trueDamage; }
    public float HealthPointsRegen { get => healthPointsRegen; }
    public float HealthPercentsRegen { get => healthPercentsRegen; }
    public float MovementSpeed { get => movementSpeed; }
}
