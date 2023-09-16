using UnityEngine;

[CreateAssetMenu(fileName = "newKey", menuName = "Scriptable Objects/Treasure/Key", order = 1)]
public class KeySO: TreasureSO, IInventoryItem
{
    [field: SerializeField] public string NameText { get; set; }
    [field: SerializeField] public int Id { get; set; }
    [field: SerializeField] public string Description { get; set; }
    [field: SerializeField] public bool IsStackable { get; set; }
    [field: SerializeField] public bool IsReusable { get; set; }
    [field: SerializeField] public Sprite InventoryThumbnail { get; set; }
    [field: SerializeField] public string KeyLevel { get; set; }

    public override string GetDescription()
    {
        return Description;
    }

    public override string GetName()
    {
        return NameText;
    }

    public override Sprite GetThumbnail()
    {
        return InventoryThumbnail;
    }
}