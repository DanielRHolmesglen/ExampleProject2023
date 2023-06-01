using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Pun;
/// <summary>
/// Manages Game State in a level, plays the appropriate waves, and manages player scoring and respawning
/// Treat this script like the information hub for your level.
/// </summary>
public class OnlineLevelManager : MonoBehaviour
{
    #region Singleton
    public static OnlineLevelManager instance;

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
    public GameObject[] players;
    public GameObject[] playerPrefabs;
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
    PhotonView view;



    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        timer.StartTimer(5f);
        currentState = GameStates.Prepping;

        Invoke("SpanwPlayerAtStart", 1);
    }
    void SpanwPlayerAtStart()
    {
        //SpawnPlayer
        int a = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        GameObject player = PhotonNetwork.Instantiate(playerPrefabs[a].name, playerSpawns[a].position, Quaternion.identity).gameObject;
        if (player == null) Debug.Log("No player reference");
        view.RPC("AddPlayerToList", RpcTarget.All, a, player);

        //assign camera in scene
        player.GetComponentInChildren<Camera>().tag = "MainCamera";
    }

    [PunRPC]
    void AddPlayerToList(int num, GameObject player)
    {
        players[num] = player;
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

        foreach(GameObject player in players)
        {
            if (player.GetComponent<Health>().isDead)
            {
                SpawnPlayerOnWaveEnd(player);
            }
            else
            {
                int playerNum = player.GetComponent<PlayerNumber>().playerNumber -1;
                GameManager.instance.currentPlayers[playerNum].wavesSurvived++;
            }
        }
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
        currentWave++;
        if (currentWave < waves.Length)
        {  
            StartPrep();
        }
        else
        {
            currentState = GameStates.Won;
            foreach (GameObject player in players)
            {
                if (!player.GetComponent<Health>().isDead)
                {
                    int playerNum = player.GetComponent<PlayerNumber>().playerNumber - 1;
                    GameManager.instance.currentPlayers[playerNum].wavesSurvived++;
                }
            }
            UIManager.EndGameUI();
            Invoke("SaveResultsAndLoadScene", 5);
        }
    }
    //process score and update the other scripts
    public void PlayerDeath(int playerNumber)
    {
        //update player score
        GameManager.instance.currentPlayers[playerNumber - 1].deaths++;

        //get a gameobject reference to the player
        GameObject currentPlayer = players[playerNumber -1];

        //deactivate components.
        currentPlayer.GetComponent<CharacterController>().enabled = false;
        currentPlayer.GetComponent<Collider>().enabled = false;
        currentPlayer.GetComponent<Health>().enabled = false;
        currentPlayer.GetComponent<CharacterMovement>().enabled = false;
        currentPlayer.GetComponent<PlayerAttacks>().meleeCollider.SetActive(false);
        currentPlayer.GetComponent<PlayerAttacks>().enabled = false;

        //Check to see if all the players are dead. If so, end the game
        bool anyAlive = false;
        foreach(GameObject player in players)
        {
            if(player.GetComponent<PlayerHealth>().isDead == false)
            {
                anyAlive = true;
            }
        }
        if(anyAlive == false)
        {
            Debug.Log("NO PLAYERS ALIVE");
            currentState = GameStates.Lost;
            UIManager.EndGameUI();
            Invoke("SaveResultsAndLoadScene", 5);
        }


    }

    void SpawnPlayerOnWaveEnd(GameObject player)
    {
        //deactivate components.
        player.GetComponent<CharacterController>().enabled = true;
        player.GetComponent<Collider>().enabled = true;

        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        playerHealth.enabled = true;
        playerHealth.currentHealth = playerHealth.maxHealth * 0.75f;
        playerHealth.UpdateUI();



        player.GetComponent<CharacterMovement>().enabled = true;
        player.GetComponent<PlayerAttacks>().enabled = true;
        player.GetComponentInChildren<Animator>().SetBool("Dead", false);
    }
    void SaveResultsAndLoadScene()
    {
        Debug.Log("LOADING THE END OF THE GAME");
        GameManager.instance.FillTempList();
        GameManager.instance.FillSaveData();
        SceneManager.LoadScene("Results");
    }

    public void IncreaseScore(int playerNumber)
    {
        GameManager.instance.currentPlayers[playerNumber].kills++;

        UIManager.UpdateUI();

        Debug.Log(GameManager.instance.currentPlayers[playerNumber].kills);
    }
}
