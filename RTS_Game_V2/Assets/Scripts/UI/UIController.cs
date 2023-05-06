using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject playerInventoryUI;
    [SerializeField] private GameObject inventoryInfoPanel;

    public GameObject PlayerInventoryUI { get { return playerInventoryUI; } }
    public GameObject InventoryInfoPanel { get { return inventoryInfoPanel; } }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Tab))
        {
            playerInventoryUI.SetActive(true);
            inventoryInfoPanel.SetActive(true);
        }
        else
        {
            playerInventoryUI.SetActive(false);
            inventoryInfoPanel.SetActive(false);
        }
    }
}
