using UnityEngine;

[CreateAssetMenu(fileName = "newDoor", menuName = "Scriptable Objects/Door")]
public class DoorSO : ScriptableObject
{
    [field: SerializeField] public string NameText { get; private set; }
    [field: SerializeField] public int Id { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public PassiveItemSO keyRequired { get; set; }
}
