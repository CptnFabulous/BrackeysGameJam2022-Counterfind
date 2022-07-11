using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesHandler : JudgementHandler
{
    [Header("Lives")]
    public int maxLives = 3; // Starting/max number of lives
    public int consecutiveSuccessesToGetNewLife = 5;

    [Header("Cosmetics")]
    public LifeMeter meter;

    float lives; // Current number of lives (when this reaches zero you fail)
    int consecutiveSuccesses;

    public InfiniteMode mode { get; set; }

    public override void OnResetGame()
    {
        lives = maxLives;
        base.OnResetGame();
    }
    public override void OnIncorrect()
    {
        lives--;
        consecutiveSuccesses = 0;
        base.OnIncorrect();

        if (lives <= 0)
        {
            mode.EndGameplay();
        }
    }
    public override void OnCorrect()
    {
        
        if (lives < maxLives) // If player is missing lives, check criteria to get them back
        {
            consecutiveSuccesses++;
            while (consecutiveSuccesses >= consecutiveSuccessesToGetNewLife)
            {
                lives++;
                consecutiveSuccesses -= consecutiveSuccessesToGetNewLife;
            }
        }

        base.OnCorrect();
    }

    public override void UpdateHUD()
    {
        meter?.UpdateMeter(lives);
        base.UpdateHUD();
    }


}
