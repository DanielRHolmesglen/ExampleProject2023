using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// This script manages spawning enemies for wave, tracking whether the wave is complete, and communicating with the level manager.
/// </summary>
public class WaveManager : MonoBehaviour
{
    [Header("Enemies")]
    public GameObject[] enemyTypes;
    
    [Header("Wave Settings")]
    public int maxNumberAtOnce;
    public int maxNumberOverLifeTime;

    List<GameObject> enemiesSpawned = new List<GameObject>();
    List<GameObject> currentEnemiesSpawned = new List<GameObject>();
    public bool isActive;
    public bool isComplete;

    public float timeBetweenSpawns;
    float time;

    public Transform[] spawnPoints;

    public UnityEvent EndWave;
    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            time += Time.deltaTime;
            if (time >= timeBetweenSpawns)
            {
                AttemptToSpawn();
                time = 0;
            }
        }
        

    }
    void AttemptToSpawn()
    {
        int enemyNum = Random.Range(0, enemyTypes.Length - 1);
        int spawnPos = Random.Range(0, spawnPoints.Length - 1);

        if(enemiesSpawned.Count < maxNumberOverLifeTime && currentEnemiesSpawned.Count < maxNumberAtOnce)
        {
            GameObject currentEnemy = Instantiate(enemyTypes[enemyNum], spawnPoints[spawnPos].position, Quaternion.identity);
            enemiesSpawned.Add(currentEnemy);
            currentEnemiesSpawned.Add(currentEnemy);
        }
    }

    private void OnDrawGizmos()
    {
        foreach(Transform point in spawnPoints)
        {
            Gizmos.DrawSphere(point.position, 0.2f);
        }
    }
    public void KillEnemy(GameObject enemy)
    {
        currentEnemiesSpawned.Remove(enemy);
        if(currentEnemiesSpawned.Count == 0 && enemiesSpawned.Count >= maxNumberOverLifeTime)
        {
            isActive = false;
            isComplete = true;
            EndWave.Invoke();
        }
    }
}
