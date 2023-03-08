using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    public bool isDead;

    public UnityEvent OnDamaged;
    public UnityEvent OnDeath;

    public virtual void TakeDamage(float damageAmount)
    {
        Debug.Log("OUCH");
        OnDamaged.Invoke();

        currentHealth -= damageAmount;

        if(currentHealth <= 0)
        {
            Die();
        }
    }
    public virtual void Die()
    {
        isDead = true;
        OnDeath.Invoke();
    }
}
