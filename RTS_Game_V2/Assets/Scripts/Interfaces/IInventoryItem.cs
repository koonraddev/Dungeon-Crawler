using UnityEngine.UI;

public interface IInventoryItem
{
    public string NameText { get; set; }
    public int Id { get; set; }
    public string Description { get; set; }
    public bool IsStackable { get; set; }
    public Image inventoryTexture { get; set; }
}
