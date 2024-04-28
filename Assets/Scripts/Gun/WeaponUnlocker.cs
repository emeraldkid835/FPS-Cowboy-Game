using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUnlocker : MonoBehaviour
{
    // Reference to the WeaponSwitcher script on the player object
    public WeaponSwitcher weaponSwitcher;
    [SerializeField] int weaponIndex; // int of 1 = shotgun, int of 2 = fireball shooter

    private void Start()
    {
        weaponSwitcher = GameObject.Find("GoodPlayer").GetComponentInChildren<WeaponSwitcher>();
    }

    // Handle collider trigger events to unlock weapons
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("player in trigger");
        // Check if the collider belongs to a gun
        GameObject collidedObject = other.gameObject;
        if (collidedObject != null)
        {
            Debug.Log("Gun is found");
            // Find the index of the collided gun
            int index = weaponIndex;

            // Unlock the gun
            weaponSwitcher.UnlockWeapon(index);
            weaponSwitcher.SwitchGun(index);
        }

        Destroy(this.gameObject);
    }
}
