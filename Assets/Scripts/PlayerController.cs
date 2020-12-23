using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float sprintSpeedModifier;
    public float jumpForce;
    public Camera playerCam;
    public LayerMask groundMask;
    public Transform groundCheck;

    private float BaseFOV;
    public float sprintFOVModifier = 1.25f;

    public float fallMultiplier = 3.5f;
    public float lowJumpMultiplier = 3f;
    private bool isJumping;
    private bool isGrounded;

    private Rigidbody rig;
    PhotonView PV;

    void Awake()
    {
        rig = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();

        BaseFOV = playerCam.fieldOfView;
    }

    private void Start()
    {
        if (!PV.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
        }
    }


    void Update()
    {
        if (!PV.IsMine)
            return;
        isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, 0.4f, groundMask);
        bool jump = Input.GetKeyDown(KeyCode.Space);
        isJumping = jump && isGrounded;
        if (isJumping)
        {
            rig.AddForce(Vector3.up * jumpForce);
        }
    }

    void FixedUpdate()
    {
        if (!PV.IsMine)
            return;
        // Fetch input
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        bool sprint = Input.GetKey(KeyCode.LeftShift);
        bool isSprinting = sprint && z > 0;

        if (rig.velocity.y < 0)
        {
            rig.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if(rig.velocity.y > 0)
        {
            rig.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        // Moving
        Vector3 move = new Vector3(x, 0, z);
        move.Normalize();

        float adjustedSpeed = speed;
        if (isSprinting)
        {
            adjustedSpeed *= sprintSpeedModifier;
            playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, BaseFOV * sprintFOVModifier, Time.deltaTime*8f);        }
        else
        {
            playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, BaseFOV, Time.deltaTime * 8f);
        }

        Vector3 targetVelocity = transform.TransformDirection(move) * adjustedSpeed * Time.deltaTime;
        targetVelocity.y = rig.velocity.y;
        rig.velocity = targetVelocity;
    }

}
