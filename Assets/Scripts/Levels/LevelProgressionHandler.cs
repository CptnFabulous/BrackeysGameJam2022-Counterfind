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

    [Header("Loading gameplay and UI")]
    public LevelByLevelMode levelSetter;
    public LevelSelectScreen selectionScreen;

    [Header("Infinite Mode")]
    public InfiniteMode infiniteMode;
    public Button infiniteModeButton;

    public void LoadNewLevel(int index)
    {
        currentLevelIndex = Mathf.Clamp(index, 0, allLevels.Length - 1);
        levelSetter.SetLevel(allLevels[currentLevelIndex]);
    }
    public void RetryLevel() => levelSetter.SetLevel(allLevels[currentLevelIndex]);
    public void ProceedToNextLevel()
    {
        currentLevelIndex++;
        levelSetter.SetLevel(allLevels[currentLevelIndex]);
    }
    public void ReturnToMenu() => levelSetter.gameElements.ExitGameplay();

    private void Awake()
    {
        selectionScreen.SetupButtons(this);

        infiniteModeButton.onClick.AddListener(infiniteMode.EnterGame);
    }
}

