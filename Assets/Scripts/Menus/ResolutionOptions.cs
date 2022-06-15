using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionOptions : MonoBehaviour
{
    public Dropdown GUIDropdown;

    private void Awake()
    {
        SetupResolutionTable();
        SetupResolutionDropdown(GUIDropdown);
    }

    public static List<Resolution> availableResolutions { get; private set; }
    public static int maxRefreshRate { get; private set; }
    public static void SetupResolutionTable()
    {
        List<Resolution> allResolutions = new List<Resolution>(Screen.resolutions);
        allResolutions.Sort((lhs, rhs) => lhs.refreshRate.CompareTo(rhs.refreshRate));
        maxRefreshRate = allResolutions[allResolutions.Count - 1].refreshRate;
        availableResolutions = new List<Resolution>();
        while (allResolutions.Count > 0)
        {
            Resolution res = allResolutions[0];
            availableResolutions.Add(res);
            allResolutions.RemoveAll((r) => r.width == res.width && r.height == res.height);
        }
        availableResolutions.Sort((a, b) => (a.width * a.height).CompareTo(b.width * b.height));
    }
    public static void ChangeResolution(int index)
    {
        Resolution toUse = availableResolutions[index];
        Screen.SetResolution(toUse.width, toUse.height, Screen.fullScreen, maxRefreshRate);
    }
    public static void SetupResolutionDropdown(Dropdown dropdown)
    {
        int currentValue = 0;
        List<string> resolutionNames = new List<string>();
        for (int i = 0; i < availableResolutions.Count; i++)
        {
            int width = availableResolutions[i].width;
            int height = availableResolutions[i].height;

            if (Screen.currentResolution.width == width && Screen.currentResolution.height == height)
            {
                currentValue = i;
            }

            resolutionNames.Add(width + " X " + height);
        }

        dropdown.ClearOptions();
        dropdown.AddOptions(resolutionNames);

        dropdown.value = currentValue;

        dropdown.onValueChanged.AddListener(ChangeResolution);
    }
}
