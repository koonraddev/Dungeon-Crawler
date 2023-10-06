using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newItemInformations", menuName = "Scriptable Objects/Item Informations")]
public class ItemInformationsSO : ScriptableObject
{
    [SerializeField] private int itemID;
    [SerializeField] private string itemName;
    [SerializeField] private string itemDescription;
    [SerializeField] private Sprite itemSprite;
    public int ItemID { get => itemID; }
    public string ItemName { get => itemName; }
    public string ItemDescription { get => itemDescription; }
    public Sprite ItemSprite { get => itemSprite; }
}
