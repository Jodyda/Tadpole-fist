using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance = null;

   // public int score, highScore;

    public Text scoreText, highScoreText;

    private HighScore currentHighScore;

    private string scores = "HighScores.json";

    private string path;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        // DontDestroyOnLoad(gameObject);
    }


    void Start()
    {
        path = Application.dataPath + "/highscores";
        highScoreText.text = GameManager.instance.highScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddScore()
    {
        GameManager.instance.score++;

        //Debug.Log("score " + GameManager.instance.score + GameManager.instance.highScore);

        UpdateHighScore();

        scoreText.text = GameManager.instance.score.ToString();
    }

    public void UpdateHighScore()
    {
        
        if (GameManager.instance.score > GameManager.instance.highScore)
        {
            GameManager.instance.highScore = GameManager.instance.score;
            //Debug.Log(GameManager.instance.highScore + "update");
            highScoreText.text = GameManager.instance.highScore.ToString();
        }
    }

    public void ResetScore()
    {
        GameManager.instance.score = 0;
    }

    public void Save()
    {
        if (GameManager.instance.highScore == 0) 
        {
            //Debug.Log("Sparar");
            Load();
        }
        print(GameManager.instance.highScore);
        currentHighScore = new HighScore { hScore = GameManager.instance.highScore };
        string json = JsonUtility.ToJson(currentHighScore);
        StreamWriter writer = new StreamWriter(path + "/" + scores);
        writer.WriteLine(json);
        writer.Close();
        //Debug.Log("Save");
    }

    public void Load()
    {
        try
        {
            path = Application.dataPath + "/highscores";
            Directory.CreateDirectory(path);
            StreamReader reader = new StreamReader(path + "/" + scores);
            string data = reader.ReadToEnd();
            currentHighScore = JsonUtility.FromJson<HighScore>(data);
            reader.Close();
        } catch(FileNotFoundException e)
        {
            if (currentHighScore == null)
            {
                currentHighScore = new HighScore();
                string json = JsonUtility.ToJson(currentHighScore);
                Directory.CreateDirectory(path);
                StreamWriter writer = new StreamWriter(path + "/" + scores);
                writer.WriteLine(json);
                writer.Close();
            }
        }
        if ( GameManager.instance.highScore < currentHighScore.hScore )
        {
            GameManager.instance.highScore = currentHighScore.hScore;
            highScoreText.text = GameManager.instance.highScore.ToString();
        }
        //Debug.Log("Laddar");
    
    }
}

[System.Serializable]
public class HighScores
{
    public HighScore scores = new HighScore();
}

[System.Serializable]
public class HighScore
{
    public int hScore;
}
