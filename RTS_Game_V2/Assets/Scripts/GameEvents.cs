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

    //public event Action onShowMessage;
    public event Action<int> onCloseMessage;

    public void CloseMessage(int i)
    {
        if(onCloseMessage != null)
        {
            onCloseMessage(i);
        }
    }
    
}
