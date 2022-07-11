using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeMeterGenerator : MonoBehaviour
{
    public LifeMeter meter;
    public Image lifePrefab;
    public Image framePrefab;

    public Vector2 gridSpacing;
    public int maxRowSize = 5;
    public bool perpendicular;


    Image[] meterFrames;

    private void Awake()
    {
        lifePrefab.gameObject.SetActive(false);
        framePrefab.gameObject.SetActive(false);

        PopulateMeter();
    }

    public void PopulateMeter()
    {
        meter.lives = new Image[meter.numberOfLives];
        meterFrames = new Image[meter.numberOfLives];
        for (int i = 0; i < meter.numberOfLives; i++)
        {
            meter.lives[i] = Instantiate(lifePrefab, meter.transform);
            meterFrames[i] = Instantiate(framePrefab, meter.transform);
            meter.lives[i].gameObject.SetActive(true);
            meterFrames[i].gameObject.SetActive(true);

            Vector3 position = PositionFromIndex(i);
            //Vector3 rotation;
            meter.lives[i].rectTransform.anchoredPosition = position;
            meterFrames[i].rectTransform.anchoredPosition = position;

        }
    }

    Vector3 PositionFromIndex(int index)
    {
        Vector3 value = new Vector3();

        int fullRows = index / maxRowSize;
        //int fullRows = Mathf.FloorToInt(index / maxRowSize);
        int positionInRow = index % maxRowSize;

        // Swaps coordinates based on if rows are oriented perpendicular to normal (i.e. as columns)
        value.x = (perpendicular ? fullRows : positionInRow);
        value.y = (perpendicular ? positionInRow : fullRows);
        // Multiplies coordinates by grid spacing
        value.x *= gridSpacing.x;
        value.y *= gridSpacing.y;

        return value;
    }
}
