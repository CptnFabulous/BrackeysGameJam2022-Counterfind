using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InfiniteMode : Gamemode
{
    public Banknote.Defect defectsToInclude;
    public UnityEvent onCorrect;
    public UnityEvent onIncorrect;

    [Header("Additional elements")]
    public LivesHandler lives;
    public ScoreHandler score;

    Banknote noteToReuse;



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
        currentItemIndex = 0;

        gameElements.timer.stopWhenExpired = false;
        gameElements.timer.StartTimer();
    }
    public override void PrepareNextItem()
    {
        base.PrepareNextItem();
        // Regenerates the new item as real or fake
        // Uses a noise value to ensure not too many real or fake ones in a row.
        float value = Random.value;//Mathf.PerlinNoise(currentItemIndex, 0);
        Debug.Log("Index = " + currentItemIndex + ", value = " + value);
        bool isFake = value <= 0.5f;
        currentItem.GenerateNote(isFake, CurrentDefects);
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
    public override void PurgeItems()
    {
        if (noteToReuse != null)
        {
            Destroy(noteToReuse);
        }
    }
    public override void EndGameplay()
    {
        gameElements.timer.Pause();
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