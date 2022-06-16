using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameplayHandler : MonoBehaviour
{
    public Gamemode currentMode
    {
        get => mode;
        set
        {
            mode = value;
            GenerateGame();
        }
    }
    Gamemode mode;
    
    [Header("Game elements")]
    public Banknote prefab;
    public ObjectViewer viewControls;
    public GameStateHandler stateHandler;

    [Header("HUD elements")]
    public Timer levelTimer;
    public Text remainingNotes;
    public Button acceptButton;
    public Button rejectButton;
    public ReferenceWindow referenceWindow;

    [Header("Transitioning to next note")]
    public Transform entryPilePosition;
    public Transform exitPilePosition;
    public float exitTime = 0.5f;
    public AnimationCurve exitAnimationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public UnityEvent onBringOut;
    public UnityEvent onPutAway;
    
    IEnumerator transition;

    private void Awake()
    {
        acceptButton.onClick.AddListener(() => JudgeItem(false));
        rejectButton.onClick.AddListener(() => JudgeItem(true));
        levelTimer.onTimeUp.AddListener(EndLevel);
        ExitGameplay();
    }
    

    
    void GenerateGame()
    {
        gameObject.SetActive(true);
        viewControls.enabled = true;
        stateHandler.enabled = true;

        mode.GenerateGamemodeElements();

        referenceWindow.Setup(mode.Defects());
        referenceWindow.SetWindowActiveState(false);

        levelTimer.StartTimer();

        stateHandler.ResumeGame();
        GoToNextItem();
    }


    void GoToNextItem()
    {
        transition = TransitionToNextItem();
        StartCoroutine(transition);
    }
    IEnumerator TransitionToNextItem()
    {
        Banknote previousItem = mode.CurrentItem();
        if (previousItem != null)
        {
            IEnumerator putAway = PutAwayOldItem(previousItem);
            yield return StartCoroutine(putAway);
        }

        mode.PrepareNextItem();

        Banknote newItem = mode.CurrentItem();
        if (newItem != null)
        {
            IEnumerator getNew = DeployNewItem(newItem);
            yield return StartCoroutine(getNew);
        }
        else
        {
            //EndLevel();
        }
    }
    IEnumerator PutAwayOldItem(Banknote oldNote)
    {
        onPutAway.Invoke();
        
        acceptButton.interactable = false;
        rejectButton.interactable = false;

        viewControls.viewedObject = null;
        oldNote.transform.parent = transform;
        Vector3 oldPosition = oldNote.transform.position;
        Quaternion oldRotation = oldNote.transform.rotation;

        for (float timer = 0; timer < 1; timer = Mathf.Clamp01(timer + Time.deltaTime / exitTime))
        {
            float t = exitAnimationCurve.Evaluate(timer);
            oldNote.transform.position = Vector3.Lerp(oldPosition, exitPilePosition.position, t);
            oldNote.transform.rotation = Quaternion.Lerp(oldRotation, exitPilePosition.rotation, t);
            yield return null;
        }
    }
    IEnumerator DeployNewItem(Banknote newNote)
    {
        onBringOut.Invoke();
        
        viewControls.AddObject(newNote.transform);
        yield return new WaitWhile(() => viewControls.controlDenied);

        acceptButton.interactable = true;
        rejectButton.interactable = true;
    }
    
    public void EndLevel()
    {
        levelTimer.Pause();
        viewControls.enabled = false;
        stateHandler.EndLevel();
    }
    public void ExitGameplay()
    {
        gameObject.SetActive(false);
        viewControls.enabled = false;
        stateHandler.enabled = false;

        if (mode == null) return;

        // Disable existing notes
        if (mode.allItems != null)
        {
            for (int i = 0; i < mode.allItems.Length; i++)
            {
                mode.allItems[i].gameObject.SetActive(false);
            }
        }
    }

    void JudgeItem(bool counterfeit)
    {
        mode.OnJudgementMade(counterfeit);
        //Debug.Log((currentlyChecking + 1) + ": " + allNotes[currentlyChecking].Counterfeit + ", " + counterfeit);
        GoToNextItem();
    }
}

