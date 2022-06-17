using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesHandler : JudgementHandler
{
    [Header("Lives")]
    public int maxLives = 3; // Starting/max number of lives
    public int consecutiveSuccessesToGetNewLife = 5;


    int lives; // Current number of lives (when this reaches zero you fail)
    int consecutiveSuccesses;

    public override void OnResetGame()
    {
        lives = maxLives;
    }
    public override void OnIncorrect()
    {
        lives--;
        consecutiveSuccesses = 0;
        base.OnIncorrect();
    }
    public override void OnCorrect()
    {
        base.OnCorrect();

        if (lives < maxLives) // If player is missing lives, check criteria to get them back
        {
            consecutiveSuccesses++;
            while (consecutiveSuccesses >= consecutiveSuccessesToGetNewLife)
            {
                lives++;
                consecutiveSuccesses -= consecutiveSuccessesToGetNewLife;
            }
        }
    }


}
