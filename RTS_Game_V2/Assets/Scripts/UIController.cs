using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    public InventorySO playerInv;
    public TMP_Text textTMP;
    List<ItemSO> invList;
    void Start()
    {
        invList = playerInv.GetInventory();
        textTMP.text = "";
    }

    // Update is called once per frame
    public void UpdateInventory()
    {
        invList = playerInv.GetInventory();
        textTMP.text = "SIEMA";
        foreach(ItemSO item in invList)
        {
            textTMP.text += item.Description;
        }
    }
}
