using System;
using UnityEngine;

[System.Serializable]
public class Item : IEquatable<Item>
{
    [SerializeField] protected int itemID;
    [SerializeField] protected string itemName;
    [SerializeField] protected string itemDescription;
    [SerializeField] protected Sprite itemSprite;

    public virtual Sprite Sprite
    {
        get { return itemSprite; }
        set { itemSprite = value; }
    }

    public virtual int ID
    {
        get { return itemID; }
        set { itemID = value; }
    }

    public virtual string Name
    {
        get { return itemName; }
        set { itemName = value; }
    }

    public virtual string Description
    {
        get { return itemDescription; }
        set { itemDescription = value; }
    }

    public Item(ItemInformationsSO itemInformationsSO, int id)
    {
        itemID = id;
        itemName = itemInformationsSO.ItemName;
        itemDescription = itemInformationsSO.ItemDescription;
        itemSprite = itemInformationsSO.ItemSprite;
    }

    public bool Equals(Item other)
    {
        if(other == null)
        {
            return false;
        }

        return this.itemID == other.itemID;
    }
}