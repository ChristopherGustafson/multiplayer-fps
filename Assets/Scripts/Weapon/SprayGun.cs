using System.Collections;
using UnityEngine;

public class SprayGun : Weapon
{
    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    Animator gunAnimator;
    AudioSource gunSound;
    private bool isReloading = false;


    [SerializeField] PlayerUI playerUI;

    void Start()
    {
        gunAnimator = GetComponentInChildren<Animator>();
        gunSound = GetComponentInChildren<AudioSource>();
    }

    void OnEnable()
    {
        if (gunAnimator == null)
            return;
        isReloading = false;
        gunAnimator.SetBool("Reloading", false);
    }
    
    public override void Shoot()
    {
        if (isReloading)
            return;
        if (weaponInfo.currentAmmo == 0)
            // Play no ammo sound
            return;

        muzzleFlash.Play();
        gunAnimator.SetTrigger("Shoot");
        gunSound.Play();

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit))
        {
            hit.collider.gameObject.GetComponent<Damageable>()?.TakeDamage(weaponInfo.damage, fpsCam.transform.position);
        }

        weaponInfo.currentAmmo--;
        playerUI.SetUIAmmo(weaponInfo.currentAmmo);
    }

    public override IEnumerator Reload()
    {
        isReloading = true;
        gunAnimator.SetBool("Reloading", true);

        yield return new WaitForSeconds(.2f);
        gunAnimator.SetBool("Reloading", false);

        yield return new WaitForSeconds(.3f);
        weaponInfo.currentAmmo = weaponInfo.maxAmmo;
        playerUI.SetUIAmmo(weaponInfo.currentAmmo);
        isReloading = false;
    }

}
