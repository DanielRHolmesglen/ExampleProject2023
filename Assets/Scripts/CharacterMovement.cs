using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Tooltip("Movement Values")]
    [SerializeField] float movementSpeed, rotationSpeed;

    //Components
    CharacterController cc;
    Animator anim;

    Vector3 movementDirection;
    Camera cam;

    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();

        cam = Camera.main;

    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        //determine camera direction on a flat plain
        Vector3 camh = cam.transform.right;
        Vector3 camv = Vector3.Cross(camh, Vector3.up);


        movementDirection = camh * h + camv * v;
        movementDirection.Normalize();

        cc.Move(movementDirection * movementSpeed * Time.deltaTime);
    }
}
