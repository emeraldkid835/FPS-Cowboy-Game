using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AmmoDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI CurrentAmmoText;
    [SerializeField] private TextMeshProUGUI CurrentStoredAmmoText;

    private GunClass activeGun;

    private void Start()
    {
        if (CurrentAmmoText == null || CurrentStoredAmmoText == null)
        {
            Debug.LogError("Need to assign the currentAmmo text and CurrentStoredAmmoText in the inspector");
            return;
        }

        activeGun = FindObjectOfType<GunClass>();
        if (activeGun == null)
        {
            Debug.LogError("Can't find gun in scene!");
            return;
        }
        UpdateCurrentAmmo(activeGun.CurrentBullets);
        UpdateCurrentStoredAmmo(activeGun.CurrentStoredAmmo);
    }

    private void Update()
    {
        UpdateCurrentAmmo(activeGun.CurrentBullets);
        UpdateCurrentStoredAmmo(activeGun.CurrentStoredAmmo);
    }

    void UpdateCurrentAmmo(int currentAmmo)
    {
        CurrentAmmoText.text = "Current Ammo: " + currentAmmo.ToString();
    }

    void UpdateCurrentStoredAmmo(int currentStoredAmmo)
    {
        CurrentStoredAmmoText.text = "Stored Ammo: " + currentStoredAmmo.ToString();
    }
}
