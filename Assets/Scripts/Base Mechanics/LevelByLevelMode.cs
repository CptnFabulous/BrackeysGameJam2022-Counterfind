using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelByLevelMode : Gamemode
{
    public Timer levelTimer;
    public LevelEndScreen endScreen;

    #region Level progression variables
    public Level currentLevel { get; private set; }
    public Banknote[] allItems { get; set; }
    public bool[] judgedFakeByPlayer { get; private set; }
    public int currentlyChecking { get; private set; }
    #endregion

    #region Level properties
    public bool notesExist => allItems.Length > 0;
    public bool onLastNote => currentlyChecking >= allItems.Length - 1;
    #endregion

    #region Override properties
    public override JudgementHandler[] modifiers => null;
    public override Banknote.Defect CurrentDefects => currentLevel.defects;
    public override Banknote currentItem
    {
        get
        {
            bool indexIsInArray = currentlyChecking >= 0;
            return notesExist && indexIsInArray ? allItems[currentlyChecking] : null;
        }
    }
    #endregion

    #region Override functions
    public override void GenerateGamemodeElements()
    {
        List<Banknote> newNotes = new List<Banknote>();
        for (int i = 0; i < currentLevel.numberOfItems; i++)
        {
            // Spawn note object
            Banknote note = Instantiate(gameElements.prefab);
            // If index is less than number of counterfeits, mark as counterfeit
            note.GenerateNote(i < currentLevel.numberOfCounterfeits, CurrentDefects);
            // Insert at a random point to shuffle the array
            newNotes.Insert(Random.Range(0, newNotes.Count), note);
        }
        allItems = newNotes.ToArray();
        judgedFakeByPlayer = new bool[allItems.Length];
        currentlyChecking = -1;

        // Reset timer and add appropriate listeners
        levelTimer.onTimeUp.AddListener(EndGameplay);
        levelTimer.stopWhenExpired = true;
        levelTimer.parTime = currentLevel.timeLimit;
        levelTimer.StartTimer();

        base.GenerateGamemodeElements();
    }
    public override void OnJudgementMade(bool deemedCounterfeit)
    {
        judgedFakeByPlayer[currentlyChecking] = deemedCounterfeit;

        if (onLastNote)
        {
            levelTimer.Pause();
        }
    }
    public override void PrepareNextItem()
    {
        // Determine if notes are available, and if some have not yet been checked
        Banknote nextItem = (notesExist && onLastNote == false) ? allItems[currentlyChecking + 1] : null;
        if (nextItem != null)
        {
            // If a new item is present, increase currentlyChecking so that one is registered
            currentlyChecking++;
        }
        else
        {
            EndGameplay();
        }
    }
    public override void EndGameplay()
    {
        levelTimer.Pause();

        endScreen.levelData = this;
        endScreen.Generate();

        base.EndGameplay();
    }
    public override void PurgeItems()
    {
        if (allItems == null) return;

        for (int i = 0; i < allItems.Length; i++)
        {
            if (allItems[i] == null) continue;
            Destroy(allItems[i].gameObject);
        }
    }
    #endregion

    public void SetLevel(Level newLevel)
    {
        currentLevel = newLevel;
        EnterGame();
    }

    public override void Awake()
    {
        levelTimer.onTimeUp.AddListener(EndGameplay);
        base.Awake();
    }
    private void LateUpdate()
    {
        if (currentLevel == null)
        {
            return;
        }
        gameElements.noteCounter.text = (currentlyChecking + 1) + "/" + allItems.Length;
        gameElements.timerDisplay.text = levelTimer.remaining.ToString(false);
    }
}
