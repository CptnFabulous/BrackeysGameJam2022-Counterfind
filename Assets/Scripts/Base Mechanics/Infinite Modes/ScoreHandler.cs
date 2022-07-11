using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreHandler : JudgementHandler
{
    [Header("Score")]
    public int scorePerNote = 100; // Score awarded per note
    public float consecutiveMultiplierIncrease = 0.1f; // Score multiplier increase for each consecutive correct note
    int score; // The current score
    int consecutiveSuccesses; // How many notes has the player gotten correct in a row?

    [Header("Cosmetics")]
    public Text scoreCounter;
    public Text multiplierCounter;

    public float currentMultiplier => 1 + (consecutiveMultiplierIncrease * consecutiveSuccesses);
    public override void OnResetGame()
    {
        score = 0;
        consecutiveSuccesses = 0;
        base.OnResetGame();
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


    public override void UpdateHUD()
    {
        scoreCounter.text = score.ToString();
        multiplierCounter.text = currentMultiplier.ToString();
        Debug.Log("Current score = " + score + ", multiplier = " + currentMultiplier);
    }
}
