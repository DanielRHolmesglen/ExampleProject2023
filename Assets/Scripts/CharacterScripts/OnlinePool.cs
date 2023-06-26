using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OnlinePool : MonoBehaviour
{
    //poolSettings
    public GameObject prefabToPool;
    public int amountInPool;
    public List<GameObject> pooledObjects;

    //PhotonSettings
    public PhotonView view;
    public enum PoolerType { item, playerAmmo }
    public PoolerType poolerSetting;

    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        if (view.IsMine)
        {
            PoolObjects();
        }
        
    }

    //poolObjects
    void PoolObjects()
    {
        for (int i = 0; i < amountInPool; i++)
        {
            GameObject item = PhotonNetwork.Instantiate(prefabToPool.name, transform.position, transform.rotation) as GameObject;
            pooledObjects.Add(item);
            if (poolerSetting == PoolerType.playerAmmo)
            {
               
                int playerNum = view.OwnerActorNr -= 1;
                view.RPC("SetOwner", RpcTarget.All, playerNum, i);
            }

            item.SetActive(false);
        }
    }
    [PunRPC]
    public void SetOwner(int playerNum, int item)
    {
        pooledObjects[item].GetComponent<PlayerAttackCollision>().playerNumber = playerNum;
    }

    public GameObject ReturnObject()
    {
        foreach(GameObject item in pooledObjects)
        {
            if (item.activeInHierarchy) return item;
        }

        Debug.LogError("Attempted to get a pooled object but there are none");
        return null;
    }
}
