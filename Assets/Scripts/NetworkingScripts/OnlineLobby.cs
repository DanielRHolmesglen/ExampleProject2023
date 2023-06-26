using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class OnlineLobby : MonoBehaviourPunCallbacks
{
    public bool[] playersReady;

    public PhotonView view;

    public TMP_Text roomName;
    public TMP_Text messages;
    public TMP_Text numberOfPlayers;
    public TMP_InputField playerName;

    public string levelName;

    // Start is called before the first frame update
    void Start()
    {
        playersReady = new bool[PhotonNetwork.CurrentRoom.MaxPlayers];
        PhotonNetwork.LocalPlayer.NickName = "Player " + PhotonNetwork.LocalPlayer.ActorNumber;
        roomName.text = "Room Name: " + PhotonNetwork.CurrentRoom.Name;

        numberOfPlayers.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString() + " / " + PhotonNetwork.CurrentRoom.MaxPlayers;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        numberOfPlayers.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString() + " / " + PhotonNetwork.CurrentRoom.MaxPlayers;
        Invoke("UpdateBoolsOnJoin", 1);

    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        numberOfPlayers.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString() + " / " + PhotonNetwork.CurrentRoom.MaxPlayers;
        view.RPC("ReadyPlayer", RpcTarget.All, otherPlayer.ActorNumber, false);
    }

    public void UpdateName()
    {
        PhotonNetwork.LocalPlayer.NickName = playerName.text;
    }

    public void LoadLevel()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            messages.text = "You are not the Host, you can't do that"; 
        }
        else if (PhotonNetwork.CurrentRoom.PlayerCount < PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            messages.text = "Waiting on players. Your lobby is not full";
        }
        else if (AllPlayersReady() == false)
        {
            messages.text = "All players are not ready";
        }
        else
        {
            SceneManager.LoadScene(levelName);
        }
    }

    bool AllPlayersReady()
    {
        foreach(bool item in playersReady)
        {
            if (item == false) return false;
        }
        return true;
    }

    [PunRPC]
    public void ReadyPlayer(int playerNumber, bool isReady)
    {
        playersReady[playerNumber - 1] = isReady;
    }

    public void RunReadyPlayer(bool isReady)
    {
        view.RPC("ReadyPlayer", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber, isReady);
    }

    void UpdateBoolsOnJoin()
    {
        int playerNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        bool isReady = playersReady[ playerNumber - 1];

        view.RPC("ReadyPlayer", RpcTarget.All, playerNumber, isReady);

    }
}
