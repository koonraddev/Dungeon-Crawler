using UnityEngine;

[CreateAssetMenu(fileName = "newChest", menuName = "Scriptable Objects/Chest", order = 4)]
public class ChestSO : ScriptableObject
{
    [field: SerializeField] public string NameText { get; private set; }
    [field: SerializeField] public int Id { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public KeySO Treasure { get; set; }
}
