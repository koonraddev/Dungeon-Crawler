using System.Collections;
using UnityEngine;


[CreateAssetMenu(fileName = "newArmor", menuName = "Scriptable Objects/Treasure/Armor", order = 2)]
public class ArmorSO : TreasureSO
{
    [SerializeField] private string nameText;
    [SerializeField] private string description;
    [SerializeField] private int armor;
    [SerializeField] private Sprite armorThumbnail;
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
    
    public int GetArmor()
    {
        return armor;
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
