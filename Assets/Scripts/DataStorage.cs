using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Runtime.ExceptionServices;
using Microsoft.SqlServer.Server;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SocialPlatforms.Impl;

public class DataStorage : MonoBehaviour
{
    private static DataStorage _INSTANCE;

    public static DataStorage getInstance()
    {
        return _INSTANCE;
    }

    private string username;
    private List<HighscoreEntry> highscores = new List<HighscoreEntry>();

    public string Username
    {
        get
        {
            return username;
        }
        set
        {
            username = value;
        }
    }

    private void Awake()
    {
        if (_INSTANCE == null)
        {
            _INSTANCE = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
    }
    public string AddScore(int score)
    {
        bool isHighscore = false;
        int index = 666;
        for (int i = 0; i < highscores.Count; i++)
        {
            if (highscores[i].score <= score)
            {
                index = i;
                break;
            }
        }
        if (index == 666)
        {
            index = highscores.Count;
        }
        if (index < 666)
        {
            isHighscore = true;
            HighscoreEntry hse = new HighscoreEntry(username, score);
            highscores.Insert(index, hse);
            Save();
        }
        return (isHighscore) ? (index + 1) + ". " + score + " - " + username : null;
    }
    public void Save()
    {
        SaveData data = new SaveData();
        data.username = username;
        data.highscores = highscores;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void Load()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            username = data.username;
            highscores = data.highscores;
        }
    }

    public string GetBestScoreText()
    {
        string s = "No Score yet";
        if (highscores.Count > 0)
        {
            s = "1. " + highscores[0].ToString();
        } else {
            if (username != null)
            {
                s = username + ", no score yet.";
            }
        }
        return s;
    }

    [Serializable]
    private class HighscoreEntry
    {
        public string name;
        public int score;

        public HighscoreEntry(string name, int score)
        {
            this.name = name;
            this.score = score;
        }

        override public string ToString()
        {
            string s = score + " - " + name;
            return s;
        }
    }

    [Serializable]
    private class SaveData
    {
        public List<HighscoreEntry> highscores = new List<HighscoreEntry>();
        public string username;
    }
}
