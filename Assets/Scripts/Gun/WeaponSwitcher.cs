using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    public bool isReloading;
    public List<GameObject> guns = new List<GameObject>();
    private int currentGunIndex = 0;
    public List<bool> gunBools;

    

    void Start()
    {
        gunBools = new List<bool>(guns.Count);
        isReloading = false;
        // Set up initial weapon state (locked/unlocked)
        for (int i = 0; i < guns.Count; i++)
        {
            // Change this condition according to your unlocking logic
            if (i == 0 || IsWeaponUnlocked(i))
            {
                gunBools.Add(true);
                if (i != 0)
                {
                    guns[i].SetActive(false);
                }
               
            }
            else
            {
                gunBools.Add(false);
                guns[i].SetActive(false); // Disable locked weapons
            }
        }
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f)
        {
            SwitchGun(1);
        }
        else if (scroll < 0f)
        {
            SwitchGun(-1);
        }
    }
    public void SwitchGun(int offset) //we need to rethink this.
    {
        bool relog = true;
        int ogoffset = offset;
        int nextGunIndex = (currentGunIndex + offset + guns.Count) % guns.Count;
        while (relog == true)
        {
            // Check if the next gun is unlocked
            if (!isReloading)
            {
                if (!gunBools[nextGunIndex])
                {
                    offset += ogoffset;
                    nextGunIndex = (currentGunIndex + offset + guns.Count) % guns.Count;
                }
                else
                {
                    // Disable current gun
                    guns[currentGunIndex].SetActive(false);

                    // Enable the next gun
                    guns[nextGunIndex].SetActive(true);

                    // Update current gun index
                    currentGunIndex = nextGunIndex;

                    // Notify the InputManager about the weapon switch
                    InputManager.instance.UpdateEquippedGun(guns[currentGunIndex].GetComponent<GunClass>());
                    relog = false;
                }

            }
            else
            {
                relog = false;
            }
        }
    }

    public void MaxAllAmmo() //hopefully max every and all ammo
    {
        foreach(GameObject gun in guns)
        {
            MonoBehaviour[] monos = gun.GetComponents<MonoBehaviour>();
            for (int i = 0; i < monos.Length; i++)
            {
                if(monos[i] is GunClass)
                {
                    GunClass temp = monos[i] as GunClass;
                    temp.AddAmmo(9999);
                }
            }
        }
    }

    // Method to check if a weapon is unlocked
    bool IsWeaponUnlocked(int index)
    {
        // Retrieve the unlocking status from PlayerPrefs or another storage mechanism
        return PlayerPrefs.GetInt("GunUnlocked_" + index, 0) == 1;
    }

    // Method to unlock a weapon
    public void UnlockWeapon(int index)
    {
        // Update the unlocking status in PlayerPrefs or another storage mechanism
        PlayerPrefs.SetInt("GunUnlocked_" + index, 1);
        PlayerPrefs.Save();

        // Update the gunBools list
        gunBools[index] = true;

        //SwitchGun(index);
        // Disable current gun
        //guns[currentGunIndex].SetActive(false);

        // Enable the unlocked gun
        //guns[index].SetActive(true);
    }

    // Method to get the currently equipped gun
    public GunClass GetCurrentGun()
    {
        return guns[currentGunIndex].GetComponent<GunClass>();
    }
}

