using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Camera viewCamera;
    public GameStateHandler stateHandler;

    [Header("Game controls")]
    public ObjectViewer objectViewer;
    public Button judgeReal;
    public Button judgeFake;
    public ReferenceWindow referenceWindow;

    [Header("Game HUD")]
    public Text timerDisplay;
    public Text noteCounter;

    [Header("Data")]
    public LivesHandler lives;
    public ScoreHandler score;
}
