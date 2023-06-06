using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class OnlinePlayerName : MonoBehaviour
{
    PhotonView view;
    TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        text = GetComponentInChildren<TMP_Text>();

        if (view.IsMine)
        {
            StartCoroutine(TriggerNameUpdate(PhotonNetwork.LocalPlayer.NickName));
        }
    }

    IEnumerator TriggerNameUpdate(string name)
    {
        yield return new WaitForSeconds(1);
        view.RPC("UpdateName", RpcTarget.All, name);
        yield return null;

    }


    [PunRPC]
    public void UpdateName(string name)
    {
        text.text = name;
    }
    
}
