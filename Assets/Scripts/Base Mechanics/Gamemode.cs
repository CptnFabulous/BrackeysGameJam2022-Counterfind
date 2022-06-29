using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gamemode : MonoBehaviour
{
    public GameplayHandler gameElements;

    public Banknote[] allItems { get; set; }
    public bool notesExist => allItems.Length > 0;

    public abstract Banknote.Defect CurrentDefects { get; }
    public abstract Banknote currentItem { get; }

    public void EnterGame()
    {
        gameElements.currentMode = this;
    }
    /// <summary>
    /// Runs when the gameplay is loaded, almost like Start().
    /// </summary>
    public virtual void GenerateGamemodeElements()
    {
        enabled = true;
    }
    public abstract void OnJudgementMade(bool deemedCounterfeit);
    public abstract void PrepareNextItem();
    public virtual void EndGameplay()
    {
        enabled = false;
        gameElements.SuspendGameplay();
    }

    private void Awake()
    {
        enabled = false;
    }
}
