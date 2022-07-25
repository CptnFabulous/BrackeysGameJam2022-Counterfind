using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteMode : Gamemode
{
    public Banknote.Defect defectsToInclude;

    Banknote noteToReuse;

    public LivesHandler lives => gameElements.player.lives;
    public ScoreHandler score => gameElements.player.score;


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

    public override JudgementHandler[] modifiers => new JudgementHandler[]
    {
        lives,
        score
    };
    public override Banknote.Defect CurrentDefects => defectsToInclude;
    public override Banknote currentItem => noteToReuse;
    public override void GenerateGamemodeElements()
    {
        base.GenerateGamemodeElements();

        noteToReuse = Instantiate(gameElements.prefab);
        
        gameElements.timer.stopWhenExpired = false;
        gameElements.timer.StartTimer();
    }
    public override void PrepareNextItem()
    {
        base.PrepareNextItem();
        // Regenerates the new item as real or fake
        bool isFake = Random.value <= 0.5f;
        currentItem.GenerateNote(isFake, CurrentDefects);
    }
    public override void PurgeItems()
    {
        if (noteToReuse != null)
        {
            Destroy(noteToReuse.gameObject);
        }
    }
    public override void EndGameplay()
    {
        InfiniteModeEndScreen endScreen = gameElements.player.GetComponentInChildren<InfiniteModeEndScreen>(true);
        endScreen.mode = this;
        endScreen.Generate();
        base.EndGameplay();
    }

    public override void Awake()
    {
        base.Awake();

        lives.mode = this;
        onCorrect.AddListener(lives.OnCorrect);
        onIncorrect.AddListener(lives.OnIncorrect);

        onCorrect.AddListener(score.OnCorrect);
        onIncorrect.AddListener(score.OnIncorrect);
    }
    public void LateUpdate()
    {
        gameElements.noteCounter.text = currentItemIndex.ToString();
        gameElements.timerDisplay.text = gameElements.timer.elapsed.ToString(false);
    }
}