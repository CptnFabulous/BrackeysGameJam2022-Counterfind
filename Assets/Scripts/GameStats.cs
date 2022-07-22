using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStats : MonoBehaviour
{
    public Text highScore;
    public Text bestAccuracy;
    public Text highestSpeed;

    private void Awake()
    {
        SaveHandler.onLoadGame += RefreshStats;
    }

    public void RefreshStats(SaveHandler.SaveFile data)
    {
        highScore.text = data.highScore.ToString();
        bestAccuracy.text = data.bestAccuracyRatio + "%";
        //highestSpeed.text = 
        //levelsCompleted.text = data.completedLevelIndex.ToString(); // I don't need to put +1 because if the player is on the first level, the index would be 0, which would also accurately represent that the player has not completed any levels.
    }
}
