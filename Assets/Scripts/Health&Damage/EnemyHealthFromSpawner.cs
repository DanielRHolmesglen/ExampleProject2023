using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemyHealthFromSpawner : HealthWithUI
{
    public bool isOnline;
    public WaveManager manager;
    public int lastDamagedBy;

    public override void Die()
    {
        
        if (isOnline)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    manager.KillEnemy(gameObject);
                }
                OnlineLevelManager.instance.IncreaseScore(lastDamagedBy - 1);
            }
            GetComponent<PhotonView>().RPC("DestroyEnemy", RpcTarget.All); 
        }
        else
        {
            manager.KillEnemy(gameObject);
            LevelManager.instance.IncreaseScore(lastDamagedBy - 1);
            Destroy(gameObject, 0.5f);
        }
        base.Die();
        
    }
    [PunRPC]
    public void DestroyEnemy()
    {
        Destroy(gameObject, 0.5f);
    }
}
