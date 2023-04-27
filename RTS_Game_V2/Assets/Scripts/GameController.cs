using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameController : MonoBehaviour
{
    [Header("UI Message")]
    [Tooltip("Message Prefab")]
    [SerializeField] private GameObject objectToPool;
    [Tooltip("Number of prefab to pool")]
    [SerializeField] private int amountToPool;
    [Tooltip("Specify whether the Object Pooler can create extra objects at runtime when there is a need for them")]
    [SerializeField] private bool canAddObjectsToPool;
    [Tooltip("Specify whether the Message Menu should follow mouse when on top of a object")]
    [SerializeField] private bool mouseFollow;
    void Start()
    {
        if (!FindObjectOfType(typeof(Inventory)))
        {
            GameObject invenotry = new GameObject("Inventory",typeof(Inventory));
        }

        if (!FindObjectOfType(typeof(UIMessageObjectPool)))
        {
            GameObject uiObjectPooler = new GameObject("UIObjectpooler", typeof(UIMessageObjectPool));
            UIMessageObjectPool.instance.AmountToPool = amountToPool;
            UIMessageObjectPool.instance.ObjectToPool = objectToPool;
            UIMessageObjectPool.instance.canAddObjects = canAddObjectsToPool;
        }
    }
}
