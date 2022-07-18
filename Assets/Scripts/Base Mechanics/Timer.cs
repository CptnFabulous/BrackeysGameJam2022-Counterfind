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
        public float inSeconds => (hours * 3600) + (minutes * 60) + seconds;
        public override string ToString() => ToString(true);
        public string ToString(bool showDecimalPlaces)
        {
            string text = "";

            if (hours < 10) text += "0";
            text += hours + ":";
            if (minutes < 10) text += "0";
            text += minutes + ":";
            if (seconds < 10) text += "0";
            text += showDecimalPlaces ? seconds : Mathf.FloorToInt(seconds);

            return text;
        }

        public TimeValue(int hours, int minutes, float seconds)
        {
            this.hours = hours;
            this.minutes = minutes;
            this.seconds = seconds;
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
    }

    public TimeValue parTime;
    public bool stopWhenExpired;
    public UnityEvent onTimerStart;
    public UnityEvent onTimeUp;
    public float secondsRemaining => parTime.inSeconds - secondsElapsed;
    public bool pastParTime => secondsElapsed >= parTime.inSeconds;
    public TimeValue elapsed => new TimeValue(secondsElapsed);
    public TimeValue remaining => new TimeValue(secondsRemaining);
    public float secondsElapsed { get; private set; }
    public float startTime { get; private set; }

    private void OnEnable()
    {
        if (secondsElapsed == 0)
        {
            startTime = Time.time;
            onTimerStart.Invoke();
        }
    }
    private void Update()
    {
        secondsElapsed += Time.deltaTime;
        if (stopWhenExpired && pastParTime)
        {
            Pause();
            onTimeUp.Invoke();
        }
    }

    public void StartTimer()
    {
        ResetTimer();
        Play();
    }
    public void ResetTimer() => secondsElapsed = 0;
    public void Play() => enabled = true;
    public void Pause() => enabled = false;
}


