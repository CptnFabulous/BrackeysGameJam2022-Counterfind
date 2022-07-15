using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectViewer : MonoBehaviour
{
    
    [Header("Setup")]
    public Camera viewingCamera;
    public Bounds positionBounds = new Bounds(Vector3.forward * 10, Vector3.one * 10);
    public Vector3 defaultPosition;
    public Vector3 defaultEulerAngles;

    [Header("Resetting object view")]
    public float resetTime = 0.5f;
    public AnimationCurve resetCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Mouse controls")]
    public float mousePanSensitivity = 2;
    public float mouseZoomSensitivity = 1;
    public float mouseRotationSensitivity = 30;

    [Header("Touch controls")]
    public OneFingerDragInput movement;
    public TwoFingerTouchInput rotationAndZoom;
    public UnityEngine.UI.Button resetButton;
    public float touchZoomSensitivity = 5;
    public float touchRotationSensitivityXY = 30;

    public Transform viewedObject { get; private set; }
    IEnumerator resetInProgress;
    bool isPanning;
    bool isRotating;
    public bool controlDenied => enabled == false || isResetting;
    public bool isResetting => resetInProgress != null;

    #region Built-in Unity-functions
    private void Awake()
    {
        // Assign listeners to mobile controls
        movement?.onDrag.AddListener((_) => Pan(movement.oldFingerPosition, movement.newFingerPosition));
        rotationAndZoom?.onDrag.AddListener(RotateXY);
        rotationAndZoom?.onRotate.AddListener(RotateZ);
        rotationAndZoom?.onPinch.AddListener((i) => Zoom(i, touchZoomSensitivity));
        resetButton?.onClick.AddListener(ResetViewOrientation);
    }
    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(positionBounds.center, positionBounds.size);
    }
    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        
    }
    #endregion

    #region Mouse+KB inputs
    public void OnPanControl(InputValue input)
    {
        isPanning = input.isPressed;
        isRotating = false;
    }
    public void OnRotateControl(InputValue input)
    {
        isRotating = input.isPressed;
        isPanning = false;
    }
    public void OnMove(InputValue input)
    {
        if (controlDenied) return;

        Vector2 value = input.Get<Vector2>();

        if (isPanning) Pan(value, mousePanSensitivity);
        if (isRotating) RotateXYGimbalClamped(value);
    }
    public void OnZoom(InputValue input)
    {
        if (controlDenied) return;
        
        Vector2 value = input.Get<Vector2>().normalized;
        Zoom(value.y, mouseZoomSensitivity);
    }
    #endregion

    #region Rotation functions
    public void Pan(Vector2 input, float sensitivity)
    {
        Vector3 values = sensitivity * Time.deltaTime * input;
        viewedObject.localPosition = viewedObject.localPosition + values;
        ClampPosition();
    }
    public void Pan(Vector2 oldScreenPosition, Vector2 newScreenPosition)
    {
        float distance = Vector3.Distance(viewingCamera.transform.position, viewedObject.transform.position);
        Vector3 oldWorldPoint = AdvancedTouchInput.ScreenToWorldPoint(viewingCamera, oldScreenPosition, distance);
        Vector3 newWorldPoint = AdvancedTouchInput.ScreenToWorldPoint(viewingCamera, newScreenPosition, distance);
        viewedObject.transform.Translate(newWorldPoint - oldWorldPoint, Space.World);
    }
    public void RotateXYGimbalClamped(Vector2 input)
    {
        input = new Vector2(input.y, input.x);
        viewedObject.Rotate(mouseRotationSensitivity * Time.deltaTime * input, Space.Self);
        Vector3 direction = viewedObject.localRotation * Vector3.forward;
        viewedObject.localRotation = Quaternion.LookRotation(direction, Vector3.up);
    }
    public void RotateXY(Vector2 input)
    {
        input *= touchRotationSensitivityXY;
        Vector3 position = viewedObject.transform.position;
        viewedObject.RotateAround(position, viewingCamera.transform.up, -input.x);
        viewedObject.RotateAround(position, viewingCamera.transform.right, input.y);
    }
    public void RotateZ(float angleDelta)
    {
        Vector3 axis = viewedObject.position - viewingCamera.transform.position;
        viewedObject.RotateAround(viewedObject.position, axis, angleDelta);
    }
    public void Zoom(float input, float sensitivity)
    {
        Vector3 values = input * sensitivity * Time.deltaTime * Vector3.back;
        viewedObject.localPosition = viewedObject.localPosition + values;
        ClampPosition();
    }
    public void ResetViewOrientation()
    {
        if (controlDenied) return;

        resetInProgress = ResetLook();
        StartCoroutine(resetInProgress);
    }
    public void SetObject(Transform newObject)
    {
        viewedObject = newObject;
        if (newObject != null)
        {
            newObject.parent = transform;
            ResetViewOrientation();
        }
    }
    #endregion

    IEnumerator ResetLook()
    {
        Vector3 oldPosition = viewedObject.localPosition;
        Vector3 newPosition = positionBounds.center + defaultPosition;

        Quaternion oldRotation = viewedObject.localRotation;
        Quaternion newRotation = Quaternion.Euler(defaultEulerAngles);

        for (float timer = 0; timer < 1; timer = Mathf.Clamp01(timer + Time.deltaTime / resetTime))
        {
            float t = resetCurve.Evaluate(timer);
            viewedObject.localPosition = Vector3.Lerp(oldPosition, newPosition, t);
            viewedObject.localRotation = Quaternion.Lerp(oldRotation, newRotation, t);
            yield return null;
        }

        resetInProgress = null;
    }
    void ClampPosition()
    {
        viewedObject.transform.localPosition = Vector3Clamp(viewedObject.transform.localPosition, positionBounds.min, positionBounds.max);
    }
    public static Vector3 Vector3Clamp(Vector3 original, Vector3 min, Vector3 max)
    {
        original.x = Mathf.Clamp(original.x, min.x, max.x);
        original.y = Mathf.Clamp(original.y, min.y, max.y);
        original.z = Mathf.Clamp(original.z, min.z, max.z);
        return original;
    }
}
