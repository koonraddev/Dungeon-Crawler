using UnityEngine;

public delegate void YesButtonDelegate();
public struct ObjectContent
{
    public string Nametext { get; set; }
    public string Description { get; set; }
    public string Message { get; set; }
    public GameObject RequestingGameObject { get; }
    public YesButtonDelegate YesButtonDelegate { get; set; }

    public ObjectContent(GameObject requestingGameObject) : this()
    {
        RequestingGameObject = requestingGameObject;
    }
}
