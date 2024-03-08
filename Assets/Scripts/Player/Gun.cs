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
            cam = Camera.main.transform;
        }

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        public void Shoot()
        {
            MuzzleFlashInstance = Instantiate(muzzleFlash, muzzleflashLocation.position, Quaternion.identity);
            PlayAudio();
            RaycastHit hit;
            if (Physics.Raycast(cam.position, cam.forward, out hit, range))
            {
                print(hit.collider.tag);

                if (hit.collider.CompareTag("ExplodingBarrel"))
                {
                    DamageSystem.DamageEvent.TriggerDamage(damage);
                }
            }
            Destroy(MuzzleFlashInstance, 2f);
        }

        void PlayAudio()
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

