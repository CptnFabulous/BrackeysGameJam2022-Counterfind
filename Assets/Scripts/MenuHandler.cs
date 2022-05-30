using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour
{
    public Canvas rootWindow;
    
    List<Canvas> childWindows;
    Canvas ownCanvas;

    public AudioSource soundEffectPlayer { get; private set; }


    private void Awake()
    {
        ownCanvas = GetComponent<Canvas>();
        childWindows = new List<Canvas>(GetComponentsInChildren<Canvas>(true));
        childWindows.Remove(ownCanvas);

        if (rootWindow == null || childWindows.Contains(rootWindow) == false)
        {
            rootWindow = childWindows[0];
        }

        soundEffectPlayer = GetComponent<AudioSource>();
        soundEffectPlayer.clip = null;
        soundEffectPlayer.loop = false;
    }
    private void Start()
    {
        SwitchWindow(rootWindow);
    }

    public void SwitchWindow(Canvas newWindow)
    {
        if (childWindows.Contains(newWindow) == false)
        {
            return;
        }
        for (int i = 0; i < childWindows.Count; i++)
        {
            childWindows[i].gameObject.SetActive(false);
        }
        newWindow.gameObject.SetActive(true);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void ToggleFullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
    

    
}
