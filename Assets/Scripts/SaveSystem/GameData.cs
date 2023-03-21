using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class GameData
{
    public int score = 0;
    public int numberOfLevelsUnlocked;

    //list of current player information
    public List<PlayerData> currentPlayers = new List<PlayerData>();

    //store the information of high scores into a array of 10
    public string[] highScorePlayerNames = new string[10];
    public int[] highScorePlayerKills = new int[10];
    public int[] highScorePlayerDeaths = new int[10];
    public int[] highScorePlayerWaves = new int[10];

    public List<PlayerData> tempScoreBoard = new List<PlayerData>();

    public void FillTempList()
    {
        for(int i = 0; i < 10; i++)
        {
            PlayerData data = new PlayerData();
            data.playerName = highScorePlayerNames[i];
            data.kills = highScorePlayerKills[i];
            data.deaths = highScorePlayerDeaths[i];
            data.wavesSurvived = highScorePlayerWaves[i];
            tempScoreBoard.Add(data);
        }
    }
    public void FillSaveData()
    {
        tempScoreBoard.Sort(SortPlayerFunc);

        for (int i = 0; i < 10; i++)
        {
            highScorePlayerNames[i] = tempScoreBoard[i].playerName;
            highScorePlayerKills[i] = tempScoreBoard[i].kills;
            highScorePlayerDeaths[i] = tempScoreBoard[i].deaths;
            highScorePlayerWaves[i] = tempScoreBoard[i].wavesSurvived;
        }
    }
    int SortPlayerFunc(PlayerData a, PlayerData b)
    {
        if (a.wavesSurvived < b.wavesSurvived)
        {
            return +1;
        }
        else if (a.wavesSurvived > b.wavesSurvived)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }
    public void AddPlayerScore(PlayerData data)
    {
        if (tempScoreBoard.Contains(data))
        {
            return;
        }
        else
        {
            tempScoreBoard.Add(data);
            FillSaveData();
        }
    }
    public void AddScore(int points)
    {
        score += points;
    }
    public void ResetData()
    {
        score = 0;
       
    }
}
