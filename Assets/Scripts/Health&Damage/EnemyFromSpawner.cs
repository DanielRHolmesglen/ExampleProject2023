using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFromSpawner : HealthWithUI
{
    public WaveManager manager;
    public int lastPlayerToHit;
    public override void Die()
    {
        manager.KillEnemy(gameObject);
        LevelManager.instance.UpdateScore(lastPlayerToHit);
        base.Die();
        Destroy(gameObject, 1);
    }
}
