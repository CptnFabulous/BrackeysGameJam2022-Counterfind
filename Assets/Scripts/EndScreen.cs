using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class EndScreen : MonoBehaviour
{
    public Text requiredRightForSuccess;
    public Text successful;
    public Text thoughtWasReal;
    public Text thoughtWasFake;
    public Text remainingTime;
    public Button retryLevelButton;
    public Button nextLevelButton;
    public Button quitButton;

    public UnityEvent onResetEndScreenElements;
    public UnityEvent onWin;
    public UnityEvent onPerfectWin;
    public UnityEvent onAllLevelsCompleted;
    public UnityEvent onFailGeneric;
    public UnityEvent onTooFewCheckedAccurately;
    public UnityEvent onTimeRanOut;

    private void Awake()
    {
        retryLevelButton.onClick.AddListener(()=> LevelProgressionHandler.Current.LoadLevel());
        nextLevelButton.onClick.AddListener(()=> LevelProgressionHandler.Current.ProceedToNextLevel());
        quitButton.onClick.AddListener(()=> LevelProgressionHandler.Current.ReturnToMenu());
    }

    public void ShowLevelEnd(LevelManager manager)
    {
        onResetEndScreenElements.Invoke();

        int finalScore = 0;
        int amountThoughtReal = 0;
        int amountThoughtFake = 0;
        for (int i = 0; i < manager.currentlyChecking; i++)
        {
            if (manager.judgedFakeByPlayer[i] == manager.allNotes[i].Counterfeit)
            {
                finalScore++;
            }
            else if (manager.allNotes[i].Counterfeit)
            {
                amountThoughtReal++;
            }
            else
            {
                amountThoughtFake++;
            }
        }

        int errors = amountThoughtReal + amountThoughtFake;

        // Checks if the items were processed within the correct time
        // Checks if the amount of errors was less than the maximum acceptable
        bool allCompleted = manager.currentlyChecking >= manager.currentLevel.numberOfItems;
        bool notTooManyErrors = errors < manager.currentLevel.numberOfErrorsForFailure;
        if (allCompleted && notTooManyErrors) // If so, the level is a success
        {
            bool complete = LevelProgressionHandler.Current.onLastLevel;
            //Debug.Log("Are there any more levels? " + !complete);
            nextLevelButton.interactable = !complete;
            if (complete)
            {
                onAllLevelsCompleted.Invoke();
            }

            if (errors <= 0)
            {
                onPerfectWin.Invoke();
            }
            onWin.Invoke();
        }
        else if (!allCompleted) // if the first bool is false, the player ran out of time
        {
            nextLevelButton.interactable = false;
            onFailGeneric.Invoke();
            onTimeRanOut.Invoke();
        }
        else // Otherwise, the player completed them all but got some wrong
        {
            nextLevelButton.interactable = false;
            onFailGeneric.Invoke();
            onTooFewCheckedAccurately.Invoke();
        }

        int minCorrectForSuccess = manager.currentLevel.numberOfItems - manager.currentLevel.numberOfErrorsForFailure + 1;
        requiredRightForSuccess.text = minCorrectForSuccess.ToString();
        successful.text = finalScore.ToString();
        thoughtWasReal.text = amountThoughtReal.ToString();
        thoughtWasFake.text = amountThoughtFake.ToString();
        remainingTime.text = manager.levelTimer.remaining.ToString();
    }
}
