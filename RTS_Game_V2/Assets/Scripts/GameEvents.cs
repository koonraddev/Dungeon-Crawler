using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents instance;

    private void Awake()
    {
        instance = this;
    }
    public event Action onInventoryUpdate;

    public void UpdateInventoryUI()
    {
        if(onInventoryUpdate!= null)
        {
            onInventoryUpdate();
        }
    }

}
