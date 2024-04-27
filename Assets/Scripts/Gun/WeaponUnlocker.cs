using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUnlocker : MonoBehaviour
{
    // Reference to the WeaponSwitcher script on the player object
    public WeaponSwitcher weaponSwitcher;

    private void Start()
    {
        weaponSwitcher = GameObject.Find("GoodPlayer").GetComponentInChildren<WeaponSwitcher>();
    }

    // Handle collider trigger events to unlock weapons
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to a gun
        GameObject collidedObject = other.gameObject;
        if (weaponSwitcher.guns.Contains(collidedObject))
        {
            // Find the index of the collided gun
            int index = weaponSwitcher.guns.IndexOf(collidedObject);

            // Unlock the gun
            weaponSwitcher.UnlockWeapon(index);
        }
    }
}
