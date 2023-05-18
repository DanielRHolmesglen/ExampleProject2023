using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class Launcher : MonoBehaviourPunCallbacks
{
    #region Serializable fields
    #endregion

    #region private fields

    //this is the clients version number. 
    string gameVersion = "1";
    #endregion

    #region Monobehaviour callbacks
    void Awake()
    {
        //CRITICAL
        //make sure everyones scenes are synched when PhotonNetwork.LoadLevel() is called;
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    //on start, try to connect to the master server.
    private void Start()
    {
        Connect();
    }


    #endregion

    #region public methods

    public void Connect()
    {
        if (PhotonNetwork.IsConnected)
        {
            //attempt to join lobby
            PhotonNetwork.JoinLobby();
        }
        else
        {
            //attempt to connect using server settings, then set your game version
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }
    #endregion
    #region Pun Callbacks
    public override void OnConnectedToMaster()
    {
        Debug.Log("Successfully connected to server. Attemping to join a lobby");
        PhotonNetwork.JoinLobby();
    }
    //moniter for disconnecting
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("Failed to connect. OnDisconnected was called with the reason {0}", cause);
        SceneManager.LoadScene(0);
    }
    //load the next scene if we successfully join a lobby.
    public override void OnJoinedLobby()
    {
        PhotonNetwork.LoadLevel("CreateOrJoinRoom");
    }

    #endregion
}
