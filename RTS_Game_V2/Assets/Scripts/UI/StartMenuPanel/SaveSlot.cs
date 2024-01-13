using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SaveSlot : MonoBehaviour
{
    [SerializeField] private bool loadingSlot;
    [SerializeField] private int slotNumber;

    [SerializeField] private GameObject foregroundMask;
    [SerializeField] private TMP_Text slotNumberObj;

    [Header("Buttons Section")]
    [SerializeField] private Button deleteButton, selectButton;

    [Header("Ask Panel Section")]
    [SerializeField] private GameObject askPanel;
    [SerializeField] private Button askPanelYesButton;
    [SerializeField] private TMP_Text askPanelInfo;

    [Header("Info Objects Section")]
    [SerializeField] private TMP_Text playerNameObj;
    [SerializeField] private TMP_Text floorObj;
    [SerializeField] private TMP_Text dateObj;
    [SerializeField] private TMP_Text timeObj;

    [Header("new section")]
    [SerializeField] private NewCharacterPanel newCharacterPanel;

    [Header("load section")]
    [SerializeField] Button loadButton;
    private ButtonManager loadButtonManager;
    private void Awake()
    {
        if(loadButton != null)
        {
            loadButtonManager = loadButton.gameObject.GetComponent<ButtonManager>();
        }
        slotNumberObj.text = slotNumber.ToString();
    }

    private void OnEnable()
    {
        if (loadButtonManager != null)
        {
            loadButtonManager.DeactivateButton();
        }
        TryGetSave();
    }

    private void SetEmptySlot()
    {
        deleteButton.gameObject.SetActive(false);

        playerNameObj.text = "-";
        floorObj.text = "-";
        dateObj.text = "///";
        timeObj.text = "-:-";


        if (loadingSlot)
        {
            foregroundMask.SetActive(true);
        }
        else
        {
            foregroundMask.SetActive(false);
            deleteButton.onClick.RemoveAllListeners();
            selectButton.onClick.AddListener(CreateSave);
        }
    }

    private void SetOccupiedSlot(PlayerData playerData)
    {
        deleteButton.gameObject.SetActive(true);
        deleteButton.onClick.RemoveAllListeners();
        deleteButton.onClick.AddListener(DeleteSave);

        playerNameObj.text = playerData.characterName;
        floorObj.text = playerData.levelCompleted.ToString();
        dateObj.text = DateTime.Parse(playerData.dateTime).ToString("dd/MM/yyyy");
        timeObj.text = DateTime.Parse(playerData.dateTime).ToString("HH/mm");


        if (loadingSlot)
        {
            selectButton.onClick.AddListener(LoadSave);
            foregroundMask.SetActive(false);
        }
        else
        {
            foregroundMask.SetActive(true);
        }
    }


    private void CreateSave()
    {
        newCharacterPanel.gameObject.SetActive(true);
        newCharacterPanel.SelectedSlot = slotNumber;
    }

    private void DeleteSave()
    {
        askPanel.SetActive(true);

        askPanelYesButton.onClick.RemoveAllListeners();
        askPanelYesButton.onClick.AddListener(DeleteSaveButton);
        askPanelInfo.text = "Do You want to delete save from slot no. " + slotNumber.ToString() + " ?";

    }

    private void DeleteSaveButton()
    {
        SaveManager.instance.DeleteSave(slotNumber);
        askPanel.SetActive(false);
        TryGetSave();
    }

    private void LoadSave()
    {
        SaveManager.instance.ChosenSlotIndex = slotNumber;
        loadButton.onClick.RemoveAllListeners();
        loadButtonManager.ActivateButton();
        loadButton.onClick.AddListener(LoadingButton);
    }

    private void LoadingButton()
    {
        //Debug.Log("Loading button");
        GameEvents.instance.LoadGameScene();
    }

    private void TryGetSave()
    {
        if(SaveManager.instance.GetPlayerData(slotNumber,out PlayerData playerData))
        {
            SetOccupiedSlot(playerData);
        }
        else
        {
            SetEmptySlot();
        }
    }

    private void OnDisable()
    {
        if (loadButtonManager != null)
        {
            loadButtonManager.DeactivateButton();
        }
    }
}
