using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public WeaponInfo weaponInfo;
    public GameObject weaponGameObject;
    public AudioClip weaponSound;
    public abstract void Shoot(PhotonView PV);
    public abstract bool canShoot();
    public abstract IEnumerator Reload();
}
