using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameplayHandler : MonoBehaviour
{
    public Gamemode currentMode { get; private set; }

    [Header("Game elements")]
    public Banknote prefab;
    public Timer timer;

    [Header("Controls")]
    public ObjectViewer viewControls;
    public Button acceptButton;
    public Button rejectButton;
    public ReferenceWindow referenceWindow;

    [Header("Showing next item")]
    public Transform entryPilePosition;
    public Transform exitPilePosition;
    public float exitTime = 0.5f;
    public AnimationCurve exitAnimationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public UnityEvent onBringOut;
    public UnityEvent onPutAway;

    [Header("Game/menu transition")]
    public GameStateHandler stateHandler;
    public GameObject menuObject;
    public UnityEvent onEnterGameplay;

    [Header("HUD elements")]
    public Text timerDisplay;
    public Text noteCounter;

    

    IEnumerator transition;

    private void Awake()
    {
        acceptButton.onClick.AddListener(() => JudgeItem(false));
        rejectButton.onClick.AddListener(() => JudgeItem(true));
        
        ExitGameplay();
    }
    
    
    public void GenerateGame(Gamemode newMode)
    {
        currentMode = newMode;
        
        menuObject.SetActive(false);

        gameObject.SetActive(true);
        viewControls.enabled = true;
        stateHandler.enabled = true;

        viewControls.SetObject(null);

        currentMode.GenerateGamemodeElements();

        referenceWindow.Setup(currentMode.CurrentDefects);

        stateHandler.ResumeGame();
        GoToNextItem();

        onEnterGameplay.Invoke();
    }

    void JudgeItem(bool counterfeit)
    {
        currentMode.OnJudgementMade(counterfeit);
        //Debug.Log((currentlyChecking + 1) + ": " + allNotes[currentlyChecking].Counterfeit + ", " + counterfeit);
        GoToNextItem();
    }

    #region Transitioning to next item
    void GoToNextItem()
    {
        transition = TransitionToNextItem();
        StartCoroutine(transition);
    }
    IEnumerator TransitionToNextItem()
    {
        if (viewControls.viewedObject != null)
        {
            IEnumerator putAway = PutAwayOldItem();
            yield return StartCoroutine(putAway);
        }

        currentMode.PrepareNextItem();

        Banknote newItem = currentMode.currentItem;
        if (newItem != null)
        {
            IEnumerator getNew = DeployNewItem(newItem);
            yield return StartCoroutine(getNew);
        }
    }
    IEnumerator PutAwayOldItem()
    {
        onPutAway.Invoke();
        
        acceptButton.interactable = false;
        rejectButton.interactable = false;

        Transform oldItemTransform = viewControls.viewedObject;

        viewControls.SetObject(null);
        oldItemTransform.parent = transform;
        Vector3 oldPosition = oldItemTransform.position;
        Quaternion oldRotation = oldItemTransform.rotation;

        for (float timer = 0; timer < 1; timer = Mathf.Clamp01(timer + Time.deltaTime / exitTime))
        {
            float t = exitAnimationCurve.Evaluate(timer);
            oldItemTransform.position = Vector3.Lerp(oldPosition, exitPilePosition.position, t);
            oldItemTransform.rotation = Quaternion.Lerp(oldRotation, exitPilePosition.rotation, t);
            yield return null;
        }
    }
    IEnumerator DeployNewItem(Banknote newNote)
    {
        onBringOut.Invoke();

        newNote.transform.position = entryPilePosition.position;
        newNote.transform.rotation = entryPilePosition.rotation;

        viewControls.SetObject(newNote.transform);
        yield return new WaitWhile(() => viewControls.controlDenied);

        acceptButton.interactable = true;
        rejectButton.interactable = true;
    }
    #endregion

    public void SuspendGameplay()
    {
        viewControls.enabled = false;
        stateHandler.EndLevel();
    }
    public void ExitGameplay()
    {
        gameObject.SetActive(false);
        viewControls.enabled = false;
        stateHandler.enabled = false;

        if (currentMode == null) return;

        currentMode.PurgeItems();

        menuObject.SetActive(true);
    }

    
}

