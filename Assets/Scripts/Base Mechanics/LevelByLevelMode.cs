using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelByLevelMode : Gamemode
{
    public LevelEndScreen endScreen;
    
    public Level currentLevel { get; private set; }
    public bool[] judgedFakeByPlayer { get; private set; }
    public int currentlyChecking { get; private set; }

    public bool onLastNote => currentlyChecking >= allItems.Length - 1;

    public void SetLevel(Level newLevel)
    {
        currentLevel = newLevel;
        gameElements.currentMode = this;
    }

    public override List<Banknote.Defect> Defects() => Banknote.FromFlags(currentLevel.defects);
    public override Banknote CurrentItem()
    {
        bool indexIsInArray = currentlyChecking >= 0;
        return notesExist && indexIsInArray ? allItems[currentlyChecking] : null;
    }
    public override Banknote NextItem()
    {
        // Determine if notes are available, and if some have not yet been checked
        return (notesExist && onLastNote == false) ? allItems[currentlyChecking + 1] : null;
    }

    public override void GenerateGamemodeElements()
    {
        // Purge existing notes (code could probably be done better to prevent garbage collection but whatever it's a game jam)
        if (allItems != null)
        {
            for (int i = 0; i < allItems.Length; i++)
            {
                Destroy(allItems[i].gameObject);
            }
        }

        List<Banknote> newNotes = new List<Banknote>();
        for (int i = 0; i < currentLevel.numberOfItems; i++)
        {
            // Spawn note object
            Banknote note = Instantiate(gameElements.prefab, gameElements.entryPilePosition);
            note.transform.localPosition = Vector3.zero;
            note.transform.localRotation = Quaternion.identity;

            // If index is less than number of counterfeits, mark as counterfeit
            // To ensure the correct amount
            note.GenerateNote(i < currentLevel.numberOfCounterfeits, currentLevel);

            // Insert at a random point to shuffle the array
            newNotes.Insert(Random.Range(0, newNotes.Count), note);
        }
        allItems = newNotes.ToArray();
        judgedFakeByPlayer = new bool[allItems.Length];
        currentlyChecking = -1;

        // Reset timer
        gameElements.levelTimer.timeLimit = currentLevel.timeLimit;
    }
    public override void OnJudgementMade(bool deemedCounterfeit)
    {
        judgedFakeByPlayer[currentlyChecking] = deemedCounterfeit;

        if (onLastNote)
        {
            gameElements.levelTimer.Pause();
        }
    }
    public override void PrepareNextItem()
    {
        if (NextItem() != null) // If a new item is present, increase currentlyChecking so that one is registered
        {
            currentlyChecking++;
        }
        else
        {
            EndGameplay();
        }
    }
    public override void EndGameplay()
    {
        gameElements.SuspendGameplay();
        endScreen.ShowLevelEnd(this);
    }



    


    private void LateUpdate()
    {
        if (currentLevel == null)
        {
            return;
        }
        gameElements.remainingNotes.text = (allItems.Length - currentlyChecking - 1).ToString();
    }

    

}
