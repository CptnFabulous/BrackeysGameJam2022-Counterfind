using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReferenceWindow : MonoBehaviour
{
    [System.Serializable]
    public struct DefectWarning
    {
        [HideInInspector] public string name;
        [TextArea] public string description;

        public DefectWarning(string newName)
        {
            name = newName;
            description = "";
        }
    }

    public Text listTextBox;
    public RectTransform regularInputs;
    public RectTransform window;
    public Button switchTo;
    public Button switchFrom;

    public string titleOfList = "Defects to check for:";
    public DefectWarning[] defectWarnings;

    public void OnValidate()
    {
        string[] names = System.Enum.GetNames(typeof(Banknote.Defect));
        List<DefectWarning> list = new List<DefectWarning>(defectWarnings);
        defectWarnings = new DefectWarning[names.Length];
        for (int i = 0; i < names.Length; i++)
        {
            // Look for an existing warning whose name matches the current name
            int index = list.FindIndex((w) => w.name == names[i]);
            if (index >= 0) // If one is found
            {
                defectWarnings[i] = list[index];
            }
            else
            {
                defectWarnings[i] = new DefectWarning(names[i]);
            }
        }
    }

    private void Awake()
    {
        switchTo.onClick.AddListener(() =>
        {
            regularInputs.gameObject.SetActive(false);
            window.gameObject.SetActive(true);
        });
        switchFrom.onClick.AddListener(() =>
        {
            window.gameObject.SetActive(false);
            regularInputs.gameObject.SetActive(true);
        });
    }


    public void Setup(Level currentLevel)
    {
        ShowPossibleDefects(currentLevel);
        switchFrom.onClick.Invoke();
    }
    public void ShowPossibleDefects(Level currentLevel)
    {
        string listOfThingsToWatchOutFor = titleOfList;

        List<Banknote.Defect> defects = Banknote.FromFlags(currentLevel.defects);

        for (int d = 0; d < defects.Count; d++)
        {
            for (int w = 0; w < defectWarnings.Length; w++)
            {
                if (defects[d].ToString() == defectWarnings[w].name)
                {
                    listOfThingsToWatchOutFor += "\n";
                    listOfThingsToWatchOutFor += defectWarnings[w].description;
                }
            }
        }

        listTextBox.text = listOfThingsToWatchOutFor;
    }
}