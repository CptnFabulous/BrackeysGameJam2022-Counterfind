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

    [Header("Cosmetics")]
    public Text scoreCounter;
    public Text streakCounter;

    public int successStreak { get; private set; } // How many notes has the player gotten correct in a row?
    public float currentMultiplier => 1 + (consecutiveMultiplierIncrease * successStreak);
    public override void OnResetGame()
    {
        score = 0;
        successStreak = 0;
        base.OnResetGame();
    }

    public override void OnCorrect()
    {
        score += Mathf.CeilToInt(scorePerNote * currentMultiplier);
        successStreak++;
        base.OnCorrect();
    }
    public override void OnIncorrect()
    {
        successStreak = 0;
        base.OnIncorrect();
    }


    public override void UpdateHUD()
    {
        scoreCounter.text = score.ToString();
        Debug.Log("Current score = " + score + ", multiplier = " + currentMultiplier);
        streakCounter.text = "X" + successStreak;
    }
}
