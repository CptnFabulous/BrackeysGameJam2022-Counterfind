using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Gamemode : MonoBehaviour
{
    public GameplayHandler gameElements;
    public UnityEvent onCorrect;
    public UnityEvent onIncorrect;

    public abstract JudgementHandler[] modifiers { get; }

    public abstract Banknote.Defect CurrentDefects { get; }
    public abstract Banknote currentItem { get; }
    public float accuracyRatio => numberCorrect / currentItemIndex;
    public int currentItemIndex { get; protected set; }
    public int numberCorrect { get; protected set; }

    public void EnterGame()
    {
        JudgementHandler[] m = modifiers;
        if (m != null)
        {
            for (int i = 0; i < m.Length; i++)
            {
                m[i].OnResetGame();
            }
        }

        gameElements.GenerateGame(this);
    }
    /// <summary>
    /// Runs when the gameplay is loaded, almost like Start().
    /// </summary>
    public virtual void GenerateGamemodeElements()
    {
        PurgeItems();
        gameElements.timer.onTimeUp.RemoveAllListeners();
        currentItemIndex = -1;
        numberCorrect = 0;
        enabled = true;
    }
    /// <summary>
    /// Runs when the player judges an item as real or fake.
    /// </summary>
    /// <param name="deemedCounterfeit"></param>
    public virtual void OnJudgementMade(bool deemedCounterfeit)
    {
        bool correct = currentItem.Counterfeit == deemedCounterfeit;
        if (correct)
        {
            numberCorrect++;
            onCorrect.Invoke();
        }
        else
        {
            onIncorrect.Invoke();
        }
    }
    /// <summary>
    /// Occurs after putting away the previous item and before loading the next one
    /// </summary>
    public virtual void PrepareNextItem() => currentItemIndex++; // Increments counter
    public virtual void EndGameplay()
    {
        enabled = false;
        gameElements.SuspendGameplay();
    }

    /// <summary>
    /// Destroys all existing items
    /// </summary>
    public abstract void PurgeItems();

    public virtual void Awake() => enabled = false;
}
