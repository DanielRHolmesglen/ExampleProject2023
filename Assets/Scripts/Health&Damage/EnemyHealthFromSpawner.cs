using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthFromSpawner : HealthWithUI
{
    public WaveManager manager;
    public int lastDamagedBy;
    public override void Die()
    {
        manager.KillEnemy(gameObject);
        LevelManager.instance.IncreaseScore(lastDamagedBy - 1);
        base.Die();
        Destroy(gameObject, 0.5f);
    }
}
