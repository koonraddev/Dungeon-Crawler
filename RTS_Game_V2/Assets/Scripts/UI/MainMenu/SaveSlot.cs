using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveSlot : MonoBehaviour
{
    [SerializeField] private bool loadingSlot;
    [SerializeField] private int slotNumber;

    [SerializeField] private NewCharacterPanel newCharacterPanel;
    [SerializeField] private SaveManager saveManager;
    [SerializeField] private SceneLoader sceneLoader;

    [SerializeField] private Color emptySlotColor, occupiedSlotColor;
    [SerializeField] private Image foreground;
    [Header("Buttons Section")]
    [SerializeField] private GameObject deleteButtonObj, selectButtonObj;

    [Header("Info Objects Section")]
    [SerializeField] private TMP_Text playerNameObj, floorObj, dateObj, timeObj;

    private Image selectButtonImage;
    private Button deleteButton, selectButton;

    private void Awake()
    {
        selectButton = selectButtonObj.GetComponent<Button>();
        selectButtonImage = selectButtonObj.GetComponent<Image>();
        deleteButton = deleteButtonObj.GetComponent<Button>();

        deleteButton.onClick.AddListener(DeleteSave);
    }

    private void OnEnable()
    {
        TryGetSave();
    }

    private void SetEmptySlot()
    {
        deleteButtonObj.SetActive(false);
        //selectButtonImage.color = emptySlotColor;

        playerNameObj.text = "-";
        floorObj.text = "-";
        dateObj.text = "///";
        timeObj.text = "-:-";
        foreground.color = emptySlotColor;
        //
        TMP_Text tmpText = selectButton.GetComponentInChildren<TMP_Text>();
        tmpText.text = "empty slot nr " + slotNumber;
        //
        if (!loadingSlot)
        {
            selectButton.onClick.AddListener(CreateSave);
        }
    }

    private void SetOccupiedSlot(PlayerData playerData)
    {
        deleteButtonObj.SetActive(true);
        selectButtonImage.color = occupiedSlotColor;

        playerNameObj.text = playerData.characterName;
        floorObj.text = playerData.levelCompleted.ToString();
        dateObj.text = playerData.dateTime.Date.ToString("dd/MM/yyyy");
        timeObj.text = playerData.dateTime.Date.ToString("HH:mm");
        foreground.color = occupiedSlotColor;

        //
        TMP_Text tmpText = selectButton.GetComponentInChildren<TMP_Text>();
        tmpText.text = "occupied slot nr " + slotNumber;
        //
        if (loadingSlot)
        {
            selectButton.onClick.AddListener(LoadSave);
        }
    }


    private void CreateSave()
    {
        newCharacterPanel.gameObject.SetActive(true);
        newCharacterPanel.SelectedSlot = slotNumber;
    }

    private void DeleteSave()
    {
        saveManager.DeleteSave(slotNumber);
        TryGetSave();
    }

    private void LoadSave()
    {
        saveManager.ChosenSlotIndex = slotNumber;
        sceneLoader.LoadDungeonScene();
    }

    private void TryGetSave()
    {
        if(saveManager.GetPlayerData(slotNumber,out PlayerData playerData))
        {
            SetOccupiedSlot(playerData);
        }
        else
        {
            SetEmptySlot();
        }
    }
}
