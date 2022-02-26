using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectViewer : MonoBehaviour
{
    public Transform viewedObject;

    [Header("Positioning")]
    public float panSensitivity = 2;
    public float zoomSensitivity = 1;
    public Bounds positionBounds = new Bounds(Vector3.forward * 10, Vector3.one * 10);

    [Header("Rotation")]
    public float rotationSensitivity = 30;
    public Vector3 defaultEulerAngles;

    [Header("Reset")]
    public UnityEngine.UI.Button resetButton;
    public float resetTime = 0.5f;
    public AnimationCurve resetCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private void Awake()
    {
        resetButton.onClick.AddListener(OnReset);
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
        if (controlDenied)
        {
            return;
        }
        
        Vector2 value = input.Get<Vector2>();

        if (isPanning)
        {
            Vector3 values = panSensitivity * Time.deltaTime * value;
            viewedObject.localPosition = viewedObject.localPosition + values;
            ClampPosition();
        }
        if (isRotating)
        {
            value = new Vector2(value.y, value.x);
            viewedObject.Rotate(rotationSensitivity * Time.deltaTime * value, Space.Self);
            Vector3 direction = viewedObject.localRotation * Vector3.forward;
            viewedObject.localRotation = Quaternion.LookRotation(direction, Vector3.up);
        }
    }
    public void OnZoom(InputValue input)
    {
        if (controlDenied)
        {
            return;
        }
        
        Vector2 value = input.Get<Vector2>().normalized;
        Vector3 values = value.y * Time.deltaTime * Vector3.back;
        viewedObject.localPosition = viewedObject.localPosition + values;
        ClampPosition();
    }
    public void OnReset()
    {
        if (controlDenied)
        {
            return;
        }
        resetInProgress = ResetLook();
        StartCoroutine(resetInProgress);
    }

    public void AddObject(Transform newObject)
    {
        viewedObject = newObject;
        newObject.parent = transform;
        OnReset();
    }

    IEnumerator ResetLook()
    {
        Vector3 oldPosition = viewedObject.localPosition;
        Quaternion oldRotation = viewedObject.localRotation;
        Quaternion newRotation = Quaternion.Euler(defaultEulerAngles);

        for (float timer = 0; timer < 1; timer = Mathf.Clamp01(timer + Time.deltaTime / resetTime))
        {
            float t = resetCurve.Evaluate(timer);
            viewedObject.localPosition = Vector3.Lerp(oldPosition, positionBounds.center, t);
            viewedObject.localRotation = Quaternion.Lerp(oldRotation, newRotation, t);
            yield return null;
        }

        resetInProgress = null;
    }
    IEnumerator resetInProgress;
    bool isPanning;
    bool isRotating;
    public bool controlDenied
    {
        get
        {
            return enabled == false || isResetting;
        }
    }
    public bool isResetting
    {
        get
        {
            return resetInProgress != null;
        }
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
