using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hazard : MonoBehaviour
{
    public float damageAmount;
    public List<string> tags;

    public UnityEvent OnHit;

    void OnTriggerEnter(Collider collision)
    {
        if (!tags.Contains(collision.tag)) return;

        Health health = collision.GetComponent<Health>();
        if(health != null)
        {
            health.TakeDamage(damageAmount);
        }
        OnHit.Invoke();
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (!tags.Contains(collision.gameObject.tag)) return;

        Health health = collision.gameObject.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(damageAmount);
        }
        OnHit.Invoke();
    }
}
