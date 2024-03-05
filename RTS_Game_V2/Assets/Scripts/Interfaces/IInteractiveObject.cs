using UnityEngine;

public interface IInteractiveObject : IContentDisplayObject
{
    public void ObjectInteraction(GameObject interactingObject = null);
    public void DoInteraction();
    public int InteractionDistance { get; }
}
