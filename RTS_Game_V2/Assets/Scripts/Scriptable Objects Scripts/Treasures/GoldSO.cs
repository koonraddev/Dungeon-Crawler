using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newGold", menuName = "Scriptable Objects/Treasure/Gold", order = 4)]
public class GoldSO : TreasureSO
{
    [SerializeField] private string nameText;
    [SerializeField] private int goldAmount;
    [SerializeField] private string description;
    [SerializeField] private Sprite goldThumbnail;
    public override void TreasureBehavoiur()
    {
        Inventory.Instance.AddGold(goldAmount);
        Destroy(this);
    }

    public int GetGoldAmount()
    {
        return goldAmount;
    }

    public override string GetName()
    {
        return nameText;
    }

    public override Sprite GetThumbnail()
    {
        return goldThumbnail;
    }

    public override string GetDescription()
    {
        return description;
    }
}
