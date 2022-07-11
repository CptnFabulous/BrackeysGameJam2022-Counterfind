using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreHandler : JudgementHandler
{
    [Header("Score")]
    public int scorePerNote = 100; // Score awarded per note
    public float consecutiveMultiplierIncrease = 0.1f; // Score multiplier increase for each consecutive correct note
    int score; // The current score
    int consecutiveSuccesses; // How many notes has the player gotten correct in a row?

    public float currentMultiplier => 1 + (consecutiveMultiplierIncrease * consecutiveSuccesses);
    public override void OnResetGame()
    {
        score = 0;
        consecutiveSuccesses = 0;
    }

    public override void OnCorrect()
    {
        score += Mathf.CeilToInt(scorePerNote * currentMultiplier);
        consecutiveSuccesses++;
        base.OnCorrect();
    }
    public override void OnIncorrect()
    {
        consecutiveSuccesses = 0;
        base.OnIncorrect();
    }


}
