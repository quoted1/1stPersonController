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
    private GameObject directionalSphere;

    private KeyCode keyPressed;

    [SerializeField]
    private GameObject mainCamera;
    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    private RotationAxes axes = RotationAxes.MouseXAndY;
    private float sensitivityX = 15F;
    private float sensitivityY = 15F;
    private float minimumX = -360F;
    private float maximumX = 360F;
    private float minimumY = -60F;
    private float maximumY = 60F;
    private float rotationX = 0F;
    private float rotationY = 0F;
    private Quaternion originalRotation1, originalRotation2;
    
    #endregion

    private void Start()
    {
        RB = this.GetComponent<Rigidbody>();
        if (GetComponent<Rigidbody>())
            RB.freezeRotation = true;
        originalRotation1 = transform.localRotation;
        originalRotation2 = mainCamera.transform.localRotation;
    }

    private void Update()
    {
        rotationX += Input.GetAxis("Mouse X") * sensitivityX;
        rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
        rotationX = Mathf.Clamp(rotationX, minimumX, maximumX);
        rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
        Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
        Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, -Vector3.right);
        transform.localRotation = originalRotation1 * xQuaternion;
        mainCamera.transform.localRotation = originalRotation2 * yQuaternion;

        /* original script, allows you to choose which axis you want to rotate in the inspector
        if (axes == RotationAxes.MouseXAndY)
        {
            // Read the mouse input axis
            rotationX += Input.GetAxis("Mouse X") * sensitivityX;
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationX = Mathf.Clamp(rotationX, minimumX, maximumX);
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
            Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
            Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, -Vector3.right);
            transform.localRotation = originalRotation * xQuaternion * yQuaternion;
        }
        else if (axes == RotationAxes.MouseX)
        {
            rotationX += Input.GetAxis("Mouse X") * sensitivityX;
            rotationX = Mathf.Clamp(rotationX, minimumX, maximumX);
            Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
            transform.localRotation = originalRotation * xQuaternion;
        }
        else
        {
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
            Quaternion yQuaternion = Quaternion.AngleAxis(-rotationY, Vector3.right);
            transform.localRotation = originalRotation * yQuaternion;
        }
        */
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
        CapsuleCollider CCH = GetComponent<CapsuleCollider>();

        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = sprintSpeed;
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            moveSpeed = crouchSpeed;
            CCH.height = Mathf.MoveTowards(CCH.height, 1, 0.15f);
        }
        else
        {
            moveSpeed = walkSpeed;
            CCH.height = Mathf.MoveTowards(CCH.height, 2, 0.15f);
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
