using System.Collections;
using UnityEngine;


[CreateAssetMenu(fileName = "newArmor", menuName = "Scriptable Objects/Treasure/Armor", order = 2)]
public class ArmorSO : TreasureSO
{
    [SerializeField] private string nameText;
    [SerializeField] private string description;
    [SerializeField] private float armor;
    [SerializeField] private float magicResistance;
    [SerializeField] private Sprite armorThumbnail;


    //public string NameText { get => nameText; }
    //public string Descritpion { get => description; }
    //public int Armor { get => armor; }
    //public int MagicResistance { get => magicResistance; }
    //public Sprite ArmorThumbnail { get => armorThumbnail; }

    public override void TreasureBehavoiur()
    {
        //zamienia zbroje w inventory na ta
    }

    public override string GetName()
    {
        return nameText;
    }

    public override string GetDescription()
    {
        return description;
    }
    
    public float GetArmor()
    {
        return armor;
    }
    public float GetMagicResistance()
    {
        return magicResistance;
    }

    public Sprite GetArmorThumbnail()
    {
        return armorThumbnail;
    }

    public override Sprite GetThumbnail()
    {
        return armorThumbnail;
    }
}
