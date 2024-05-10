using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvertedToggleDynamicBoolLogic : MonoBehaviour
{
    [System.Serializable]
    private class UnityEventBool : UnityEngine.Events.UnityEvent<bool> { }
    [SerializeField] private UnityEventBool onValueChangedInverse;

    private void Start()
    {
        GetComponent<Toggle>().onValueChanged.AddListener((on) => { onValueChangedInverse.Invoke(!on); });
    }
}
