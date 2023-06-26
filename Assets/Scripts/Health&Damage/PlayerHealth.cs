using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PlayerHealth : HealthWithUI
{
    public bool isOnline;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public override void Die()
    {
        base.Die();
        anim.SetBool("Dead", true);
        if (isOnline)
        {
            PhotonView view = GetComponent<PhotonView>();
            if (view.IsMine)
            {
                int playerNum = view.OwnerActorNr;
                view.RPC("CallPlayerDeath", RpcTarget.All, playerNum);
            }
        }
        else
        {
            LevelManager.instance.PlayerDeath(GetComponent<PlayerNumber>().playerNumber);
        }
    }
    [PunRPC]
    void CallPlayerDeath(int playerNum)
    {
        OnlineLevelManager.instance.PlayerDeath(playerNum);
    }
}
