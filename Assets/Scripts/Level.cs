using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New level", menuName = "ScriptableObjects/Level", order = 0)]
public class Level : ScriptableObject
{
    public int numberOfItems;
    public int numberOfCounterfeits;
    public int numberOfErrorsForFailure = 3;
    public Timer.TimeValue timeLimit = new Timer.TimeValue(0, 10, 0);
    // Defects
}
