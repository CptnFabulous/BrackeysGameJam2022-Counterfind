using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class LevelEndScreen : EndScreen
{
    public Button nextLevelButton;

    [Header("Information to display")]
    public GameObject winGraphic;
    public GameObject failGraphic;
    public string whenAllLevelsCompleted = "You completed all of the levels!";
    public string whenTimeUp = "You ran out of time!";
    public string whenTooInaccurate = "You made too many mistakes!";

    [Header("Stats")]
    public Text requiredRightForSuccess;
    public Text successful;
    public Text thoughtWasReal;
    public Text thoughtWasFake;
    public Text remainingTime;

    public UnityEvent onWin;
    public UnityEvent onPerfectWin;
    public UnityEvent onFailGeneric;


    public LevelByLevelMode levelData { get; set; }
    

    public override void Awake()
    {
        base.Awake();
        nextLevelButton.onClick.AddListener(LevelProgressionHandler.Current.ProceedToNextLevel);
    }

    public override void Generate()
    {
        base.Generate();

        #region Calculate scores
        int finalScore = 0;
        int amountThoughtReal = 0;
        int amountThoughtFake = 0;
        for (int i = 0; i < levelData.currentItemIndex + 1; i++)
        {
            if (levelData.judgedFakeByPlayer[i] == levelData.allItems[i].Counterfeit)
            {
                finalScore++;
            }
            else if (levelData.allItems[i].Counterfeit)
            {
                amountThoughtReal++;
            }
            else
            {
                amountThoughtFake++;
            }
        }
        #endregion

        #region Calculate completion status
        int errors = amountThoughtReal + amountThoughtFake;
        bool allChecked = levelData.onLastNote;
        bool notTooManyErrors = errors < levelData.currentLevel.numberOfErrorsForFailure;
        bool levelCompleted = allChecked && notTooManyErrors;
        #endregion

        #region Populate failure/success info
        //(levelCompleted ? onWin : onFailGeneric).Invoke();
        winGraphic.SetActive(levelCompleted);
        failGraphic.SetActive(!levelCompleted);
        if (levelCompleted) // If so, the level is a success
        {
            onWin.Invoke();
            if (errors <= 0)
            {
                onPerfectWin.Invoke();
            }
            if (LevelProgressionHandler.Current.onLastLevel)
            {
                reason.text = whenAllLevelsCompleted;
            }
        }
        else
        {
            // If allChecked is false, the player ran out of time
            // Otherwise, the player completed them all but got some wrong
            reason.text = !allChecked ? whenTimeUp : whenTooInaccurate;
            onFailGeneric.Invoke();
        }
        #endregion

        #region Display stats
        int minCorrectForSuccess = levelData.currentLevel.numberOfItems - levelData.currentLevel.numberOfErrorsForFailure + 1;
        requiredRightForSuccess.text = minCorrectForSuccess.ToString();
        successful.text = finalScore.ToString();
        thoughtWasReal.text = amountThoughtReal.ToString();
        thoughtWasFake.text = amountThoughtFake.ToString();
        remainingTime.text = levelData.levelTimer.remaining.ToString(false);
        #endregion

        nextLevelButton.interactable = levelCompleted && !LevelProgressionHandler.Current.onLastLevel;
    }

    /// <summary>
    /// References the current MusicHandler to switch the music state.
    /// </summary>
    /// <param name="stateName"></param>
    public void SetMusicState(string stateName) => MusicHandler.Current.SwitchStateFromString(stateName);
}
