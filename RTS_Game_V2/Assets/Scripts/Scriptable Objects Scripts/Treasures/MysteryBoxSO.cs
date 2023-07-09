using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryBoxSO : TreasureSO
{
    [SerializeField] private string nameText;
    [SerializeField] private string description;
    [SerializeField] private Sprite mysteryBoxThumbnail;
    [SerializeField] private int mysteriesAmount;

    public override string GetDescription()
    {
        return description;
    }

    public override string GetName()
    {
        return nameText;
    }

    public override Sprite GetThumbnail()
    {
        return mysteryBoxThumbnail;
    }

    public override void TreasureBehavoiur()
    {
        throw new System.NotImplementedException();
    }
}
