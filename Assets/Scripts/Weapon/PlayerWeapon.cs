using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
            EquipWeapon(0);
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
                EquipWeapon(i);
                break;
            }
        }

        if (weapons[currentWeaponIndex] is SprayGun)
        {
            if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / weapons[currentWeaponIndex].weaponInfo.fireRate;
                weapons[currentWeaponIndex].Shoot();
            }
        }
  
    }


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

        if (PV.IsMine)
        {
            Hashtable hash = new Hashtable();
            hash.Add("weaponIndex", currentWeaponIndex);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if(!PV.IsMine && targetPlayer == PV.Owner)
        {
            EquipWeapon((int)changedProps["weaponIndex"]);
        }
    }

}
