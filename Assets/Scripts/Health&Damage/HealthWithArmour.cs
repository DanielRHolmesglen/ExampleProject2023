using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthWithArmour : HealthWithUI
{
    public float armour;

    // Start is called before the first frame update
    void Start()
    {
        LevelManager.instance.PlayerDeath(7);
    }

    public override void TakeDamage(float damageAmount)
    {
        if (armour > 0)
        {
            armour -= damageAmount;
            damageAmount *= 0.7f;
        }
        base.TakeDamage(damageAmount);
    }
}
