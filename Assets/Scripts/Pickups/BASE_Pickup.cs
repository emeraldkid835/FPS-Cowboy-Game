using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Animation))]
[RequireComponent(typeof(AudioSource))]
public abstract class BASE_Pickup : MonoBehaviour
{
    private Collider myCol;
    private Animation myAnim;
    [SerializeField] private AudioSource pickupSFX;
    [SerializeField] private AudioSource ambientSFX;
  
    protected void Start()
    {
        myCol = GetComponent<Collider>();
        myAnim = GetComponent<Animation>();
        pickupSFX = GetComponent<AudioSource>();

        myCol.isTrigger = true;
        myAnim.Play();
    }

    // Update is called once per frame
    protected void Update()
    {
        if (myAnim.isPlaying == false) //basic loop logic for the animation and ambientSFX (assuming they exist, of course)
        {
            myAnim.Play();
        }
        if (ambientSFX.isPlaying == false)
        {
            ambientSFX.Play();
        }
    }

    //should change sound effect behaviors if we get an audio manager implemented.

    protected void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            pickupSFX.Play();
            ambientSFX.Stop();
            PickupBehavior();
            this.gameObject.GetComponent<MeshRenderer>().enabled = false; //hide the gameobject while still letting logic run, if necessary
            myCol.enabled = false;
        }
    }

    protected abstract void PickupBehavior(); //this the mf you override to make the pickups do things
}
