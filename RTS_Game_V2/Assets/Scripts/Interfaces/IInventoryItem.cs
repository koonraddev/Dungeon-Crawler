using UnityEngine;

public interface IInventoryItem
{
    public string NameText { get; set; }
    public int Id { get; set; }
    public string Description { get; set; }
    public bool IsStackable { get; set; }
    public Sprite InventoryTexture { get; set; }
    public bool IsReusable { get; set; }
}
