using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gamemode : MonoBehaviour
{
    public GameplayHandler gameElements;

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
    /// <summary>
    /// Runs when the player judges an item as real or fake.
    /// </summary>
    /// <param name="deemedCounterfeit"></param>
    public abstract void OnJudgementMade(bool deemedCounterfeit);
    /// <summary>
    /// Occurs after putting away the previous item and before loading the next one
    /// </summary>
    public abstract void PrepareNextItem();
    public virtual void EndGameplay()
    {
        enabled = false;
        gameElements.SuspendGameplay();
    }

    /// <summary>
    /// Destroys all existing items
    /// </summary>
    public abstract void PurgeItems();

    private void Awake()
    {
        enabled = false;
    }
}
