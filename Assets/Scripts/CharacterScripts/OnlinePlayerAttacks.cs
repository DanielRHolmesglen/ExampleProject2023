using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OnlinePlayerAttacks : MonoBehaviour
{
    Animator anim;

    [Header("Ranged Attack Settings")]
    public GameObject projectile;
    public Transform launchPoint;

    public float lobSpeed, lobLift;

    [Header("Melee Attack Settings")]
    public GameObject meleeCollider;
    public float attackDelay;
    public float attackLifeTime;

    private PhotonView view;
    private OnlinePool pool;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        view = GetComponent<PhotonView>();
        pool = GetComponent<OnlinePool>();
        meleeCollider.GetComponent<PlayerAttackCollision>().playerNumber = PhotonNetwork.LocalPlayer.ActorNumber;
    }

    // Update is called once per frame
    void Update()
    {
        if (!view.IsMine) return;

        if (Input.GetKeyDown(KeyCode.J))
        {
            anim.Play("Shoot");
            Shoot();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            anim.Play("MeleeAttack");
            view.RPC("OnlineMeleeAttack", RpcTarget.All);
            
        }
        if (Input.GetKey(KeyCode.L))
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
        /*GameObject projectile = pool.ReturnObject(); //get an aobject from your pool
        Rigidbody rb = projectile.GetComponent<Rigidbody>(); //get the rigidbody

        //reset the rigidbody
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        projectile.transform.position = launchPoint.position;
        projectile.transform.rotation = Quaternion.LookRotation(transform.forward);

        projectile.SetActive(true);
        rb.AddForce((anim.transform.forward * lobSpeed) + (Vector3.up * lobLift), ForceMode.Impulse);*/

        GameObject currentProj = PhotonNetwork.Instantiate(projectile.name, launchPoint.position, Quaternion.LookRotation(transform.forward));

        currentProj.GetComponent<Rigidbody>().AddForce((anim.transform.forward * lobSpeed) + (Vector3.up * lobLift), ForceMode.Impulse);
        //currentProj.GetComponent<PlayerAttackCollision>().playerNumber = PhotonNetwork.LocalPlayer.ActorNumber;

    }
    [PunRPC]
    void OnlineMeleeAttack()
    {
        Invoke("ActivateMeleeCollider", attackDelay);
    }
    void ActivateMeleeCollider()
    {
        meleeCollider.SetActive(true);
        Invoke("DeactivateMeleeCollider", attackLifeTime);
    }
    void DeactivateMeleeCollider()
    {
        meleeCollider.SetActive(false);
    }
}
