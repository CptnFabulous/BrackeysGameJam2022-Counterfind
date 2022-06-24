using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class LevelProgressionHandler : MonoBehaviour
{
    public static LevelProgressionHandler Current
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LevelProgressionHandler>();
            }
            return instance;
        }
    }
    public static LevelProgressionHandler instance;
    
    [Header("Level data")]
    public Level[] allLevels;
    public int currentLevelIndex;

    public bool onLastLevel => currentLevelIndex + 1 >= allLevels.Length;

    [Header("UI - Level Select Screen")]
    //public Canvas mainMenu;
    public Button levelButtonPrefab;
    public RectTransform levelButtonParent;
    Button[] levelButtons;

    [Header("Loading levels")]
    public LevelByLevelMode levelSetter;


    private void Awake()
    {
        SetupLevelSelectScreen();
    }

    void SetupLevelSelectScreen()
    {
        levelButtonPrefab.gameObject.SetActive(false);
        levelButtons = new Button[allLevels.Length];
        for (int i = 0; i < allLevels.Length; i++)
        {
            Button levelButton = Instantiate(levelButtonPrefab, levelButtonParent);
            levelButtons[i] = levelButton;
            int indexToAssign = i;
            levelButton.onClick.AddListener(() => LoadNewLevel(indexToAssign));

            levelButton.gameObject.SetActive(true);
            RectTransform rt = levelButton.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector3(0, -rt.rect.height * i, 0);
            levelButtonParent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rt.rect.height * (i + 1));

            levelButton.name = "Enter level: " + allLevels[i].name;
            Text levelNameLabel = levelButton.GetComponentInChildren<Text>();
            levelNameLabel.text = allLevels[i].name;
        }
    }

    public void LoadNewLevel(int index)
    {
        currentLevelIndex = Mathf.Clamp(index, 0, allLevels.Length - 1);
        levelSetter.SetLevel(allLevels[currentLevelIndex]);
    }
    public void RetryLevel()
    {
        levelSetter.SetLevel(allLevels[currentLevelIndex]);
    }
    public void ProceedToNextLevel()
    {
        currentLevelIndex++;
        levelSetter.SetLevel(allLevels[currentLevelIndex]);
    }
    public void ReturnToMenu()
    {
        levelSetter.gameElements.ExitGameplay();
    }
}
