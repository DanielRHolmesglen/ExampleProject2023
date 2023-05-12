using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameData saveData;

    //list of current player information
    public List<PlayerData> currentPlayers = new List<PlayerData>();

    public List<PlayerData> tempScoreBoard = new List<PlayerData>();

    // Start is called before the first frame update
    void Start()
    {
        #region Singleton
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        #endregion

        //attempt to load a save data
       saveData = SaveSystem.instance.LoadGame();

        //check if successful, if not, create a new save data
        if (saveData == null) saveData = new GameData();

        //create the temp list for sorting later
        FillTempList();

        //create a new current player list
        currentPlayers = new List<PlayerData>(0);
        currentPlayers.Add(new PlayerData());
        currentPlayers.Add(new PlayerData());
    }
    private void Update()
    {
       
    }
    public void FillTempList()
    {
        Debug.Log("FILL THE SAVE LIST");
        tempScoreBoard = new List<PlayerData>();
        for (int i = 0; i < 10; i++)
        {
            PlayerData data = new PlayerData();
            data.playerName = saveData.highScorePlayerNames[i];
            data.kills = saveData.highScorePlayerKills[i];
            data.deaths = saveData.highScorePlayerDeaths[i];
            data.wavesSurvived = saveData.highScorePlayerWaves[i];
            tempScoreBoard.Add(data);
        }
        foreach(PlayerData player in currentPlayers)
        {
            Debug.Log("ADD PLAYER SCORES TO LIST");
            tempScoreBoard.Add(player);
        }
    }
    public void FillSaveData()
    {
        tempScoreBoard.Sort(SortPlayerFunc);

        for (int i = 0; i < 10; i++)
        {
            saveData.highScorePlayerNames[i] = tempScoreBoard[i].playerName;
            saveData.highScorePlayerKills[i] = tempScoreBoard[i].kills;
            saveData.highScorePlayerDeaths[i] = tempScoreBoard[i].deaths;
            saveData.highScorePlayerWaves[i] = tempScoreBoard[i].wavesSurvived;
        }
        SaveSystem.instance.SaveGame(saveData);
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
    public void ClearAndSaveData()
    {
        for (int i = 0; i < 10; i++)
        {
            saveData.highScorePlayerNames[i] = "";
            saveData.highScorePlayerKills[i] = 0;
            saveData.highScorePlayerDeaths[i] = 0;
            saveData.highScorePlayerWaves[i] = 0;
        }
        SaveSystem.instance.SaveGame(saveData);
    }
    /* void CheckForEmpties()
     {
         if (highScorePlayerNames.Length == 0)
         {
             highScorePlayerNames = new string[10];
         }
         if (highScorePlayerDeaths.Length == 0)
         {
             highScorePlayerDeaths = new int[10];
         }
         if (highScorePlayerWaves.Length == 0)
         {
             highScorePlayerWaves = new int[10];
         }
         if (highScorePlayerKills.Length == 0)
         {
             highScorePlayerKills = new int[10];
         }

     }*/
}
