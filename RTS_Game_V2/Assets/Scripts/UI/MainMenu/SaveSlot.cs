using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    [SerializeField] private bool loadingSlot;
    [SerializeField] private GameObject deleteButtonObj, selectButtonObj;
    [SerializeField] private int slotNumber;
    [SerializeField] private Color emptySlotColor, occupiedSlotColor;
    private Image selectButtonImage;
    private Button deleteButton, selectButton;

    private void Awake()
    {
        selectButton = selectButtonObj.GetComponent<Button>();
        selectButtonImage = selectButtonObj.GetComponent<Image>();
        deleteButton = deleteButtonObj.GetComponent<Button>();

        deleteButton.onClick.AddListener(DeleteSave);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Reset()
    {
        
    }

    public void SetEmptySlot()
    {
        deleteButtonObj.SetActive(false);
        selectButtonImage.color = emptySlotColor;

        if (!loadingSlot)
        {
            selectButton.onClick.AddListener(CreateSave);
        }
    }

    public void SetOccupiedSlot()
    {
        deleteButtonObj.SetActive(true);
        selectButtonImage.color = occupiedSlotColor;

        if (loadingSlot)
        {
            selectButton.onClick.AddListener(LoadSave);
        }
    }


    private void CreateSave()
    {

    }

    private void DeleteSave()
    {
        Reset();
    }

    private void LoadSave()
    {

    }
}
