using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class SaveHandler
{
    public struct SaveFile
    {
        public int numberOfGamesPlayed;
        public int highScore;
        public float bestAccuracyRatio;
        public float longestGameDuration;
        public float longestStreak;

        public int completedLevelIndex;
        public float[] bestLevelTimes;
        public bool[] perfectScores;
    }

    public static SaveFile currentData;
    static int saveIndex = 0;

    /// <summary>
    /// Invoked whenever a save file is loaded.
    /// </summary>
    public static System.Action<SaveFile> onLoadGame;
    /// <summary>
    /// Invoked whenever the game is saved.
    /// </summary>
    public static System.Action<SaveFile> onSaveGame;

    /// <summary>
    /// Saves data held in currentData to a JSON file.
    /// </summary>
    public static void Save()
    {
        string jsonString = JsonUtility.ToJson(currentData);
        StreamWriter streamWriter = new StreamWriter(FilePath(saveIndex));
        streamWriter.Write(jsonString);
        onSaveGame.Invoke(currentData);
    }
    /// <summary>
    /// Loads data from the correct JSON file into currentData.
    /// </summary>
    public static void Load()
    {
        StreamReader streamReader = new StreamReader(FilePath(saveIndex));
        string jsonString = streamReader.ReadToEnd();
        currentData = JsonUtility.FromJson<SaveFile>(jsonString);
        onLoadGame.Invoke(currentData);
    }

    static string FilePath(int index) => Application.persistentDataPath + "/" + fileName + index + extension;
    const string fileName = "SaveData";
    const string extension = ".json";
}
