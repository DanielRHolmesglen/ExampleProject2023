using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class SimpleEnemyAI : MonoBehaviour
{
    public Transform target;
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        InvokeRepeating("CheckForTarget", 1, 5);
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        agent.SetDestination(target.position);
    }

    void CheckForTarget()
    {
        Debug.Log("Checking for players");

        PlayerData[] players = LevelManager.instance.players;
        string message1 = "";
        foreach(PlayerData player in players)
        {
            message1 += player.name + " ";
        }
        Debug.Log(message1);
        players.OrderBy(point => Vector3.Distance(transform.position, point.gameObject.transform.position)).ToArray();
        string message2 = "";
        foreach (PlayerData player in players)
        {
            message2 += player.name + " ";
        }
        Debug.Log(message2);
        target = players[0].gameObject.transform;
    }
}
