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

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        view = GetComponent<PhotonView>();
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
        GameObject currentProj = PhotonNetwork.Instantiate(projectile.name, launchPoint.position, Quaternion.LookRotation(transform.forward));

        currentProj.GetComponent<Rigidbody>().AddForce((anim.transform.forward * lobSpeed) + (Vector3.up * lobLift), ForceMode.Impulse);
        currentProj.GetComponent<PlayerAttackCollision>().playerNumber = PhotonNetwork.LocalPlayer.ActorNumber;

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
