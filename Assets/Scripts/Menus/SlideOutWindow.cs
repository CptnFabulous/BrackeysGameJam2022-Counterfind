using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class SlideOutWindow : MonoBehaviour
{
    public Transform hiddenOrientation;
    public Transform shownOrientation;

    [Header("References")]
    public Button showButton;
    public Button hideButton;
    public ToggleTransition transitionHandler;


    private void Awake()
    {
        transitionHandler.effect.AddListener(Effect);

        if (showButton == hideButton) // If show and hide buttons are the same, make it a toggle
        {
            showButton.onClick.AddListener(() => transitionHandler.active = !transitionHandler.active);
        }
        else
        {
            showButton.onClick.AddListener(() => transitionHandler.active = true);
            hideButton.onClick.AddListener(() => transitionHandler.active = false);
        }

        Effect(0);
    }

    public void Effect(float t)
    {
        transform.position = Vector3.Lerp(hiddenOrientation.position, shownOrientation.position, t);
        transform.rotation = Quaternion.Lerp(hiddenOrientation.rotation, shownOrientation.rotation, t);
    }
}
