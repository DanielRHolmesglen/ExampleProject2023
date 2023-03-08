using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Manages Game State in a level, plays the appropriate waves, and manages player scoring and respawning
/// Treat this script like the information hub for your level.
/// </summary>
public class LevelManager : MonoBehaviour
{
    #region Singleton
    public static LevelManager instance;

    void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    #endregion

    
    [Header("Players")]
    public PlayerData[] players;
    public Transform[] playerSpawns;

    //list of waves
    [Header("Level Settings")]
    public WaveManager[] waves;
    public int currentWave = 0;

    public Timer timer;

    public float timeBetweenWaves;

    public enum GameStates { Prepping, InWave, Paused, Won, Lost}
    public GameStates currentState;

    [Header("Attached Components and Scripts")]
    public InLevelUIManager UIManager;



    // Start is called before the first frame update
    void Start()
    {
        timer.StartTimer(5f);
        currentState = GameStates.Prepping;
    }

    // Update is called once per frame
    void Update()
    {
        //UI update
        if(currentState == GameStates.Prepping)
        {
            UIManager.UpdateUI();
        }
    }
    //run a timer between waves for a prep time
    public void StartPrep()
    {
        currentState = GameStates.Prepping;
        timer.StartTimer(timeBetweenWaves);
    }
    //find and run the current wave until all enemies are dead
    public void BeginWave()
    {
        currentState = GameStates.InWave;
        waves[currentWave].isActive = true;
        UIManager.UpdateUI();
    }
    //Track player deaths and run game over

    //track waves completed and run victory
    public void EndWave()
    {
        
        if(currentWave < waves.Length)
        {
            currentWave++;
            StartPrep();
        }
        else
        {
            currentState = GameStates.Won;
            UIManager.EndGameUI();
        }
    }
    //process score and update the other scripts
    public void PlayerDeath(int playerNumber)
    {
        bool anyAlive = false;
        foreach(PlayerData player in players)
        {
            if(player.GetComponent<PlayerHealth>().isDead == false)
            {
                anyAlive = true;
            }
        }
        if(anyAlive == false)
        {
            currentState = GameStates.Lost;
            UIManager.EndGameUI();
        }
    }
}
