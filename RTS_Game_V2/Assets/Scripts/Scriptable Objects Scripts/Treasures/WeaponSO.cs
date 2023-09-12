using UnityEngine;

[CreateAssetMenu(fileName = "newWeapon", menuName = "Scriptable Objects/Treasure/Weapon", order = 3)]
public class WeaponSO : TreasureSO, IEquipmentItem
{
    [SerializeField] private string nameText;
    [SerializeField] private string description;
    [SerializeField] private float damage;
    [SerializeField] private float attackSpeed;
    [SerializeField] private Sprite weaponThumbnail;
    private ItemSlotType slotType = ItemSlotType.WEAPON;

    public ItemSlotType ItemSlotType { get => slotType; }
    public string NameText { get => nameText; }
    public string Description { get => description; }
    public Sprite EquipmentThumbnail { get => weaponThumbnail; }

    public override void TreasureBehavoiur()
    {
        //zamienia bron w inventory na ta
    }

    public override string GetName()
    {
        return nameText;
    }

    public override string GetDescription()
    {
        return description;
    }

    public float GetDamage()
    {
        return damage;
    }
    
    public float GetAttackSpeed()
    {
        return attackSpeed;
    }

    public Sprite GetWeaponThumbnail()
    {
        return weaponThumbnail;
    }

    public override Sprite GetThumbnail()
    {
        return weaponThumbnail;
    }
}
