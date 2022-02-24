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

    public UnityEvent onPerfectWin;
    public UnityEvent onWin;
    public UnityEvent onTooFewCheckedAccurately;
    public UnityEvent onTimeRanOut;

    public void ShowLevelEnd(LevelManager manager)
    {
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
        // Checks first that all were checked within the time limit
        if (manager.currentlyChecking < manager.current.numberOfItems)
        {
            nextLevelButton.interactable = false;
            onTimeRanOut.Invoke();
        }
        else if (errors < manager.current.numberOfErrorsForFailure) // Checks how many were completed
        {
            if (errors <= 0)
            {
                onPerfectWin.Invoke();
            }
            else
            {
                onWin.Invoke();
            }
        }
        else // If neither previous statement was true, player failed due to getting too many wrong
        {
            nextLevelButton.interactable = false;
            onTooFewCheckedAccurately.Invoke();
        }

        requiredRightForSuccess.text = (manager.current.numberOfItems - manager.current.numberOfErrorsForFailure).ToString();
        successful.text = finalScore.ToString();
        thoughtWasReal.text = amountThoughtReal.ToString();
        thoughtWasFake.text = amountThoughtFake.ToString();
        remainingTime.text = manager.levelTimer.remaining.ToString();
    }
}
