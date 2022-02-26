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

    [Header("Variant effects")]
    public Material nonFluorescentMaterial;
    public Material fluorescentMaterial;

    public void GenerateDefects()
    {
        fake = true;
    }
    bool fake = false;
    public bool Counterfeit
    {
        get
        {
            return fake;
        }
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
}
