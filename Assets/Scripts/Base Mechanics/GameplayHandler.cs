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
    public Text noteCounter;
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

    [Header("Transitioning from menus")]
    public GameObject menuObject;
    public UnityEvent onEnterGameplay;


    IEnumerator transition;

    private void Awake()
    {
        acceptButton.onClick.AddListener(() => JudgeItem(false));
        rejectButton.onClick.AddListener(() => JudgeItem(true));
        

        ExitGameplay();
    }
    

    
    void GenerateGame()
    {
        menuObject.SetActive(false);

        gameObject.SetActive(true);
        viewControls.enabled = true;
        stateHandler.enabled = true;

        mode.GenerateGamemodeElements();

        referenceWindow.Setup(mode.CurrentDefects);
        //referenceWindow.SetWindowActiveState(false);


        stateHandler.ResumeGame();
        GoToNextItem();

        onEnterGameplay.Invoke();
    }


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

        mode.PrepareNextItem();

        Banknote newItem = mode.currentItem;
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

        viewControls.viewedObject = null;
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
        
        viewControls.AddObject(newNote.transform);
        yield return new WaitWhile(() => viewControls.controlDenied);

        acceptButton.interactable = true;
        rejectButton.interactable = true;
    }
    
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

        if (mode == null) return;

        // Disable existing notes
        if (mode.allItems != null)
        {
            for (int i = 0; i < mode.allItems.Length; i++)
            {
                mode.allItems[i].gameObject.SetActive(false);
            }
        }

        menuObject.SetActive(true);
    }

    void JudgeItem(bool counterfeit)
    {
        mode.OnJudgementMade(counterfeit);
        //Debug.Log((currentlyChecking + 1) + ": " + allNotes[currentlyChecking].Counterfeit + ", " + counterfeit);
        GoToNextItem();
    }
}

