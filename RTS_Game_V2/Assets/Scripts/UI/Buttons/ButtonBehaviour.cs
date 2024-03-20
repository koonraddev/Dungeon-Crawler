using UnityEngine;

public class ButtonBehaviour : MonoBehaviour
{
    GameObject rootObject;
    private void Start()
    {
        rootObject = gameObject.transform.root.gameObject;
    }
    public void CloseMessageMenu()
    {
        rootObject.SetActive(false);
    }
}
