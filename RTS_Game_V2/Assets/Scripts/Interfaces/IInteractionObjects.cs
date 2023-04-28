using UnityEngine;
using System.Collections.Generic;

public interface IInteractionObjects
{
    public void ObjectInteraction();
    public void DoInteraction();
    public void OnMouseEnterObject(Color highLightColor);
    public void OnMouseExitObject();
    public Dictionary<string, string> GetContentToDisplay();
    public GameObject GetGameObject();
}
