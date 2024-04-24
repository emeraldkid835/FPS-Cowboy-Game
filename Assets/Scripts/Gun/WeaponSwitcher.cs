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
        // Disable all guns except the first one
        for (int i = 1; i < guns.Count; i++)
        {
            guns[i].gameObject.SetActive(false);
        }
        //if no player prefs do this:
        for (int i = 0; i < gunBools.Count; i++)
        {
            if(i == 0)
            {
                gunBools[i] = true;
            }
            else
            {
                gunBools[i] = false;
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

    public void SwitchGun(int offset)
    {
        if (isReloading == false && gunBools[currentGunIndex + offset] == true)
        {
            // Disable current gun
            guns[currentGunIndex].SetActive(false);

            // Calculate the index of the next gun
            currentGunIndex = (currentGunIndex + offset + guns.Count) % guns.Count;

            // Enable the next gun
            guns[currentGunIndex].SetActive(true);

            // Notify the InputManager about the weapon switch
            InputManager.instance.UpdateEquippedGun(guns[currentGunIndex].GetComponent<GunClass>());
        }
    }

    // Method to get the currently equipped gun
    public GunClass GetCurrentGun()
    {
        return guns[currentGunIndex].GetComponent<GunClass>();
    }
}

