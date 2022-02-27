using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateHandler : MonoBehaviour
{
    public Canvas headsUpDisplay;
    public Canvas pauseMenu;
    public Canvas endScreen;
    public Button pauseButton;
    public Button resumeButton;
    public Button quitToMainMenuButton;

    Canvas currentlyOpen;


    private void OnEnable()
    {
        if (currentlyOpen != null)
        {
            SwitchCanvas(currentlyOpen);
        }
    }
    private void OnDisable()
    {
        headsUpDisplay.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(false);
        endScreen.gameObject.SetActive(false);
    }
    private void Awake()
    {
        pauseButton.onClick.AddListener(PauseGame);
        resumeButton.onClick.AddListener(ResumeGame);
        quitToMainMenuButton.onClick.AddListener(LevelProgressionHandler.Current.ReturnToMenu);
    }
    private void Start()
    {
        ResumeGame();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        SwitchCanvas(pauseMenu);
    }
    public void ResumeGame()
    {
        SwitchCanvas(headsUpDisplay);
        Time.timeScale = 1;
    }
    public void EndLevel()
    {
        Time.timeScale = 1;
        SwitchCanvas(endScreen);
    }

    void SwitchCanvas(Canvas desired)
    {
        headsUpDisplay.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(false);
        endScreen.gameObject.SetActive(false);
        desired.gameObject.SetActive(true);
        currentlyOpen = desired;

    }
}
