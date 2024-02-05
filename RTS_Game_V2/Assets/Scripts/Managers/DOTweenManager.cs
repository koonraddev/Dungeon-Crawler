using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DOTweenManager : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(this);
        DOTween.defaultAutoPlay = AutoPlay.None;
    }
}
