using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Banknote : MonoBehaviour
{
    //[Header("Checks")]
    // Microprint is present and readable


    [Header("Graphic elements")]
    public Image background;
    public Image printedLines;
    public Image microprint;
    public Text serialNumber;
    public Image reflectiveGraphic;
    public Image preciselyProportionedGraphic;
    public Image clearCornerPatch;

    [Header("Variant effects")]
    //public Material nonFluorescentMaterial;
    //public Material fluorescentMaterial;
    public Sprite correctMicroprint;
    public Sprite[] faultyMicroprint;
    public Sprite correctClearPatch;
    public Sprite[] wrongClearPatches;
    //public string identicalSerialNumber = "SN1138538045";
    public Material reflectiveMaterial;
    public Material wrongReflectiveMaterial;
    //public Material iridescentMaterial;
    //public Material nonIridescentMaterial;

    bool fake = false;
    public bool Counterfeit
    {
        get
        {
            return fake;
        }
    }
    public void GenerateNote(bool counterfeit, Defect defects/*Level currentLevel*/)
    {
        ResetToLegitimate(); // Reset values to defaults before defects are applied

        fake = counterfeit; // Assigns value
        if (counterfeit == false)
        {
            return;
        }

        List<Defect> possibleDefects = FromFlags(defects);
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
            case Defect.serialNumberIncorrectlyFormatted:
                serialNumber.text = NumberString(12);
                break;
                /*
            case Defect.graphicBadlyProportioned:
                preciselyProportionedGraphic.preserveAspect = false;
                break;
                */
                /*
            case Defect.cornerImageNotReflective:
                reflectiveGraphic.material = wrongReflectiveMaterial;
                break;
                */
        }
    }
    void ResetToLegitimate()
    {
        microprint.sprite = correctMicroprint;
        clearCornerPatch.sprite = correctClearPatch;
        serialNumber.text = CorrectSerialNumber();
        //preciselyProportionedGraphic.preserveAspect = true;
        //reflectiveGraphic.material = reflectiveMaterial;
    }


    string CorrectSerialNumber()
    {
        return "SN" + NumberString(10);
    }
    string NumberString(int length)
    {
        string serialNumber = "";
        for (int i = 0; i < length; i++)
        {
            serialNumber += Random.Range(0, 10);
        }
        return serialNumber;
    }

    [System.Flags]
    public enum Defect
    {
        microprintIsIncorrect = 1,
        imageNotPresentInClearPatch = 2,
        serialNumberIncorrectlyFormatted = 4,
        graphicBadlyProportioned = 8
        //cornerImageNotReflective = 8,
        //holographicImagesDoNotAnimate
        //serialNumberDoesNotFluoresce
        // Image is an appropriate material that doesn't crumple up
        // Image is embossed
        // Line printing is consistent
        // Bird image animates when tilted
        // Bird image changes colour when tilted
        // Note as a whole does not fluoresce
        // Serial number does fluoresce
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
