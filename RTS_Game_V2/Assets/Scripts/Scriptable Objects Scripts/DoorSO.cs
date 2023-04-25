using UnityEngine;

[CreateAssetMenu(fileName = "newDoor", menuName = "Scriptable Objects/Door", order = 3)]
public class DoorSO : ScriptableObject
{
    [field: SerializeField] public string NameText { get; private set; }
    [field: SerializeField] public int Id { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public KeySO keyRequired { get; set; }
}
