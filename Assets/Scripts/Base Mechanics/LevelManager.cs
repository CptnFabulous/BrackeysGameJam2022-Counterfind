using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    public Level currentLevel
    {
        get
        {
            return current;
        }
        set
        {
            current = value;
            GenerateNewLevel();
        }
    }
    Level current;

    [Header("Game elements")]
    public Banknote prefab;
    public ObjectViewer viewControls;
    public GameStateHandler stateHandler;
    public EndScreen endScreen;

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
    
    public Banknote[] allNotes { get; private set; }
    public bool[] judgedFakeByPlayer { get; private set; }
    public int currentlyChecking { get; private set; }
    IEnumerator transition;

    private void Awake()
    {
        acceptButton.onClick.AddListener(() => JudgeItem(false));
        rejectButton.onClick.AddListener(() => JudgeItem(true));
        levelTimer.onTimeUp.AddListener(EndLevel);

        ExitGameplay();
    }
    private void LateUpdate()
    {
        if (currentLevel == null)
        {
            return;
        }
        remainingNotes.text = (allNotes.Length - currentlyChecking - 1).ToString();
    }

    public void GenerateNewLevel()
    {
        gameObject.SetActive(true);
        viewControls.enabled = true;
        stateHandler.enabled = true;

        // Purge existing notes (code could probably be done better to prevent garbage collection but whatever it's a game jam)
        if (allNotes != null)
        {
            for (int i = 0; i < allNotes.Length; i++)
            {
                Destroy(allNotes[i].gameObject);
            }
        }

        List<Banknote> newNotes = new List<Banknote>();
        for (int i = 0; i < currentLevel.numberOfItems; i++)
        {
            // Spawn note object
            Banknote note = Instantiate(prefab, entryPilePosition);
            note.transform.localPosition = Vector3.zero;
            note.transform.localRotation = Quaternion.identity;

            // If index is less than number of counterfeits, mark as counterfeit
            // To ensure the correct amount
            note.GenerateNote((i < currentLevel.numberOfCounterfeits), currentLevel);

            // Insert at a random point to shuffle the array
            newNotes.Insert(Random.Range(0, newNotes.Count), note);
        }
        allNotes = newNotes.ToArray();
        judgedFakeByPlayer = new bool[allNotes.Length];
        currentlyChecking = -1;

        referenceWindow.Setup(currentLevel);
        referenceWindow.SetWindowActiveState(false);

        // Reset timer
        levelTimer.timeLimit = currentLevel.timeLimit;
        levelTimer.StartTimer();

        stateHandler.ResumeGame();

        // Start transition to next item
        transition = TransitionToNextItem();
        StartCoroutine(transition);
    }
    IEnumerator TransitionToNextItem()
    {
        if (currentlyChecking >= allNotes.Length - 1)
        {
            levelTimer.Pause();
        }

        Banknote previousItem = (currentlyChecking < 0) ? null : allNotes[currentlyChecking];
        if (previousItem != null)
        {
            IEnumerator putAway = PutAwayOldItem(previousItem);
            yield return StartCoroutine(putAway);
        }

        currentlyChecking++;

        Banknote newItem = (currentlyChecking >= allNotes.Length) ? null : allNotes[currentlyChecking];
        if (newItem != null)
        {
            IEnumerator getNew = DeployNewItem(newItem);
            yield return StartCoroutine(getNew);
        }
        else
        {
            EndLevel();
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
    void JudgeItem(bool counterfeit)
    {
        judgedFakeByPlayer[currentlyChecking] = counterfeit;

        //Debug.Log((currentlyChecking + 1) + ": " + allNotes[currentlyChecking].Counterfeit + ", " + counterfeit);

        transition = TransitionToNextItem();
        StartCoroutine(transition);
    }
    void EndLevel()
    {
        levelTimer.Pause();
        viewControls.enabled = false;
        stateHandler.EndLevel();
        endScreen.ShowLevelEnd(this);
    }
    public void ExitGameplay()
    {
        gameObject.SetActive(false);
        viewControls.enabled = false;
        stateHandler.enabled = false;
        // Disable existing notes
        if (allNotes != null)
        {
            for (int i = 0; i < allNotes.Length; i++)
            {
                allNotes[i].gameObject.SetActive(false);
            }
        }
    }
}
