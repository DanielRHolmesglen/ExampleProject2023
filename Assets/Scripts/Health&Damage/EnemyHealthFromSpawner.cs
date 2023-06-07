using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthFromSpawner : HealthWithUI
{
    public bool isOnline;
    public WaveManager manager;
    public int lastDamagedBy;
    public override void Die()
    {
        manager.KillEnemy(gameObject);
        if (isOnline)
        {
            OnlineLevelManager.instance.IncreaseScore(lastDamagedBy - 1);
        }
        else
        {
            LevelManager.instance.IncreaseScore(lastDamagedBy - 1);
        }
        base.Die();
        Destroy(gameObject, 0.5f);
    }
}
