using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicHandler : MonoBehaviour
{
    #region Singleton data
    public static MusicHandler Current
    {
        get
        {
            if (c == null)
            {
                c = FindObjectOfType<MusicHandler>();
            }
            return c;
        }
    }
    static MusicHandler c;
    #endregion

    #region MusicState enum and MusicStateData struct
    public enum MusicState
    {
        Title,
        Menus,
        Idle,
        Active,
        Win,
        Fail
    }
    [System.Serializable]
    public struct MusicStateData
    {
        [HideInInspector] public string name;
        public AudioClip[] tracks;
        public bool looping;

        public MusicStateData(string name)
        {
            this.name = name;
            tracks = new AudioClip[0];
            looping = true;
        }
    }
    #endregion

    public MusicState currentState { get; private set; }
    public MusicStateData[] musicForStates;

    public AudioSource audioPlayer
    {
        get
        {
            if (ap == null) ap = GetComponent<AudioSource>();
            return ap;
        }
    }
    AudioSource ap;
    

    private void OnValidate()
    {
        string[] names = System.Enum.GetNames(typeof(MusicState)); // Obtains current enum values by name
        List<MusicStateData> oldStates = new List<MusicStateData>(musicForStates); // Makes new searchable list based on old data
        musicForStates = new MusicStateData[names.Length]; // Wipes array and sets to appropriate length

        for (int i = 0; i < names.Length; i++) // For each state type
        {
            int index = oldStates.FindIndex((d) => d.name == names[i]); // Checks if data already exists for a state
            musicForStates[i] = (index >= 0) ? oldStates[index] : new MusicStateData(names[i]); // If so, copy it. If not, make a new one
        }
    }

    public void SwitchStateFromString(string stateName)
    {
        string[] enumNames = System.Enum.GetNames(typeof(MusicState));
        for (int i = 0; i < enumNames.Length; i++)
        {
            if (stateName == enumNames[i])
            {
                SmashCut((MusicState)i);
                return;
            }
        }
        Debug.Log(this + " does not recognise state: " + stateName);
    }
    public void SmashCut(MusicState newState)
    {
        currentState = newState;
        MusicStateData newStateData = musicForStates[(int)newState];
        AudioClip[] tracks = newStateData.tracks;
        AudioClip newTrack = tracks.Length > 0 ? tracks[Random.Range(0, tracks.Length)] : null;
        if (newTrack == null)
        {
            Debug.Log("No track available for state " + newState + "!");
            audioPlayer.Stop();
            return;
        }

        audioPlayer.clip = newTrack;
        audioPlayer.loop = newStateData.looping;
        audioPlayer.Play();
    }
    public void StopMusic() => audioPlayer.Stop();

}
