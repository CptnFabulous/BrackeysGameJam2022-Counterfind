using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleTransition : MonoBehaviour
{
    public float transitionTime = 0.25f;
    public AnimationCurve transitionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public UnityEngine.Events.UnityEvent<float> effect;

    public bool startActive = false;

    bool currentlyActive;
    float currentValue;
    IEnumerator currentTransition;

    float stateTarget => active ? 1 : 0; // What the timer value is transitioning towards
    bool mustTransition => currentValue != stateTarget;
    public bool active
    {
        get => currentlyActive;
        set
        {
            // Start the transition coroutine, but only if an existing transition coroutine is not active
            // The transition coroutine will automatically shift to the desired value if it changes
            currentlyActive = value;
            if (mustTransition && currentTransition == null)
            {
                currentTransition = Transition();
                StartCoroutine(currentTransition);
            }
        }
    }

    private void Awake()
    {
        SetEffectImmediately(startActive);
    }

    public void SetEffectImmediately(bool nowActive)
    {
        effect.Invoke(startActive ? 1 : 0);
        currentlyActive = nowActive;
    }


    IEnumerator Transition()
    {
        while (mustTransition)
        {
            currentValue = Mathf.MoveTowards(currentValue, stateTarget, Time.deltaTime / transitionTime);
            float t = transitionCurve.Evaluate(currentValue);
            effect.Invoke(t); // Invoke function

            yield return null;
        }

        currentTransition = null; // Nulls value at the end so the 'active' setter can tell there's no coroutine active
    }
}
