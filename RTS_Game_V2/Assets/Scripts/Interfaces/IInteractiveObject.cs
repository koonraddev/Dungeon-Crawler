using UnityEngine;

public interface IInteractiveObject : IContentDisplayObject
{
    public void ObjectInteraction(GameObject interactingObject = null);
    public void DoInteraction();
    public GameObject GameObject { get; }
    public int InteractionDistance { get; }
}
