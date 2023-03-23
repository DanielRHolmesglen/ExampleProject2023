using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : HealthWithUI
{
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public override void Die()
    {
        base.Die();
        anim.SetBool("Dead", true);
        LevelManager.instance.PlayerDeath(GetComponent<PlayerNumber>().playerNumber);
    }
}
