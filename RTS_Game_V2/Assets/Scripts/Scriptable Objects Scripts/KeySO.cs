using UnityEngine;

[CreateAssetMenu(fileName = "newKey", menuName = "Scriptable Objects/Item/Key", order = 2)]
public class KeySO: ScriptableObject, IInventoryItem
{
    [field: SerializeField] public string NameText { get; set; }
    [field: SerializeField] public int Id { get; set; }
    [field: SerializeField] public string Description { get; set; }
    [field: SerializeField] public string KeyLevel { get; set; }
    [field: SerializeField] public bool IsStackable { get; set; }
    [field: SerializeField] public bool IsReusable { get; set; }
    [field: SerializeField] public Sprite InventoryTexture { get; set; }
}