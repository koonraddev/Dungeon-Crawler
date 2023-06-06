using UnityEngine;
using System.Collections.Generic;

public interface IInteractionObjects : IContentDisplayObject
{
    public void ObjectInteraction();
    public void DoInteraction();
    public void OnMouseEnterObject(Color highLightColor);
    public void OnMouseExitObject();
    public GameObject GetGameObject();
}
