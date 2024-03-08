using System.Collections;
using System.Collections.Generic;
using UnityEngine;




    public class Gun : MonoBehaviour
    {
        Transform cam;
        [SerializeField] Transform muzzleflashLocation;
        private AudioSource audioSource;

        [Header("Visual Effects")]
        public GameObject muzzleFlash;

        [Header("Sound Effects")]
        public AudioClip shootAudio;


        private GameObject MuzzleFlashInstance;

        [SerializeField] float range = 100f;
        [SerializeField] float damage = 50f;

        private void Awake()
        {
            cam = Camera.main.transform; // Get the main camera transform for raycasting
        }

        private void Start()
        {
            audioSource = GetComponent<AudioSource>(); // Add an audioSource if it doesn't exist
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        public void Shoot() // method that is called in my InputMaster script that handles Unity's new Input method
        {
            MuzzleFlashInstance = Instantiate(muzzleFlash, muzzleflashLocation.position, Quaternion.identity); // Instantiate and destroy the muzzle flash effect clone
            PlayAudio();
            Destroy(MuzzleFlashInstance, 2f);
            RaycastHit hit;  // Perform a raycast to detect objects within range of the gun
            if (Physics.Raycast(cam.position, cam.forward, out hit, range))
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

