using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfiniteModeEndScreen : EndScreen
{
    [Header("Infinite mode stats")]
    public Text time;
    public Text score;
    public Text numberChecked;
    public Text accuracy;
    public Text longestStreak;

    public InfiniteMode mode { get; set; }
    
    public override void Generate()
    {
        base.Generate();
        
        time.text = mode.gameElements.timer.elapsed.ToString(false);
        score.text = mode.score.currentScore.ToString();

        numberChecked.text = mode.currentItemIndex.ToString();
        longestStreak.text = mode.score.longestStreak.ToString();

        accuracy.text = (mode.accuracyRatio * 100) + "%";
    }
}
