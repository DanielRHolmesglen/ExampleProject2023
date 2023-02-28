using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{
    Animator anim;

    public GameObject projectile;
    public Transform launchPoint;

    public float lobSpeed, lobLift;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            anim.Play("Shoot");
            Shoot();
        }
        if (Input.GetKey(KeyCode.R))
        {
            anim.SetBool("Taunt", true);
        }
        else
        {
            anim.SetBool("Taunt", false);
        }
    }
    void Shoot()
    {
        GameObject currentProj = Instantiate(projectile, launchPoint.position, Quaternion.LookRotation(transform.forward));

        currentProj.GetComponent<Rigidbody>().AddForce((anim.transform.forward * lobSpeed) + (Vector3.up * lobLift), ForceMode.Impulse);

    }
}
