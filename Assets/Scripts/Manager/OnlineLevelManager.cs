using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
/// <summary>
/// Manages Game State in a level, plays the appropriate waves, and manages player scoring and respawning
/// Treat this script like the information hub for your level.
/// </summary>
public class OnlineLevelManager : MonoBehaviourPunCallbacks, IOnEventCallback, IInRoomCallbacks
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

    #region variables
    [Header("Players")]
    public GameObject[] players;
    public GameObject[] playerPrefabs;
    public Transform[] playerSpawns;

    //list of waves
    [Header("Level Settings")]
    public WaveManager[] waves;
    public int currentWave = 0;

    public OnlineTimer timer;

    public float timeBetweenWaves;

    public enum GameStates { Prepping, InWave, Paused, Won, Lost}
    public GameStates currentState;

    [Header("Attached Components and Scripts")]
    public InLevelUIManager UIManager;
    PhotonView view;

    //Event data

    private const byte CHANGE_STATE = 2;
    private const byte UPDATE_SCORE = 3;
    #endregion

    #region game set up
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
        Invoke("CallAddPlayerToList", 1);

        //assign camera in scene
       // player.GetComponentInChildren<Camera>().tag = "MainCamera";
    }
    void CallAddPlayerToList()
    {
        view.RPC("AddPlayerToList", RpcTarget.All);
    }

    [PunRPC]
    void AddPlayerToList() //find all the instances of players and add them to the array
    {
        GameObject[] playersFound = GameObject.FindGameObjectsWithTag("Player");//find the objects
        foreach(GameObject player in playersFound)//itterate through each object
        {
            int playerNum = player.GetComponent<PhotonView>().OwnerActorNr - 1; //get the objects player number
            players[playerNum] = player;//assign the object to the correct slot
            GameManager.instance.currentPlayers[playerNum].playerName = PhotonNetwork.PlayerList[playerNum].NickName;
        }

    }
    #endregion

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
    #region ManageGameStates
    public void StartPrep()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        currentState = GameStates.Prepping;

        object[] data = new object[] { GameStates.Prepping };
        PhotonNetwork.RaiseEvent(CHANGE_STATE, data, RaiseEventOptions.Default, SendOptions.SendUnreliable);

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
        if (!PhotonNetwork.IsMasterClient) return;

        currentState = GameStates.InWave;

        object[] data = new object[] { GameStates.InWave };
        PhotonNetwork.RaiseEvent(CHANGE_STATE, data, RaiseEventOptions.Default, SendOptions.SendUnreliable);
        waves[currentWave].isActive = true;
        UIManager.UpdateUI();
    }

    //track waves completed and run victory
    public void EndWave()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        currentWave++;
        if (currentWave < waves.Length)
        {  
            StartPrep();
        }
        else
        {
            currentState = GameStates.Won;

            object[] data = new object[] { GameStates.Won };
            PhotonNetwork.RaiseEvent(CHANGE_STATE, data, RaiseEventOptions.Default, SendOptions.SendUnreliable);

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
        if (!PhotonNetwork.IsMasterClient) return;
        //update player score
        int deaths = GameManager.instance.currentPlayers[playerNumber - 1].deaths++;

        //get a gameobject reference to the player
        GameObject currentPlayer = players[playerNumber -1];

        //convert score change and player number into data for event
        PlayerData();

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

            object[] data = new object[] { GameStates.Lost };
            PhotonNetwork.RaiseEvent(CHANGE_STATE, data, RaiseEventOptions.Default, SendOptions.SendUnreliable);

            UIManager.EndGameUI();
            Invoke("SaveResultsAndLoadScene", 5);
        }


    }

    void SpawnPlayerOnWaveEnd(GameObject player)
    {
        if (!PhotonNetwork.IsMasterClient) return;
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
        if (!PhotonNetwork.IsMasterClient) return;
        GameManager.instance.currentPlayers[playerNumber].kills++;

        PlayerData();

        UIManager.UpdateUI();

        Debug.Log(GameManager.instance.currentPlayers[playerNumber].kills);
    }
    public void PlayerData()
    {
        //make a reference to each player data
        PlayerData p1 = GameManager.instance.currentPlayers[0];
        PlayerData p2 = GameManager.instance.currentPlayers[1];

        //convert each score into object form for raise events
        object[] playerData = new object[]
        {
            p1.kills,
            p1.deaths,
            p1.wavesSurvived,
            p2.kills,
            p2.deaths,
            p2.wavesSurvived
        };
        PhotonNetwork.RaiseEvent(UPDATE_SCORE, playerData, RaiseEventOptions.Default, SendOptions.SendReliable);
    }
    #endregion

    #region Pun Event Methods
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }
    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    public void OnEvent(EventData photonEvent)
    {
        if(photonEvent.Code == CHANGE_STATE)
        {
            object[] data = (object[])photonEvent.CustomData;
            currentState = (GameStates)data[0];
            UIManager.UpdateUI();
        }
        if (photonEvent.Code == UPDATE_SCORE)
        {
            object[] data = (object[])photonEvent.CustomData;
            PlayerData p1 = GameManager.instance.currentPlayers[0];
            p1.kills = (int)data[0];
            p1.deaths = (int)data[1];
            p1.wavesSurvived = (int)data[2];
            PlayerData p2 = GameManager.instance.currentPlayers[1];
            p2.kills = (int)data[3];
            p2.deaths = (int)data[4];
            p2.wavesSurvived = (int)data[5];
            UIManager.UpdateUI();
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //remove the player from the list of players, freeing up its actorNumber
        PhotonNetwork.CurrentRoom.Players.Remove(otherPlayer.ActorNumber);
        //print a message
        //exit the game to restart
        
    }
    #endregion
}
