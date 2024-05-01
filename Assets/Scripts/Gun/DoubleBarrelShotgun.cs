using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DoubleBarrelShotgun : GunClass
{
    public int currentBullets;
    private float nextTimeToFire;
    public GameObject muzzleflashprefab;
    public Transform muzzleflashLocation;
    private GameObject muzzleFlashInstance;
    private AudioSource audioSource;
    public AudioClip shootAudio;
    private int currentStoredAmmo;
    public int maxStoredAmmo = 16;
    private Recoil recoil;
    [SerializeField] private GameObject bulletVisualPrefab;
    [SerializeField] private AudioSource reloadSound;
    private Animator _animator;
    private WeaponSwitcher ws;
    private bool isReloading = false;

    public override float Damage => 20f;
    public override float Range => 35f;
    public override float FireRate => 3f;
    public override int MaxBulletsPerMagazine => 2;
    public override float ReloadTime => 4f;

    public uint pelletsPerShot = 6;
    [Range(0f,1f)] public float maxVariation = 0.2f; //tweak as necessary


    public override GameObject MuzzleFlashPrefab => muzzleflashprefab;
    public override AudioClip ShootSound => shootAudio;
    public override int CurrentBullets => currentBullets;
    public override int MaxStoredAmmo => 16;
    public override int CurrentStoredAmmo => currentStoredAmmo;

    public DoubleBarrelShotgun()
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
        ws = GameObject.Find("GunContainer").GetComponent<WeaponSwitcher>();
        _animator = GetComponent<Animator>();
    }

    // Implement shooting logic specific to the shotgun
    public override void Shoot()
    {
        // DoubleBarrel shooting logic
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

        Destroy(muzzleFlashInstance, 2f);
        // Play shoot audio
        audiomanager.instance.PlaySFX3D(shootAudio, muzzleflashLocation.position, 1f, 0.99f, 1.01f);
        // Apply recoil when shooting
        recoil.RecoilFire();
        // Perform a raycast to detect hits

        for (int i = 0; i < pelletsPerShot - 1; i++)
        {
            Vector3 direction = muzzleflashLocation.forward; //don't know why we can't just manipulate this, but spread appears to be a necessary double step
            if (i != 0) //first shot should be accurate
            {
                Vector3 spread = Vector3.zero;
                spread += muzzleflashLocation.up * Random.Range(-1f, 1f); //give spread some juicy
                spread += muzzleflashLocation.right * Random.Range(-1f, 1f);
                direction += Vector3.Normalize(spread) * Random.Range(0f, maxVariation);
            }//normalize to keep things sensible

            GameObject tempV = GameObject.Instantiate(bulletVisualPrefab);
            tempV.GetComponent<bullet_Visual>().renderPoint1 = muzzleflashLocation.position;
            tempV.GetComponent<bullet_Visual>().gunDirection = direction;
            tempV.GetComponent<bullet_Visual>().renderPoint2 = muzzleflashLocation.position + (direction * 3f);

            RaycastHit hit;
            if (Physics.Raycast(muzzleflashLocation.position, direction, out hit, Range))
            {
                Debug.DrawLine(muzzleflashLocation.position, hit.point, Color.yellow, 5f); //you need gizmos on for this bad boy
                // Handle hit detection, apply damage to the target, etc.
                Transform objectHit = hit.transform;
                if (objectHit.gameObject.tag != "Player")
                {
                    MonoBehaviour[] mono = objectHit.gameObject.GetComponents<MonoBehaviour>();

                    foreach (MonoBehaviour item in mono)
                    {
                        if (item is IDamage)
                        {
                            IDamage temp = item as IDamage;
                            temp.TakeDamage(Damage, IDamage.DamageType.Sharp);
                            break;
                        }
                    }
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
            if(reloadSound != null)
            {
                audiomanager.instance.PlaySFX3D(reloadSound.clip, this.transform.position, 0);
            }
            _animator.SetTrigger("Reload");
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
    }



    public IEnumerator Reloadtime()
    {
        Debug.Log("Hit Reload");
        if (!isReloading && currentBullets != maxStoredAmmo && currentStoredAmmo != 0)// Revolver reloading logic
        {
            isReloading = true;
            ws.isReloading = true;
            Debug.Log("Reloading");

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

