using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InfiniteMode : Gamemode
{
    public Banknote.Defect defectsToInclude;
    //public LivesHandler lives;
    //public ScoreHandler score;
    public UnityEvent onCorrect;
    public UnityEvent onIncorrect;

    int currentNote;

    /*
    Ideas for infinite mode:
    Score system with points multiplier.
    * Each correct check increases your points multiplier by a small margin, depending on how quickly you checked it.
    * Making a mistake resets it (or at least severely slashes it)
    ‘Lives’ system – Player can make a certain number of mistakes before failing.
    * Player can regain lives if they get a certain number correct in a row
    Speed requirement – If you don’t check notes fast enough (e.g. if the game detects you’ve only done so many within a certain time period), you also get penalised.
    * This encourages haste, while the mistake penalties encourage caution.
    * This forces the player to balance rushing ahead versus playing it safe.
    */

    public override Banknote.Defect CurrentDefects => defectsToInclude;
    public override Banknote currentItem => allItems[0];
    public override void GenerateGamemodeElements()
    {
        allItems = new Banknote[1];
        allItems[0] = Instantiate(gameElements.prefab);
        currentNote = 0;
    }
    public override void PrepareNextItem()
    {
        // Regenerates the new item as real or fake, using a noise value to ensure not too many real or fake ones in a row.
        bool isFake = Mathf.PerlinNoise(currentNote, 0) <= 0.5f;
        currentItem.GenerateNote(isFake, CurrentDefects);
        currentNote++;
    }
    public override void OnJudgementMade(bool deemedCounterfeit)
    {
        bool correct = currentItem.Counterfeit == deemedCounterfeit;
        if (correct)
        {
            onCorrect.Invoke();
        }
        else
        {
            onIncorrect.Invoke();
        }
    }
    public override void EndGameplay()
    {
        throw new System.NotImplementedException();
    }


}