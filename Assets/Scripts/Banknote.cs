using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Banknote : MonoBehaviour
{
    //[Header("Checks")]
    // Image is an appropriate material that doesn't crumple up
    // Image is embossed
    // Line printing is consistent
    // Microprint is present and readable
    // Bird image animates when tilted
    // Bird image changes colour when tilted
    // Note as a whole does not fluoresce
    // Serial number does fluoresce


    [Header("Graphic elements")]
    public Image background;
    public Image printedLines;
    public Image microprint;
    public Text serialNumber;
    public Image animatedGraphic;
    public Image clearCornerPatch;

    [Header("Variant effects")]
    public Material nonFluorescentMaterial;
    public Material fluorescentMaterial;
    public Sprite correctMicroprint;
    public Sprite[] faultyMicroprint;
    public Sprite correctClearPatch;
    public Sprite[] wrongClearPatches;

    bool fake = false;
    public bool Counterfeit
    {
        get
        {
            return fake;
        }
    }
    public void GenerateNote(bool counterfeit, Level currentLevel)
    {
        fake = counterfeit;
        if (counterfeit == false)
        {
            //Debug.Log(name + " is legitimate");
            ResetToLegitimate();
            return;
        }

        List<Defect> possibleDefects = FromFlags(currentLevel.defects);
        // Randomly select an option from available defects as specified by the 
        if (possibleDefects.Count > 0)
        {
            int index = Random.Range(0, possibleDefects.Count);
            ApplyDefect(possibleDefects[index]);
        }
    }
    void ApplyDefect(Defect value)
    {
        //Debug.Log("Applying defect " + value + " to " + name);
        switch(value)
        {
            case Defect.microprintIsIncorrect:
                int printIndex = Random.Range(0, faultyMicroprint.Length - 1);
                microprint.sprite = faultyMicroprint[printIndex];
                break;
            case Defect.imageNotPresentInClearPatch:
                int patchIndex = Random.Range(0, wrongClearPatches.Length - 1);
                clearCornerPatch.sprite = wrongClearPatches[patchIndex];
                break;
        }
    }
    void ResetToLegitimate()
    {
        microprint.sprite = correctMicroprint;
        clearCornerPatch.sprite = correctClearPatch;
    }
    [System.Flags]
    public enum Defect
    {
        microprintIsIncorrect = 1,
        imageNotPresentInClearPatch = 2,
        serialNumberIncorrectlyFormatted = 4,
        //holographicImagesDoNotAnimate = 8,
        //serialNumberDoesNotFluoresce = 16,
    }

    public static List<Defect> FromFlags(Defect flags)
    {
        List<Defect> list = new List<Defect>();
        // Fill a list with defects obtained from the list of flags for the current level
        foreach (Defect defect in System.Enum.GetValues(typeof(Defect)))
        {
            if (flags.HasFlag(defect))
            {
                list.Add(defect);
            }
        }
        return list;
    }
}
