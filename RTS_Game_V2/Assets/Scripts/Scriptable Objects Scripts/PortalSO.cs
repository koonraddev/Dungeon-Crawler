using UnityEngine;

[CreateAssetMenu(fileName = "newPortal", menuName = "Scriptable Objects/Portal")]
public class PortalSO : ScriptableObject
{
    [field: SerializeField] public string NameText { get; private set; }
    [field: SerializeField] public int Id { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public PassiveItemSO keyRequired { get; set; }

    public PortalSO(string name, string description, PassiveItemSO requiredKey = null)
    {
        NameText = name;
        Id = 0;
        Description = description;
        keyRequired = requiredKey;
    }
}
