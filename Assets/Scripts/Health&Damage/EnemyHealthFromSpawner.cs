using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthFromSpawner : HealthWithUI
{
    public WaveManager manager;

    public override void Die()
    {
        manager.KillEnemy(gameObject);
        base.Die();
        Destroy(gameObject, 0.5f);
    }
}
