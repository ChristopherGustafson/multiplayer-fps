using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController2 : MonoBehaviour, Damageable
{
    // Assignables
    private Rigidbody rig;
    PhotonView PV;
    public Camera playerCam;
    public LayerMask groundMask;
    public Transform groundCheck;

    PlayerManager playerManager;
    [SerializeField] PlayerUI playerUI;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] AudioSource playerAudio;
    [SerializeField] AudioClip takeDamage;
    // Looking
    [SerializeField] float mouseSensitivity;
    [SerializeField] GameObject cameraHolder;
    private float verticalLookRotation;
    private static bool cursorLocked = true;

    // Walking
    public float speed;

    // Sprinting
    public float sprintSpeedModifier;
    private float BaseFOV;
    public float sprintFOVModifier = 1.25f;

    // Jumping
    public float jumpForce;
    public float fallMultiplier = 3.5f;
    public float lowJumpMultiplier = 3f;

    // Input
    private float x, z;
    private bool jump, sprint;

    // States 
    private bool isJumping;
    private bool isGrounded;
    private bool isSprinting;

    // Player Health
    private const float maxHealth = 100;
    private float health = maxHealth;

    void Awake()
    {
        rig = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();

        BaseFOV = playerCam.fieldOfView;

        playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start()
    {
        // Remove camera and rigidbody if not ours
        if (!PV.IsMine) { 
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rig);
        }
        // If our view, ignore ourselves when using raycasts (shooting)
        else
        {
            gameObject.layer = 2;
            playerUI.SetUIHealth(100f);
        }

    }

    void Update()
    {
        if (!PV.IsMine)
            return;


        FetchInput();
        CheckStates();
        Look();
        ToggleCursor();
        Jump();

        if(transform.position.y < -20f)
        {
            playerManager.Die();
        }
    }

    void FixedUpdate()
    {
        if (!PV.IsMine)
            return;

        Movement();
    }

    private void FetchInput()
    {
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");
        sprint = Input.GetKey(KeyCode.LeftShift);
        jump = Input.GetKeyDown(KeyCode.Space);
    }

    private void CheckStates()
    {
        isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, 0.4f, groundMask);
        isJumping = jump && isGrounded;
        isSprinting = (sprint && z > 0 && isGrounded) || (isSprinting && !isGrounded);
    }

    private void Look()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);
        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }

    void ToggleCursor()
    {
        if (cursorLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                cursorLocked = false;
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                cursorLocked = true;
            }
        }
    }

    private void Movement()
    {
        // Smoother jumping/Falling
        if (rig.velocity.y < 0)
        {
            rig.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rig.velocity.y > 0)
        {
            rig.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        // Moving
        Vector3 move = new Vector3(x, 0, z);
        move.Normalize();

        // Adjust speed to if sprinting or not
        float adjustedSpeed = speed;
        if (isSprinting)
        {
            adjustedSpeed *= sprintSpeedModifier;
            playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, BaseFOV * sprintFOVModifier, Time.deltaTime * 8f);
        }
        else
        {
            playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, BaseFOV, Time.deltaTime * 8f);
        }

        Vector3 targetVelocity = transform.TransformDirection(move) * adjustedSpeed * Time.deltaTime;
        targetVelocity.y = rig.velocity.y;
        rig.velocity = targetVelocity;
    }

    private void Jump()
    {
        // Jumping
        if (isJumping)
        {
            rig.AddForce(Vector3.up * jumpForce);
        }
    }

    public bool TakeDamage(float damage, Vector3 from)
    {
        PV.RPC("RPC_TakeDamage", RpcTarget.All, damage, from);
        return false;
    }

    [PunRPC]
    void RPC_TakeDamage(float damage, Vector3 from)
    {
        if (!PV.IsMine)
            return;

        health -= damage;

        playerUI.SetUIHealth(health);
        playerUI.SetDamageIndication(from);
        /*
        playerAudio.clip = takeDamage;
        playerAudio.Play();
        */
       
        if(health <= 0)
        {
            playerManager.Die();
        }
    }
}
