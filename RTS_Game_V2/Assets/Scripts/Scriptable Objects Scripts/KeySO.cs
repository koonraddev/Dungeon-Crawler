using UnityEngine;

[CreateAssetMenu(fileName = "newKey", menuName = "Scriptable Objects/Item/Key", order = 2)]
public class KeySO: ScriptableObject
{
    [field: SerializeField] public string NameText { get; private set; }
    [field: SerializeField] public int Id { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public string KeyLevel { get; private set; }
}
