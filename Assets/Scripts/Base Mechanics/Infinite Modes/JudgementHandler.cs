using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class JudgementHandler : MonoBehaviour
{
    public UnityEvent onCorrect;
    public UnityEvent onIncorrect;

    public abstract void OnResetGame();
    public virtual void OnCorrect() => onCorrect.Invoke();
    public virtual void OnIncorrect() => onIncorrect.Invoke();
    public GameObject hudObject;


    private void OnEnable() => hudObject?.SetActive(true);
    private void OnDisable() => hudObject?.SetActive(false);
}
