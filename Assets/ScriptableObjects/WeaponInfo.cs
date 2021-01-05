using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FPS/New Gun")]
public class WeaponInfo : ScriptableObject
{
    public string weaponName;
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 10f;
    public int maxAmmo;
    public int currentAmmo;
    public float reloadTime;
}
