using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject playerInventoryUI;
    [SerializeField] private GameObject inventoryInfoPanel;

    public GameObject PlayerInventoryUI { get { return playerInventoryUI; } }
    public GameObject InventoryInfoPanel { get { return inventoryInfoPanel; } }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            playerInventoryUI.SetActive(!playerInventoryUI.activeSelf);
            inventoryInfoPanel.SetActive(!inventoryInfoPanel.activeSelf);
        }
    }
}
