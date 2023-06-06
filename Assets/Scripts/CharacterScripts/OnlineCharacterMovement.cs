using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Photon.Pun;

public class OnlineCharacterMovement : MonoBehaviour
{
    [Header("Movement Values")]
    [SerializeField] float movementSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float gravityForce;
    [SerializeField] float jumpForce;

    //Components
    CharacterController cc;
    Animator anim;
    PhotonView view;

    Vector3 movementDirection;
    Camera cam;

    public Transform target;

    //Gravity and jump
    Vector3 playerVelocity;
    public bool groundedPlayer;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();

        cam = Camera.main;
        view = GetComponent<PhotonView>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!view.IsMine) return;

        groundedPlayer = cc.isGrounded;
        if(groundedPlayer && playerVelocity.y < 0)
        {
            if (anim.GetBool("Jump")) anim.SetBool("Jump", false);
            playerVelocity.y = 0;
        }

        float h = (Convert.ToInt64(Input.GetKey(KeyCode.A))*-1) + Convert.ToInt64(Input.GetKey(KeyCode.D));
        float v = (Convert.ToInt64(Input.GetKey(KeyCode.S)) * -1) + Convert.ToInt64(Input.GetKey(KeyCode.W));

        //determine camera direction on a flat plain
        Vector3 camh = cam.transform.right;
        Vector3 camv = Vector3.Cross(camh, Vector3.up);

        if(h != 0 || v != 0)
        {
            movementDirection = camh * h + camv * v;
            movementDirection.Normalize();
            cc.Move(movementDirection * movementSpeed * Time.deltaTime);

            anim.SetBool("HasInput", true);
        }
        else
        {
            anim.SetBool("HasInput", false);
        }
        

        Quaternion desiredDirection = Quaternion.LookRotation(movementDirection);
        anim.transform.rotation = Quaternion.Lerp(anim.transform.rotation, desiredDirection, rotationSpeed);

        Vector3 animationVector = anim.transform.InverseTransformDirection(cc.velocity);

        anim.SetFloat("HorizontalSpeed", animationVector.x);
        anim.SetFloat("VerticalSpeed", animationVector.z);

        ProcessGravity();
       
    }
    public void ProcessGravity()
    {
        if(Input.GetKeyDown(KeyCode.Space) && groundedPlayer)
        {
            anim.SetBool("Jump", true);
            playerVelocity.y += Mathf.Sqrt(jumpForce * -3.0f * gravityForce);
        }

        playerVelocity.y += gravityForce * Time.deltaTime;
        cc.Move(playerVelocity * Time.deltaTime);
    }
}
