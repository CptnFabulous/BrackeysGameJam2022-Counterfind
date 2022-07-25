using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class EndScreen : MonoBehaviour
{
    public ObjectToggleGroup toggleGroup;
    [Header("General")]
    public Text reason;
    public Button retryButton;
    public Button quitButton;
    
    public virtual void Awake()
    {
        toggleGroup?.allObjects.Add(gameObject);
        retryButton.onClick.AddListener(LevelProgressionHandler.Current.RetryLevel);
        quitButton.onClick.AddListener(LevelProgressionHandler.Current.ReturnToMenu);
    }
    public virtual void Generate()
    {
        toggleGroup?.SetActiveObject(gameObject);
        gameObject.SetActive(true);
        reason.text = "";
    }
}
