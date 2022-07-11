using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeMeter : MonoBehaviour
{
    public int numberOfLives;
    public Image[] lives;

    private void OnValidate()
    {
        if (lives.Length != numberOfLives)
        {
            Image[] newLives = new Image[numberOfLives];
            for (int i = 0; i < numberOfLives; i++)
            {
                if (i < lives.Length)
                {
                    newLives[i] = lives[i];
                }
            }
            lives = newLives;
        }
    }

    public void UpdateMeter(float value)
    {
        for (int i = 0; i < numberOfLives; i++)
        {
            float amountToSubtract = Mathf.Min(value, 1);
            value -= amountToSubtract;
            lives[i].fillAmount = amountToSubtract;
        }
    }
}
