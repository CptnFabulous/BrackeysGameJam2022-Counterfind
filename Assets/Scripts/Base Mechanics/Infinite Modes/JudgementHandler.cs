using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class JudgementHandler : MonoBehaviour
{
    public UnityEvent onCorrect;
    public UnityEvent onIncorrect;

    public GameObject hudObject;

    public virtual void OnResetGame()
    {
        UpdateHUD();
    }
    public virtual void OnCorrect()
    {
        UpdateHUD();
        onCorrect.Invoke();
    }
    public virtual void OnIncorrect()
    {
        UpdateHUD();
        onIncorrect.Invoke();
    }
    public virtual void UpdateHUD() { }

    private void OnEnable() => hudObject?.SetActive(true);
    private void OnDisable() => hudObject?.SetActive(false);
}
