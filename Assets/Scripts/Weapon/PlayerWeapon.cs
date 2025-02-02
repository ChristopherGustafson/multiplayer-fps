﻿using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerWeapon : MonoBehaviourPunCallbacks
{
    PhotonView PV;
    
    public Weapon[] weapons;

    public int currentWeaponIndex = -1;
    int previousWeaponIndex = -1;

    [SerializeField] PlayerUI playerUI;

    private float nextTimeToFire = 0f;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (PV.IsMine)
        {

            PV.RPC("EquipWeapon", RpcTarget.All, 0);
            playerUI.SetUIAmmo(weapons[currentWeaponIndex].weaponInfo.currentAmmo);
            
        }
    }

    void Update()
    {
        if (!PV.IsMine)
            return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(weapons[currentWeaponIndex].Reload());
            return;
        }

        for (int i = 0; i < weapons.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                PV.RPC("EquipWeapon", RpcTarget.All, i);
                break;
            }
        }

        if (weapons[currentWeaponIndex] is SprayGun)
        {
            if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / weapons[currentWeaponIndex].weaponInfo.fireRate;
                if(weapons[currentWeaponIndex].canShoot()) PV.RPC("RPC_Shoot", RpcTarget.All);
            }
        }
  
    }

    [PunRPC]
    void RPC_Shoot()
    {
        if (currentWeaponIndex == -1) return;
        weapons[currentWeaponIndex].Shoot(PV);
    }

    [PunRPC]
    void EquipWeapon(int _index)
    {
        if (_index == currentWeaponIndex)
            return;

        currentWeaponIndex = _index;
        weapons[currentWeaponIndex].weaponGameObject.SetActive(true);

        if(previousWeaponIndex != -1)
        {
            weapons[previousWeaponIndex].weaponGameObject.SetActive(false);
        }
        previousWeaponIndex = currentWeaponIndex;
    }
}
