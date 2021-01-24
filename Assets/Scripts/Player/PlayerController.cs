using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviour, Damageable
{
    // Assignables
    PhotonView PV;

    PlayerManager playerManager;
    [SerializeField] PlayerUI playerUI;
    [SerializeField] GameObject playerBody;
    [SerializeField] PlayerMovement playerMovement;
    
    private static bool cursorLocked = true;
    
   
    // Player Health
    private const float maxHealth = 100;
    private float health = maxHealth;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
        playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start()
    {
        // Remove camera and rigidbody if not ours
        if (!PV.IsMine) { 
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(GetComponentInChildren<Rigidbody>());
            Destroy(GetComponentInChildren<Canvas>().gameObject);
        }
        // If our view, ignore ourselves when using raycasts (shooting)
        else
        {
            playerBody.layer = 2;
            playerUI.SetUIHealth(100f);
        }

    }

    void Update()
    {
        if (!PV.IsMine)
            return;

        playerMovement.GetInput();
        playerMovement.Look();

        if(playerBody.transform.position.y < -20f)
        {
            playerManager.Die();
        }
    }

    void FixedUpdate()
    {
        if (!PV.IsMine)
            return;

        playerMovement.Movement();
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

    private bool kill = false;
    public bool TakeDamage(float damage, Vector3 from)
    {
        if (!kill)
        {
            kill = (health - damage) <= 0;
            PV.RPC("RPC_TakeDamage", RpcTarget.All, damage, from);
            StartCoroutine(KillCooldown());
            return kill;
        }
        else
        {
            PV.RPC("RPC_TakeDamage", RpcTarget.All, damage, from);
            return false;
        }
    }

    IEnumerator KillCooldown()
    {
        yield return new WaitForSeconds(1);
        kill = false;
    }

    [PunRPC]
    void RPC_TakeDamage(float damage, Vector3 from)
    {

        health -= damage;
        if (!PV.IsMine)
        {
            return;
        }


        playerUI.SetUIHealth(health);
        playerUI.SetDamageIndication(from);
        /*
        playerAudio.clip = takeDamage;
        playerAudio.Play();
        */

        if (health <= 0)
        {
            playerManager.Die();
        }
    }
}
