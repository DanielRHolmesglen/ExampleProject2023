using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// This script manages Game State in level, playing appropriate waves, and managing players and scores.
/// </summary>
public class LevelManager : MonoBehaviour
{
    #region singleton
    [HideInInspector] public static LevelManager instance;
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

    [Header("Level Settings")]
    public WaveManager[] waves;
    public int currentWave = 0;

    public Timer timer;

    public float timeBetweenWaves;

    public enum GameState {Prepping, InWave, Paused, Won, Lost}
    public GameState currentState;

    [Header("Events")]
    public UnityEvent OnWaveEnded;
    public UnityEvent OnLost;
    public UnityEvent OnWon;
    public UnityEvent OnPrepEnd;

    [Header("Attached Components")]
    public InLevelUIManager UIManager;
    // Start is called before the first frame update
    void Start()
    {
        timer.StartTimer(5f);
        currentState = GameState.Prepping;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentState == GameState.Prepping)
        {
            UIManager.UpdateUI();
        }
    }
    public void BeginWave()
    {
        currentState = GameState.InWave;
        waves[currentWave].isActive = true;
        UIManager.UpdateUI();
    }
    public void EndWave()
    {
        currentWave++;
        if(currentWave < waves.Length)
        {
            StartPrep();
        }
        else
        {
            currentState = GameState.Won;
        }
    }
    public void StartPrep()
    {
        currentState = GameState.Prepping;
        timer.StartTimer(timeBetweenWaves);
    }
    public void UpdateScore(int playerNum)
    {
        players[playerNum - 1].kills++;
        UIManager.UpdateUI();
    }
}
