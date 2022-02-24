using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [System.Serializable]
    public struct TimeValue
    {
        public int hours;
        public int minutes;
        public float seconds;
        public float InSeconds
        {
            get
            {
                return (hours * 3600) + (minutes * 60) + seconds;
            }
        }

        public TimeValue(int h, int m, float s)
        {
            hours = h;
            minutes = m;
            seconds = s;
        }
        public TimeValue(float totalTimeInSeconds)
        {
            float remaining = totalTimeInSeconds;
            hours = Mathf.FloorToInt(remaining / 3600);
            remaining = remaining % 3600;
            minutes = Mathf.FloorToInt(remaining / 60);
            remaining = remaining % 60;
            seconds = remaining;
        }

        public override string ToString()
        {
            return hours + ":" + minutes + ":" + seconds;
        }
    }

    public TimeValue timeLimit = new TimeValue(1, 0, 0);
    public bool goIntoNegatives = false;
    public UnityEvent onTimerStart;
    public UnityEvent onTimeUp;
    public UnityEngine.UI.Text visualTimer;

    float remainingTimeInSeconds;
    bool timeIsUp;
    public TimeValue remaining
    {
        get
        {
            return new TimeValue(remainingTimeInSeconds);
        }
    }

    public void StartTimer()
    {
        remainingTimeInSeconds = timeLimit.InSeconds;
        timeIsUp = false;
        onTimerStart.Invoke();
        Resume();
    }
    public void Pause()
    {
        enabled = false;
    }
    public void Resume()
    {
        enabled = true;
    }

    void Start()
    {
        StartTimer();
    }
    void Update()
    {
        remainingTimeInSeconds -= Time.deltaTime;
        if (remainingTimeInSeconds <= 0 && timeIsUp == false)
        {
            timeIsUp = true;
            onTimeUp.Invoke();
            if (goIntoNegatives == false)
            {
                remainingTimeInSeconds = 0;
                Pause();
            }
        }
    }
    private void LateUpdate()
    {
        if (visualTimer != null)
        {
            visualTimer.text = remaining.ToString();
        }
    }


}
