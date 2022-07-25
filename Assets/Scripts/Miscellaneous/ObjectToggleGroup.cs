using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectToggleGroup : MonoBehaviour
{
    public List<GameObject> allObjects;
    public GameObject current { get; private set; }

    public void SetActiveObject(GameObject objectToEnable)
    {
        if (objectToEnable != null && allObjects.Contains(objectToEnable) == false) return;

        for (int i = 0; i < allObjects.Count; i++)
        {
            allObjects[i]?.SetActive(allObjects[i] == objectToEnable); // Set active if true
        }
        current = objectToEnable;
        // Disabling everything at once then enabling the correct one might be easier
        // but will result in OnDisable and OnEnable being run unnecessarily
        // in the event that the desired object is already active
    }

    private void Awake() => instances.Add(this);
    private void OnDestroy() => instances.Remove(this);

    static List<ObjectToggleGroup> instances = new List<ObjectToggleGroup>();
    public static void SetActive(GameObject objectToEnable)
    {
        ObjectToggleGroup instance = instances.Find((i) => i.allObjects.Contains(objectToEnable));
        if (instance == null)
        {
            objectToEnable.SetActive(true);
            return;
        }
        instance.SetActiveObject(objectToEnable);
    }
}
