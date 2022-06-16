using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gamemode : MonoBehaviour
{
    public GameplayHandler gameElements;

    public Banknote[] allItems { get; set; }
    public bool notesExist => allItems.Length > 0;

    public abstract List<Banknote.Defect> Defects();
    public abstract Banknote CurrentItem();
    public abstract Banknote NextItem();


    public abstract void GenerateGamemodeElements();
    public abstract void OnJudgementMade(bool deemedCounterfeit);
    public abstract void PrepareNextItem();
    public abstract void EndGameplay();
}
