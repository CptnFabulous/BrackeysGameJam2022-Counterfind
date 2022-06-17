using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaceHandler : MonoBehaviour
{
    public int checkTargetPerMinute = 10;



    List<float> checkTimes;



    public void OnCheck()
    {
        checkTimes.Add(Time.time);

        checkTimes.RemoveAll((t) => Time.time - t > 60);

        if (checkTimes.Count < checkTargetPerMinute)
        {
            // Not enough checks in the list means the player is not checking the appropriate amount per minute. Penalise!
        }
    }
}
