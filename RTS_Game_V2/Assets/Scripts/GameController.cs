using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject objectToPool;
    [SerializeField] private int amountToPool;
    [SerializeField] private bool canAddObjectsToPool;
    void Start()
    {
        if (!FindObjectOfType(typeof(Inventory)))
        {
            GameObject invenotry = new GameObject("Inventory",typeof(Inventory));
        }

        if (!FindObjectOfType(typeof(UIObjectPool)))
        {
            GameObject uiObjectPooler = new GameObject("UIObjectpooler", typeof(UIObjectPool));
            UIObjectPool.instance.AmountToPool = amountToPool;
            UIObjectPool.instance.ObjectToPool = objectToPool;
            UIObjectPool.instance.canAddObjects = objectToPool;
        }
    }
}
