using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolver : GunClass
{
    public int currentBullets;
    private float nextTimeToFire;
    public GameObject muzzleflashprefab;
    public Transform muzzleflashLocation;
    private GameObject muzzleFlashInstance;
    private AudioSource audioSource;
    public AudioClip shootAudio;
    private int currentStoredAmmo;
    public int maxStoredAmmo = 30;
    private Recoil recoil;

    private bool isReloading = false;

    public override float Damage => 30f;
    public override float Range => 300f;
    public override float FireRate => 5f;
    public override int MaxBulletsPerMagazine => 6;
    public override float ReloadTime => 3f;



    public override GameObject MuzzleFlashPrefab => muzzleflashprefab;
    public override AudioClip ShootSound => shootAudio;
    public override int CurrentBullets => currentBullets;
    public override int MaxStoredAmmo => 30;
    public override int CurrentStoredAmmo => currentStoredAmmo;
    public Revolver()
    {
        
        currentBullets = MaxBulletsPerMagazine;
        
        currentStoredAmmo = MaxStoredAmmo;
        muzzleflashprefab = MuzzleFlashPrefab;
        
    }

    public void Awake()
    {
        audioSource = GetComponent<AudioSource>(); // Add an audioSource if it doesn't exist
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        recoil = GameObject.Find("CameraRot/CameraRecoil").GetComponent<Recoil>();
    }

    // Implement shooting logic specific to the Revolver
    public override void Shoot()
    {
        // Revolver shooting logic
        if (isReloading)
        {
            Debug.Log("Reloading... can't shoot yet");
            return;
        }
        if (currentBullets <= 0)
        {
            Debug.Log("Weapon Empty!");
            return;
        }
        if (Time.time < nextTimeToFire || currentBullets <= 0)
            return;

        // Instantiate muzzle flash prefab at the muzzle flash location
        muzzleFlashInstance = Instantiate(MuzzleFlashPrefab, muzzleflashLocation.position, muzzleflashLocation.rotation);

        // Give Recoil
        recoil.RecoilFire();

        Destroy(muzzleFlashInstance, 2f);
        // Play shoot audio
        audioSource.PlayOneShot(shootAudio);

        // Perform a raycast to detect hits
        RaycastHit hit;
        if (Physics.Raycast(muzzleflashLocation.position, muzzleflashLocation.forward, out hit, Range))
        {
            // Handle hit detection, apply damage to the target, etc.
            Transform objectHit = hit.transform;
            MonoBehaviour[] mono = objectHit.gameObject.GetComponents<MonoBehaviour>();

            foreach (MonoBehaviour item in mono)
            {
                if (item is IDamage)
                {
                    IDamage temp = item as IDamage;
                    temp.TakeDamage(Damage);
                    break;
                }
            }
        }


        // Update next time the pistol can fire
        nextTimeToFire = Time.time + 1f / FireRate;
        // Reduce current bullets
        currentBullets--;
    }



    // Implement reloading logic specific to the Revolver
    public override void Reload()
    {
        if (isReloading != true && currentBullets < MaxBulletsPerMagazine)
        {
            StartCoroutine(Reloadtime());
        }

    }

    public override void AddAmmo(int amount)
    {
        if (currentStoredAmmo < maxStoredAmmo)
        {
            currentStoredAmmo = Mathf.Min(currentStoredAmmo + amount, maxStoredAmmo);
        }
    }



    public IEnumerator Reloadtime()
    {
        Debug.Log("Hit Reload");
        if (!isReloading && currentBullets != maxStoredAmmo && currentStoredAmmo != 0)// Revolver reloading logic
        {
            isReloading = true;
            Debug.Log("Reloading");
            // start reloading Audio

            yield return new WaitForSeconds(ReloadTime);

            Debug.Log("Done Reloading");

            int bulletsToReload = Mathf.Min(MaxBulletsPerMagazine - currentBullets, currentStoredAmmo);
            currentBullets += bulletsToReload;
            currentStoredAmmo -= bulletsToReload;

            isReloading = false;
        }

    }
}
