using UnityEngine;

[CreateAssetMenu(fileName = "newWeapon", menuName = "Scriptable Objects/Treasure/Weapon", order = 3)]
public class WeaponSO : TreasureSO
{
    [SerializeField] private string nameText;
    [SerializeField] private string description;
    [SerializeField] private int damage;
    [SerializeField] private int attackSpeed;
    [SerializeField] private Sprite weaponThumbnail;
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

    public int GetDamage()
    {
        return damage;
    }
    
    public int GetAttackSpeed()
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
