using System.Collections;
using System.Collections.Generic;
using UnityEngine;




    public class Gun : MonoBehaviour
    {
        Transform fpsCam;
        [SerializeField] Transform muzzleflashLocation;
        private AudioSource audioSource;

        [Header("Visual Effects")]
        public GameObject muzzleFlash;

        [Header("Sound Effects")]
        public AudioClip shootAudio;


        private GameObject MuzzleFlashInstance;

        [SerializeField] public float range = 300f;
        [SerializeField] public float damage = 50f;
        [SerializeField] public float fireRate = 10f;

        [SerializeField] public int MaxBullets = 6;
        [SerializeField] public int MaxStoredAmmo = 30;
        public float reloadTime = 1.5f;

        private int currentBullets;
        private int currentStoredAmmo;
        private bool isReloading = false;

        
        



        private void Awake()
        {
            fpsCam = Camera.main.transform; // Get the main camera transform for raycasting
        }

        private void Start()
        {
            audioSource = GetComponent<AudioSource>(); // Add an audioSource if it doesn't exist
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            currentBullets = MaxBullets;
            currentStoredAmmo = MaxStoredAmmo;
            

        }

        public void Shoot() // method that is called in my InputMaster script that handles Unity's new Input method
        {
            if (isReloading)
            {
                Debug.Log("reloading.. can't shoot!");
                return;
            }

            if (currentBullets <= 0)
            {
                // play clink audio to indicate "out of ammo"
                Debug.Log("Magazine Empty");
                return;
            }

            if (currentBullets > 0 && isReloading == false)
            {
                MuzzleFlashInstance = Instantiate(muzzleFlash, muzzleflashLocation.position, Quaternion.identity); // Instantiate and destroy the muzzle flash effect clone
                PlayAudio();
                Destroy(MuzzleFlashInstance, 2f);
                RaycastHit hit;  // Perform a raycast to detect objects within range of the gun
                if (Physics.Raycast(fpsCam.position, fpsCam.forward, out hit, range))
                {
                    Transform objectHit = hit.transform;

                    MonoBehaviour[] mono;
                    mono = objectHit.gameObject.GetComponents<MonoBehaviour>(); // Check if the object containing a monobehavior that was hit Implements the IDamage Interface
                    foreach (MonoBehaviour item in mono)
                    {
                         if(item is IDamage)
                         {
                              IDamage temp = item as IDamage; // Setting IDamage items as temp
                              Debug.Log($"{gameObject.name} took {damage} damage.");
                              temp.TakeDamage(damage);  // Calls the IDamage interface Method
                              return;
                         }
                    }
                
                }

                currentBullets--;
            }
            
            
        }

        IEnumerator Reload()
        {
            isReloading = true;
            Debug.Log("Reloading");

            yield return new WaitForSeconds(reloadTime);

            int bulletsToReload = Mathf.Min(MaxBullets - currentBullets, currentStoredAmmo);
            currentBullets += bulletsToReload;
            currentStoredAmmo -= bulletsToReload;

            isReloading = false;
        }

        void PlayAudio() // Method for playing the shoot audio
        {
            if (shootAudio != null)
            {
                audioSource.clip = shootAudio;
                audioSource.Play();
            }
            else
            {
                Debug.Log("AudioClip not assigned!");
            }
        }
    }

