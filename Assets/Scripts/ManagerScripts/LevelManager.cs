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
    [HideInInspector] public LevelManager instance;
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
    public GameObject player1;
    public GameObject player2;
    public Transform player1SpawnPosition;
    public Transform player2SpawnPosition;

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

    // Start is called before the first frame update
    void Start()
    {
        timer.StartTimer(0.5f);
        currentState = GameState.Prepping;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void BeginWave()
    {
        currentState = GameState.InWave;
        waves[currentWave].isActive = true;
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
}
