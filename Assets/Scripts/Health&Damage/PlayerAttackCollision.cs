using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAttackCollision : MonoBehaviour
{
    public int playerNumber;//assigned by a player attack when it is called

    public bool autoDestroy; //if this object is a projectile, it should destroy itself;

    public float timeTillDestruction;//how long before the object is destroyed, if it should be destroyed

    public float damageAmount;//how much you will hurt the enemy

    public List<string> tags;

    public UnityEvent OnHit;
    // Start is called before the first frame update
    void Start()
    {
        if (autoDestroy)
        {
            Destroy(gameObject, timeTillDestruction);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            EnemyHealthFromSpawner healthScript = collision.gameObject.GetComponent<EnemyHealthFromSpawner>();
            healthScript.lastDamagedBy = playerNumber;
            healthScript.TakeDamage(damageAmount);
        }
        if(tags.Contains(collision.gameObject.tag) && autoDestroy)
        {
            Destroy(gameObject);
        }
        OnHit.Invoke();
    }

}
