using UnityEngine;
using Photon.Pun;

public class SingleShotGun : Weapon
{
    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
   
    public override void Use()
    {
        muzzleFlash.Play();

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, weaponInfo.range))
        {
            hit.collider.gameObject.GetComponent<Damageable>()?.TakeDamage(weaponInfo.damage);
        }
    }
}
