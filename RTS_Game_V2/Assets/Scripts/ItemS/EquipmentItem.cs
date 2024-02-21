using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EquipmentItem : Item , IStatisticItem, ISerializationCallbackReceiver
{
    [SerializeField] private EquipmentSlotType itemSlot;
    private AttackType attackType;
    private bool isWeapon, projectileAttack;
    private GameObject weaponPrefab, projectilePrefab;
    private Dictionary<StatisticType, float> statistics;
    [SerializeField] private List<StatisticType> statTypes;
    [SerializeField] private List<float> statValues;

    public override int ID
    {
        get { return itemID; }
        set { itemID = value; }
    }

    public override string Name
    {
        get { return itemName; }
        set { itemName = value; }
    }

    public override string Description
    {
        get { return itemDescription; }
        set { itemDescription = value; }
    }
    public override Sprite Sprite
    {
        get { return itemSprite; }
        set { itemSprite = value; }
    }

    public AttackType AttackType { get => attackType; }
    public EquipmentSlotType ItemSlot { get => itemSlot; }
    public bool IsWeapon { get => isWeapon; }
    public bool ProjectileAttack { get => projectileAttack; }
    public GameObject WeaponPrefab { get => weaponPrefab; }
    public GameObject ProjectilePrefab { get => projectilePrefab; }
    public Dictionary<StatisticType, float> Statistics => statistics;

    public EquipmentItem(EquipmentItemSO equipmentItemSO): base(equipmentItemSO.ItemInformations, equipmentItemSO.ItemID)
    {
        attackType = equipmentItemSO.AttackType;
        isWeapon = equipmentItemSO.IsWeapon;
        weaponPrefab = equipmentItemSO.WeaponPrefab;
        projectileAttack = equipmentItemSO.ProjectileAttack;
        projectilePrefab = equipmentItemSO.ProjectilePrefab;
        itemID = equipmentItemSO.ItemID;
        itemName = equipmentItemSO.ItemInformations.ItemName;
        itemDescription = equipmentItemSO.ItemInformations.ItemDescription;
        itemSprite = equipmentItemSO.ItemInformations.ItemSprite;
        statistics = equipmentItemSO.ItemStatistics.Statistics;
        itemSlot = equipmentItemSO.ItemSlot;
    }

    public void OnBeforeSerialize()
    {
        if (statTypes == null)
        {
            statTypes = new();
        }

        if (statValues == null)
        {
            statValues = new();
        }

        if (statistics == null)
        {
            statistics = new();
        }

        statTypes.Clear();
        statValues.Clear();

        foreach (var item in statistics)
        {
            statTypes.Add(item.Key);
            statValues.Add(item.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        statistics = new Dictionary<StatisticType, float>();

        for (int i = 0; i < Mathf.Min(statTypes.Count, statValues.Count); i++)
        {
            statistics.Add(statTypes[i], statValues[i]);
        }
    }
}
