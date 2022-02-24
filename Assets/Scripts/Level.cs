using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New level", menuName = "ScriptableObjects/Level", order = 0)]
public class Level : ScriptableObject
{
    public int numberOfItems = 20;
    public int numberOfCounterfeits = 10;
    public int numberOfErrorsForFailure = 3;
    public Timer.TimeValue timeLimit = new Timer.TimeValue(0, 10, 0);
    // Defects
}
