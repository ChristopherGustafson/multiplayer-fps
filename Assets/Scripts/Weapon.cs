using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public WeaponInfo weaponInfo;
    public GameObject weaponGameObject;

    public abstract void Use();
}
