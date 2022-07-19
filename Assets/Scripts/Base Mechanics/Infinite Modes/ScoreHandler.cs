using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreHandler : JudgementHandler
{
    [Header("Score")]
    public int scorePerNote = 100; // Score awarded per note
    public float consecutiveMultiplierIncrease = 0.1f; // Score multiplier increase for each consecutive correct note
    

    [Header("Cosmetics")]
    public Text scoreCounter;
    public Text streakCounter;

    public int currentScore { get; private set; } // The current score
    public int successStreak { get; private set; } // How many notes has the player gotten correct in a row?
    public int longestStreak { get; private set; } // What's the maximum number of correct judgements the player has gotten in this game?
    public float currentMultiplier => 1 + (consecutiveMultiplierIncrease * successStreak);
    public override void OnResetGame()
    {
        currentScore = 0;
        successStreak = 0;
        longestStreak = 0;
        base.OnResetGame();
    }

    public override void OnCorrect()
    {
        currentScore += Mathf.CeilToInt(scorePerNote * currentMultiplier);
        successStreak++;
        longestStreak = Mathf.Max(longestStreak, successStreak);

        base.OnCorrect();
    }
    public override void OnIncorrect()
    {
        successStreak = 0;
        base.OnIncorrect();
    }


    public override void UpdateHUD()
    {
        scoreCounter.text = currentScore.ToString();
        streakCounter.enabled = successStreak > 1;
        streakCounter.text = "X" + successStreak;
        Debug.Log("Current score = " + currentScore + ", multiplier = " + currentMultiplier);
    }
}
