using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public bool noMoreLevels
    {
        get
        {
            return currentLevelIndex >= allLevels.Length;
        }
    }

    [Header("UI - Level Select Screen")]
    public GameObject menuObject;
    //public Canvas mainMenu;
    public Button levelButtonPrefab;
    public RectTransform levelButtonParent;
    Button[] levelButtons;

    [Header("Loading levels")]
    public LevelManager levelSetter;


    private void Awake()
    {
        levelButtonPrefab.gameObject.SetActive(false);
        Debug.Log("Assinging buttons");
        levelButtons = new Button[allLevels.Length];
        for (int i = 0; i < allLevels.Length; i++)
        {
            Button levelButton = Instantiate(levelButtonPrefab, levelButtonParent);
            levelButtons[i] = levelButton;
            int indexToAssign = i;
            levelButton.onClick.AddListener(() =>
            {
                currentLevelIndex = indexToAssign;
                LoadLevel();
            });

            levelButton.gameObject.SetActive(true);
            RectTransform rt = levelButton.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector3(0, -rt.rect.height * i, 0);
            levelButtonParent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rt.rect.height * (i + 1));

            levelButton.name = "Enter level: " + allLevels[i].name;
            Text levelNameLabel = levelButton.GetComponentInChildren<Text>();
            levelNameLabel.text = allLevels[i].name;
        }
    }


    public void LoadLevel()
    {
        menuObject.gameObject.SetActive(false);
        Debug.Log(currentLevelIndex + ", " + allLevels.Length);
        levelSetter.currentLevel = allLevels[currentLevelIndex];
    }
    public void ProceedToNextLevel()
    {
        currentLevelIndex++;
        LoadLevel();
    }
    public void ReturnToMenu()
    {
        levelSetter.ExitGameplay();
        menuObject.gameObject.SetActive(true);
    }
}
