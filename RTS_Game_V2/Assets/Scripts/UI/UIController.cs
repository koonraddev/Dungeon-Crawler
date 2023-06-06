using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject playerInventoryUI;
    [SerializeField] private GameObject inventoryInfoPanel;
    [SerializeField] private GameObject inventoryDeletePanel;
    [SerializeField] private GameObject inventoryDropPanel;

    public GameObject PlayerInventoryUI { get { return playerInventoryUI; } }
    public GameObject InventoryInfoPanel { get { return inventoryInfoPanel; } }
    public GameObject InventoryDeletePanel { get { return inventoryDeletePanel; } }
    public GameObject InventoryDropPanel { get { return inventoryDropPanel; } }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            playerInventoryUI.SetActive(!playerInventoryUI.activeSelf);
            inventoryInfoPanel.SetActive(!inventoryInfoPanel.activeSelf);
            inventoryDeletePanel.SetActive(!inventoryDeletePanel.activeSelf);
            inventoryDropPanel.SetActive(!inventoryDropPanel.activeSelf);
        }
    }
}
