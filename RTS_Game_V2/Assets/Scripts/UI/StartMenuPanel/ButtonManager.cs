using UnityEngine;
using UnityEngine.UI;
public class ButtonManager : MonoBehaviour
{
    [SerializeField] private Button button;

    public void ActivateButton()
    {
        button.interactable = true;
    }

    public void DeactivateButton()
    {
        button.interactable = false;
    }
}
