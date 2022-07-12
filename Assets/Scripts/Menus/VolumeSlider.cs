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
        slider.onValueChanged.AddListener((_)=> SetValue(slider.normalizedValue));
    }
    /// <summary>
    /// Logarithmically adjusts volume value using an input between 0 and 1.
    /// </summary>
    /// <param name="value"></param>
    public void SetValue(float value) => mixerToAlter.SetFloat(parameterName, LinearToLogarithmic(value));
    {
    }

    #region Converting between linear and logarithmic values
    /// <summary>
    /// Alters value to account for logarithmic sound scale
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static float LinearToLogarithmic(float value) => Mathf.Log10(ClampEpsilon(value)) * 20;
    /// <summary>
    /// Alters logarithmic volume value back into linear slider value
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static float LogarithmicToLinear(float value) => ClampEpsilon(Mathf.Pow(10, value / 20));
    /// <summary>
    /// Clamps between one and lowest non-zero value (logarithmic code is apparently weird with zero values)
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static float ClampEpsilon(float value) => Mathf.Clamp(value, float.Epsilon, 1);
    #endregion
}