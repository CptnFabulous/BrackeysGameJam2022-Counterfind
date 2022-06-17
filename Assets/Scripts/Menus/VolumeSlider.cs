using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

// Used this tutorial as reference: https://johnleonardfrench.com/the-right-way-to-make-a-volume-slider-in-unity-using-logarithmic-conversion/

[RequireComponent(typeof(Slider))]
public class VolumeSlider : MonoBehaviour
{
    public AudioMixer mixerToAlter;
    public string parameterName;
    Slider slider;

    private void Awake()
    {
        // References slider and adjusts variables to set up operation
        slider = GetComponent<Slider>();
        slider.minValue = float.Epsilon;
        slider.maxValue = 1;
        slider.onValueChanged.AddListener(SetValue);
    }
    /// <summary>
    /// Logarithmically adjusts volume value using an input between 0 and 1.
    /// </summary>
    /// <param name="value"></param>
    public void SetValue(float value)
    {
        value = Mathf.Clamp(value, float.Epsilon, 1); // Clamps between one and lowest non-zero value (logarithmic code is apparently weird with zero values)
        value = Mathf.Log10(value) * 20; // Alters value to account for logarithmic sound scale
        mixerToAlter.SetFloat(parameterName, value);
    }
}