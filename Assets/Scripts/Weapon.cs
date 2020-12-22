using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public GameObject[] loadout;
    public Transform weaponParent;

    private GameObject currentWeapon;
    private int currentWeaponIndex = -1;
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) EquipWeapon(0);
    }

    void EquipWeapon (int weaponIndex)
    {
        if(currentWeaponIndex != weaponIndex)
        {
            if (currentWeapon != null) Destroy(currentWeapon);
            GameObject newWeapon = Instantiate(loadout[weaponIndex], weaponParent) as GameObject;
            newWeapon.transform.localPosition = Vector3.zero;
            newWeapon.transform.localEulerAngles = Vector3.zero;
            currentWeapon = newWeapon;
            currentWeaponIndex = weaponIndex;
        }
    }
}
