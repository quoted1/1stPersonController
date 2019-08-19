using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Public Variables

    #endregion

    #region Private Variables
    private Rigidbody RB;
    private float moveSpeed;

    [SerializeField]
    private float crouchSpeed = 0.25f;
    [SerializeField]
    private float walkSpeed = 0.5f;
    [SerializeField]
    private float sprintSpeed = 1.5f;

    private bool isGrounded;
    private float moveX;
    private float moveY;

    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private GameObject floorCollider;

    [SerializeField]
    private GameObject directionalSphere;

    private KeyCode keyPressed;
    #endregion

    private void Start()
    {
        RB = this.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        InputManager();
    }

    private void InputManager()
    {
        bool walking;
        //Directional movement
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");
        if (isGrounded == true)
        {
            directionalSphere.transform.localPosition = new Vector3(moveX, 0, moveY);
        }
        this.transform.position = Vector3.MoveTowards(this.transform.position, directionalSphere.transform.position, moveSpeed * 0.1f/*XYDamping*/);

        //if you can figure out how to do this better, please do, i dont think a switch function would work
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = sprintSpeed;
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            moveSpeed = crouchSpeed;
        }
        else
        {
            moveSpeed = walkSpeed;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded == true)
        {
            RB.AddForce(Vector3.up*jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        isGrounded = true;
    }
}
