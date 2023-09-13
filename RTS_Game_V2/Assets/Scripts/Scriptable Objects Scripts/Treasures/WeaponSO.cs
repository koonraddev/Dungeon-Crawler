using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newWeapon", menuName = "Scriptable Objects/Treasure/Weapon", order = 3)]
public class WeaponSO : TreasureSO, IEquipmentItem
{
    [SerializeField] private string nameText;
    [SerializeField] private string description;
    [SerializeField] private float physicalDamage;
    [SerializeField] private float magicDamage;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float attackRange;
    [SerializeField] private Sprite weaponThumbnail;
    private ItemSlotType slotType = ItemSlotType.WEAPON;
    private Dictionary<StatisticType, float> statistics;

    public ItemSlotType ItemSlotType { get => slotType; }
    public string NameText { get => nameText; }
    public string Description { get => description; }
    public Sprite EquipmentThumbnail { get => weaponThumbnail; }
    public Dictionary<StatisticType, float> Statistics { get => statistics; }

    private void Awake()
    {
        //Debug.Log("Awake");
        //statistics = new Dictionary<StatisticType, float>();
        //statistics.Add(StatisticType.PhysicalDamage, physicalDamage);
        //statistics.Add(StatisticType.MagicDamage, magicDamage);
        //statistics.Add(StatisticType.AttackSpeed, attackSpeed);
        //statistics.Add(StatisticType.AttackRange, attackRange);
    }

    private void OnValidate()
    {
        if (statistics == null)
        {
            statistics = new Dictionary<StatisticType, float>();
        }

        if (!statistics.ContainsKey(StatisticType.PhysicalDamage))
        {
            statistics.Add(StatisticType.PhysicalDamage, physicalDamage);
        }
        else
        {
            statistics[StatisticType.PhysicalDamage] = physicalDamage;
        }

        if (!statistics.ContainsKey(StatisticType.MagicDamage))
        {
            statistics.Add(StatisticType.MagicDamage, magicDamage);
        }
        else
        {
            statistics[StatisticType.MagicDamage] = magicDamage;
        }

        if (!statistics.ContainsKey(StatisticType.AttackSpeed))
        {
            statistics.Add(StatisticType.AttackSpeed, attackSpeed);
        }
        else
        {
            statistics[StatisticType.AttackSpeed] = attackSpeed;
        }

        if (!statistics.ContainsKey(StatisticType.AttackRange))
        {
            statistics.Add(StatisticType.AttackRange, attackRange);
        }
        else
        {
            statistics[StatisticType.AttackRange] = attackRange;
        }
        //Debug.Log("Validate");

        //statistics[StatisticType.PhysicalDamage] = physicalDamage;
        //statistics[StatisticType.MagicDamage] = magicDamage;
        //statistics[StatisticType.AttackSpeed] = attackSpeed;
        //statistics[StatisticType.AttackRange] = attackRange;
    }
    public override void TreasureBehavoiur()
    {
        //to do
    }


    public void OnEnable()
    {
        Debug.Log(statistics[StatisticType.PhysicalDamage]);
        Debug.Log(statistics[StatisticType.MagicDamage]);
        Debug.Log(statistics[StatisticType.AttackSpeed]);
        Debug.Log(statistics[StatisticType.AttackRange]);
    }

    public override string GetName()
    {
        return nameText;
    }

    public override string GetDescription()
    {
        return description;
    }

    public override Sprite GetThumbnail()
    {
        return weaponThumbnail;
    }
}
