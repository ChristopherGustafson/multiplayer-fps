using UnityEngine;
using Photon.Pun;
using System.Collections;

public class SingleShotGun : Weapon
{
    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    Animator gunAnimator;

    void Start()
    {
        gunAnimator = GetComponentInChildren<Animator>();     
    }

    public override void Shoot()
    {
        muzzleFlash.Play();
        gunAnimator.SetTrigger("Shoot");

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, weaponInfo.range))
        {

            hit.collider.gameObject.GetComponent<Damageable>()?.TakeDamage(weaponInfo.damage, fpsCam.transform.position);
        }
    }

    public override IEnumerator Reload()
    {
        yield return new WaitForSeconds(0.5f);
        weaponInfo.currentAmmo = weaponInfo.maxAmmo;
    }
}
