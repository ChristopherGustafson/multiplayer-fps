using Photon.Pun;
using System.Collections;
using UnityEngine;

public class SprayGun : Weapon
{
    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    Animator gunAnimator;
    private bool isReloading = false;

    [SerializeField] PlayerUI playerUI;

    [SerializeField] AudioSource gunshotsAudio;
    [SerializeField] GameObject hitmarker;
    [SerializeField] AudioClip hitmarkerSound;

    PhotonView PV;
    PlayerManager playerManager;

    void Start()
    {
        gunAnimator = GetComponentInChildren<Animator>();
    }

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
    }

    void OnEnable()
    {
        if (gunAnimator == null)
            return;
        isReloading = false;
        gunAnimator.SetBool("Reloading", false);
    }

    public override bool canShoot()
    {
        return (!isReloading && weaponInfo.currentAmmo != 0);
    }

    public override void Shoot(PhotonView PV)
    {
       
        muzzleFlash.Play();
        /*
        gunshotsAudio.Stop();
        gunshotsAudio.clip = weaponSound;
        gunshotsAudio.Play();
        */
        if (PV.IsMine) {
            gunAnimator.SetTrigger("Shoot");
            RaycastHit hit;
            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit))
            {
                Damageable objectHit = hit.collider.transform.parent.gameObject.GetComponent<Damageable>();
                if (objectHit != null)
                {
                    if (objectHit.TakeDamage(weaponInfo.damage, fpsCam.transform.position))
                    {
                        Debug.Log("Got Kill");
                        playerManager.Kill();
                    }
                    GameObject hitm = Instantiate(hitmarker);
                    hitm.transform.SetParent(playerUI.transform, false);
                    Destroy(hitm, 1f);
                    
                }

                weaponInfo.currentAmmo--;
                playerUI.SetUIAmmo(weaponInfo.currentAmmo);
            }
        }
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
