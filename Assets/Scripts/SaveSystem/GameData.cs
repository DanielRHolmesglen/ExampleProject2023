using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class GameData
{
    public int score = 0;
    public int numberOfLevelsUnlocked;

    public string[] currentPlayerNames;
    public int[] currentPlayerKills;
    public int[] currentPlayerDeaths;
    public int[] currentPlayerWaves;

    public void AddScore(int points)
    {
        score += points;
    }
    public void ResetData()
    {
        score = 0;
       
    }
}
