using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// Managing spawning of enemies for a wave, as well as how long the wave should last.
/// This works alongside the levelmanager
/// </summary>
public class WaveManager : MonoBehaviour
{
    [Header("Enemy info")]
    public GameObject[] enemyTypes; //the prefabs of enemies you will spawn
    public Transform[] spawnPoints; //the positions to create enemies

    [Header("Wave Settings")]
    public int maxNumberAtOnce; //the ammount of enemies to spawn at one interval
    public int maxNumberOverAll; //the overall number of enemies to spawn before the wave is over

    public bool isActive; //The wave will begin spawning once isActive is true
    public bool isComplete;// isComplete is true when all enemies are spawned AND killed.

    public float timeBetweenSpawns;
    float timer;

    List<GameObject> enemiesSpawned = new List<GameObject>(); //all enemies created by this wave manager over its lifeTime.
    List<GameObject> currentEnemiesSpawned = new List<GameObject>(); //all the enemies currently alive that have been spawned by this script.

    [HideInInspector]
    public int killed; //a number of enemies killed from this spawner

    /// <summary>
    /// this event is called when the wave is finished, as isComplete is called.
    /// extend the functionality of this script by subscribing fucntions to this event
    /// </summary>
    public UnityEvent EndWave; 

    // Update is called once per frame
    void Update()
    {
        //attempt to spawn an enemy every interval, provided the spawner is active
        if (isActive)
        {
            timer += Time.deltaTime;
            if(timer > timeBetweenSpawns)
            {
                AttemptToSpawn();
                timer = 0;
            }
        }
    }
    /// <summary>
    /// Called to Spawn an enemy into the scene. If the count of enemies active in the scene excedes maxNumberOverAll or maxNumberAtOnce this will not run.
    /// </summary>
    void AttemptToSpawn()
    {
        //pick enemy type
        int type = Random.Range(0, enemyTypes.Length);
        //pick the position
        int pos = Random.Range(0, spawnPoints.Length);

        //check if you can spawn
        if(enemiesSpawned.Count < maxNumberOverAll && currentEnemiesSpawned.Count < maxNumberAtOnce)
        {
            //spawn the enemy
            GameObject currentEnemy = Instantiate(enemyTypes[type], spawnPoints[pos].position, Quaternion.identity);
            
            //attach this spawner to the EnemyHealth script
            currentEnemy.GetComponent<EnemyHealthFromSpawner>().manager = this;
            //add this enemy to the lists of enemies that this manager holds.
            enemiesSpawned.Add(currentEnemy);
            currentEnemiesSpawned.Add(currentEnemy);
        }
        
    }
    /// <summary>
    /// Called by the enemies health script when the enemies dies.
    /// updats score, removes enemie form the count  and checks if the wave is over
    /// </summary>
    /// <param name="enemy"></param>
    public void KillEnemy(GameObject enemy)
    {
        killed++; //add to score
        currentEnemiesSpawned.Remove(enemy); // remove from list

        //if the maximum number of enemies have been spawned and this was the last one alive, end the wave.
        if (currentEnemiesSpawned.Count == 0 && enemiesSpawned.Count >= maxNumberOverAll)
        {
            isActive = false;
            isComplete = true;
            EndWave.Invoke();
        }
    }
}
