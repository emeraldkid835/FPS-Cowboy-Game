using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Winchester : GunClass
{
    [SerializeField] private GameObject bulletVisualPrefab;
    public int currentBullets;
    private float nextTimeToFire;
    public GameObject muzzleflashprefab;
    public Transform muzzleflashLocation;
    private GameObject muzzleFlashInstance;
    private AudioSource audioSource;
    public AudioClip shootAudio;
    private int currentStoredAmmo;
    public int maxStoredAmmo = 25;
    private Recoil recoil;
    private WeaponSwitcher ws;

    private bool isReloading = false;

    [SerializeField] PlayerPause playerPause;

    public override float Damage => 60f;
    public override float Range => 300f;
    public override float FireRate => 3f;
    public override int MaxBulletsPerMagazine => 8;
    public override float ReloadTime => 3f;



    public override GameObject MuzzleFlashPrefab => muzzleflashprefab;
    public override AudioClip ShootSound => shootAudio;
    public override int CurrentBullets => currentBullets;
    public override int MaxStoredAmmo => maxStoredAmmo;
    public override int CurrentStoredAmmo => currentStoredAmmo;
    public Winchester()
    {

        currentBullets = MaxBulletsPerMagazine;
        maxStoredAmmo = MaxStoredAmmo;
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
        ws = GameObject.Find("GunContainer").GetComponent<WeaponSwitcher>();
        playerPause = GameObject.Find("GoodPlayer").GetComponent<PlayerPause>();
    }

    // Implement shooting logic specific to the Revolver
    public override void Shoot()
    {
        if(playerPause.isPaused == false)
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
                Reload();
                return;
            }
            if (Time.time < nextTimeToFire || currentBullets <= 0)
                return;

            // Instantiate muzzle flash prefab at the muzzle flash location
            muzzleFlashInstance = Instantiate(MuzzleFlashPrefab, muzzleflashLocation.position, muzzleflashLocation.rotation);

            // Give Recoil


            Destroy(muzzleFlashInstance, 2f);
            // Play shoot audio
            audiomanager.instance.PlaySFX3D(shootAudio, muzzleflashLocation.position, 1f, 0.99f, 1.01f);

            // Perform a raycast to detect hits
            RaycastHit hit;
            GameObject temp = GameObject.Instantiate(bulletVisualPrefab);
            temp.GetComponent<bullet_Visual>().renderPoint1 = muzzleflashLocation.position;
            temp.GetComponent<bullet_Visual>().gunDirection = muzzleflashLocation.forward;
            temp.GetComponent<bullet_Visual>().renderPoint2 = muzzleflashLocation.position + (muzzleflashLocation.forward * 3f);
            if (Physics.Raycast(muzzleflashLocation.position, muzzleflashLocation.forward, out hit, Range))
            {
                Debug.DrawLine(muzzleflashLocation.position, hit.point, Color.yellow, 5f);
                // Handle hit detection, apply damage to the target, etc.
                Transform objectHit = hit.transform;
                if (objectHit.gameObject.tag != "Player") //no more self damage!
                {

                    MonoBehaviour[] mono = objectHit.gameObject.GetComponents<MonoBehaviour>();

                    foreach (MonoBehaviour item in mono)
                    {
                        if (item is IDamage)
                        {
                            IDamage tempI = item as IDamage;
                            tempI.TakeDamage(Damage, IDamage.DamageType.Sharp);
                            break;
                        }
                    }
                }
            }

            recoil.RecoilFire();
            // Update next time the pistol can fire
            nextTimeToFire = Time.time + 1f / FireRate;
            // Reduce current bullets
            currentBullets--;
        }
        
    }



    // Implement reloading logic specific to the Revolver
    public override void Reload()
    {
        if (playerPause.isPaused == false && isReloading != true && currentBullets < MaxBulletsPerMagazine)
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

    public override void AmmoUpgrade(int amount)
    {
        maxStoredAmmo += amount;
        Debug.Log("maxStoredAmmo+10");
    }



    public IEnumerator Reloadtime()
    {
        Debug.Log("Hit Reload");
        if (!isReloading && currentBullets != maxStoredAmmo && currentStoredAmmo != 0)// Winchester reloading logic
        {
            isReloading = true;
            ws.isReloading = true;
            Debug.Log("Reloading");
            // start reloading Audio
            // reloading animation here too
            yield return new WaitForSeconds(ReloadTime);

            Debug.Log("Done Reloading");

            int bulletsToReload = Mathf.Min(MaxBulletsPerMagazine - currentBullets, currentStoredAmmo);
            currentBullets += bulletsToReload;
            currentStoredAmmo -= bulletsToReload;

            isReloading = false;
            ws.isReloading = false;
        }

    }
}
