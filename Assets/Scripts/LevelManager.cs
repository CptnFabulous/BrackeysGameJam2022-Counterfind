using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public Level current;

    [Header("Game elements")]
    public Banknote prefab;
    public ObjectViewer viewControls;
    [Header("HUD elements")]
    public Timer levelTimer;
    public Text remainingNotes;
    public Button acceptButton;
    public Button rejectButton;

    [Header("Transitioning to next note")]
    public Transform entryPilePosition;
    public Transform exitPilePosition;
    public float exitTime = 0.5f;
    public AnimationCurve exitAnimationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    
    public Banknote[] allNotes { get; private set; }
    public bool[] playerVerdicts { get; private set; }
    public int currentlyChecking { get; private set; }
    IEnumerator transition;

    private void Awake()
    {
        acceptButton.onClick.AddListener(() => JudgeItem(true));
        rejectButton.onClick.AddListener(() => JudgeItem(false));
    }
    private void Start()
    {
        GenerateNewLevel();
    }
    private void LateUpdate()
    {
        remainingNotes.text = (allNotes.Length - currentlyChecking - 1).ToString();
    }


    void GenerateNewLevel()
    {
        // Purge existing notes (code could probably be done better to prevent garbage collection but whatever it's a game jam)
        if (allNotes != null)
        {
            for (int i = 0; i < allNotes.Length; i++)
            {
                Destroy(allNotes[i].gameObject);
            }
        }
        
        List<Banknote> newNotes = new List<Banknote>();
        for (int i = 0; i < current.numberOfItems; i++)
        {
            // Spawn note object
            Banknote note = Instantiate(prefab, entryPilePosition);
            note.transform.localPosition = Vector3.zero;
            note.transform.localRotation = Quaternion.identity;
            // For a certain amount of new notes, mark as defective
            if (i < current.numberOfCounterfeits)
            {
                note.GenerateDefects();
            }
            // Insert at a random point to shuffle the array
            newNotes.Insert(Random.Range(0, newNotes.Count), note);
        }
        allNotes = newNotes.ToArray();
        playerVerdicts = new bool[allNotes.Length];
        currentlyChecking = -1;

        // Reset timer
        levelTimer.timeLimit = current.timeLimit;
        levelTimer.StartTimer();

        // Start transition to next item
        transition = TransitionToNextItem();
        StartCoroutine(transition);
    }
    
    IEnumerator TransitionToNextItem()
    {
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
    }
    IEnumerator PutAwayOldItem(Banknote oldNote)
    {
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
        viewControls.AddObject(newNote.transform);
        viewControls.OnReset();
        yield return new WaitWhile(() => viewControls.isResettingLook);

        acceptButton.interactable = true;
        rejectButton.interactable = true;
    }



    void JudgeItem(bool legitimate)
    {
        playerVerdicts[currentlyChecking] = legitimate;

        transition = TransitionToNextItem();
        StartCoroutine(transition);
    }
}
