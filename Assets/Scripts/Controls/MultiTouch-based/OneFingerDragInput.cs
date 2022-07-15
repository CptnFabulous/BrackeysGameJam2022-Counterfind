using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneFingerDragInput : AdvancedTouchInput
{
    //public MultiTouch inputHandler;

    public UnityEngine.Events.UnityEvent<Vector2> onDrag;
    protected override int requiredNumberOfTouches => 1;
    protected override bool InputsHaveChanged() => newFingerPosition != oldFingerPosition;
    public Vector2 newFingerPosition => inputHandler.positions[0];
    public Vector2 oldFingerPosition { get; private set; }

    protected override bool InTouchZone() => RectTransformUtility.RectangleContainsScreenPoint(touchZone, newFingerPosition);
    protected override void ProcessInputs()
    {
        Vector2 scaledPositionDifference = (newFingerPosition - oldFingerPosition) / screenScale;
        onDrag.Invoke(scaledPositionDifference);
    }
    protected override void ResetOldInputs() => oldFingerPosition = newFingerPosition;
}