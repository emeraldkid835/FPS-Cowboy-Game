using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUnlocker : MonoBehaviour
{
    // Reference to the WeaponSwitcher script on the player object
    public WeaponSwitcher weaponSwitcher;
    [SerializeField] private AudioSource meSound;
    [SerializeField] int weaponIndex; // int of 1 = shotgun, int of 2 = fireball shooter, 3 = winchester (depends on player prefab)

    private void Start()
    {
        if (weaponSwitcher == null)
        {
            weaponSwitcher = GameObject.Find("GoodPlayer").GetComponentInChildren<WeaponSwitcher>();
        }
        if(PlayerPrefs.GetInt("GunUnlocked_" + weaponIndex) == 1)
        {
            Destroy(this.gameObject);
        }
    }

    // Handle collider trigger events to unlock weapons
    private void OnTriggerEnter(Collider other)
    {
        
            Debug.Log("player in trigger");
            // Check if the collider belongs to a gun. (What?)
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
        if (other.tag == "Player")
        {
            audiomanager.instance.PlaySFX3D(meSound.clip, this.transform.position);

            Destroy(this.gameObject);
        }
    }
    
}
