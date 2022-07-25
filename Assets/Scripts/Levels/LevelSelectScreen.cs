using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectScreen : MonoBehaviour
{
    //public LevelProgressionHandler handler;

    [Header("HUD elements")]
    public ScrollRect scrollWindow;
    public Button buttonPrefab;

    Button[] levelButtons;
    Text[] bestTimes;
    Image[] perfectCompletions;

    private void Awake()
    {
        SaveHandler.onLoadGame += Refresh;
    }
    public void SetupButtons(LevelProgressionHandler handler)
    {
        scrollWindow.horizontal = false;
        RectTransform buttonParent = scrollWindow.content;

        levelButtons = new Button[handler.allLevels.Length]; // Create array of buttons for each level
        buttonPrefab.gameObject.SetActive(true); // Enable button prefab to ensure that instances for each level are enabled
        for (int i = 0; i < levelButtons.Length; i++) // Create buttons for levels
        {
            // Spawn button and add listener to open the appropriate level
            Button button = Instantiate(buttonPrefab, buttonParent);
            button.onClick.AddListener(() => handler.LoadNewLevel(i));
            // Add information to button
            button.name = "Enter level: " + handler.allLevels[i].name;
            Text levelNameLabel = button.GetComponentInChildren<Text>();
            levelNameLabel.text = handler.allLevels[i].name;
            // Set position of button
            RectTransform rt = button.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector3(0, -rt.rect.height * i, 0);
            // Assign button in array
            levelButtons[i] = button;
        }
        buttonPrefab.gameObject.SetActive(false); // Disable original prefab since it doesn't have a level assigned

        // Set size of window based off bounds of buttons
        RectTransform buttonPrefabTransform = buttonPrefab.GetComponent<RectTransform>();
        buttonParent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, buttonPrefabTransform.rect.height * levelButtons.Length);

        //Refresh(SaveHandler.currentData);
    }

    public void Refresh(SaveHandler.SaveFile saveData)
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            bool unlocked = i <= saveData.completedLevelIndex;

            // Change interactable state based on if the player has unlocked the level yet
            levelButtons[i].interactable = unlocked;
            
            /*
            continue;
            // Code past this point doesn't work because references are not properly established for the time and score displays

            bool completed = i < saveData.completedLevelIndex;
            // Show time if player has completed level
            Text bestTimeDisplay = bestTimes[i];
            bestTimeDisplay.gameObject.SetActive(completed);
            if (completed)
            {
                Timer.TimeValue bestTime = new Timer.TimeValue(saveData.bestLevelTimes[i]);
                bestTimeDisplay.text = bestTime.ToString(false);
            }

            // Show 'perfect' symbol based on if player has got a perfect score on that level
            perfectCompletions[i].gameObject.SetActive(completed && saveData.perfectScores[i]);
            */
        }
    }
}
