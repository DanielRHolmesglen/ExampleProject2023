using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class GameData
{
    public int score = 0;
    public int numberOfLevelsUnlocked;

    //store the information of high scores into a array of 10
    public string[] highScorePlayerNames = new string[10];
    public int[] highScorePlayerKills = new int[10];
    public int[] highScorePlayerDeaths = new int[10];
    public int[] highScorePlayerWaves = new int[10];

    public void AddScore(int points)
    {
        score += points;
    }
    public void ResetData()
    {
        score = 0;
       
    }
    
}
