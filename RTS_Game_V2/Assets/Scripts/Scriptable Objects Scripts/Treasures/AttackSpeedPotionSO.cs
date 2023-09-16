using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newAttackSpeedPotion", menuName = "Scriptable Objects/Treasure/Potion/Attack Speed Potion", order = 2)]
public class AttackSpeedPotionSO : TreasureSO, IInventoryItem
{
    [field: SerializeField] public string NameText { get; set; }
    [field: SerializeField] public int Id { get; set; }
    [field: SerializeField] public string Description { get; set; }
    [field: SerializeField] public bool IsStackable { get; set; }
    [field: SerializeField] public Sprite InventoryThumbnail { get; set; }
    [field: SerializeField] public bool IsReusable { get; set; }
    [field: SerializeField] public int attackSpeedPoints { get; set; }

    public override string GetName()
    {
        return NameText;
    }

    public override Sprite GetThumbnail()
    {
        return InventoryThumbnail;
    }

    public override string GetDescription()
    {
        return Description;
    }
}
