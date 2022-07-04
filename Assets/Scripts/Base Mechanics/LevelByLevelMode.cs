using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelByLevelMode : Gamemode
{
    public Timer levelTimer;
    public LevelEndScreen endScreen;
    
    public Level currentLevel { get; private set; }
    public bool[] judgedFakeByPlayer { get; private set; }
    public int currentlyChecking { get; private set; }

    public Banknote[] allItems { get; set; }
    public bool notesExist => allItems.Length > 0;
    public bool onLastNote => currentlyChecking >= allItems.Length - 1;

    public override Banknote.Defect CurrentDefects => currentLevel.defects;
    public override Banknote currentItem
    {
        get
        {
            bool indexIsInArray = currentlyChecking >= 0;
            return notesExist && indexIsInArray ? allItems[currentlyChecking] : null;
        }
    }

    private void Awake()
    {
        levelTimer.onTimeUp.AddListener(EndGameplay);
    }

    public override void GenerateGamemodeElements()
    {
        List<Banknote> newNotes = new List<Banknote>();
        for (int i = 0; i < currentLevel.numberOfItems; i++)
        {
            // Spawn note object
            Banknote note = Instantiate(gameElements.prefab);
            // If index is less than number of counterfeits, mark as counterfeit
            // To ensure the correct amount
            note.GenerateNote(i < currentLevel.numberOfCounterfeits, CurrentDefects);

            // Insert at a random point to shuffle the array
            newNotes.Insert(Random.Range(0, newNotes.Count), note);
        }
        allItems = newNotes.ToArray();
        judgedFakeByPlayer = new bool[allItems.Length];
        currentlyChecking = -1;

        // Reset timer
        levelTimer.timeLimit = currentLevel.timeLimit;
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
        endScreen.ShowLevelEnd(this);
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

    public void SetLevel(Level newLevel)
    {
        currentLevel = newLevel;
        EnterGame();
    }



    


    private void LateUpdate()
    {
        if (currentLevel == null)
        {
            return;
        }
        gameElements.noteCounter.text = (currentlyChecking + 1) + "/" + allItems.Length;
    }

    

}
