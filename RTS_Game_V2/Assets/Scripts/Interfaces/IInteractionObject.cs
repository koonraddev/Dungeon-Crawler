using UnityEngine;

public interface IInteractionObject : IContentDisplayObject
{
    public void ObjectInteraction(GameObject interactingObject = null);
    public void DoInteraction();
    public void OnMouseEnterObject(Color highLightColor);
    public void OnMouseExitObject();
    public GameObject GameObject { get; }
    public int InteractionDistance { get; }
}
