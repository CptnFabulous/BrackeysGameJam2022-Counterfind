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



        // If a root window is null, or is assigned to something that isn't actually part of the menu hierarchy
        if (rootWindow == null || childWindows.Contains(rootWindow) == false)
        {
            if (childWindows.Count > 0)
            {
                rootWindow = childWindows[0];
            }
            else
            {
                rootWindow = ownCanvas;
            }
        }

        soundEffectPlayer = GetComponent<AudioSource>();
        soundEffectPlayer.clip = null;
        soundEffectPlayer.loop = false;
        soundEffectPlayer.playOnAwake = false;
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



    #region Game functions
    public void QuitGame() => Application.Quit();
    public void ToggleFullscreen() => Screen.fullScreen = !Screen.fullScreen;
    #endregion

    
}
