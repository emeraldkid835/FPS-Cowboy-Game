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
    // Start is called before the first frame update
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
        if (myAnim.isPlaying == false)
        {
            myAnim.Play();
        }
        if (ambientSFX.isPlaying == false)
        {
            ambientSFX.Play();
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            pickupSFX.Play();
            ambientSFX.Stop();
            PickupBehavior();
            this.gameObject.GetComponent<MeshRenderer>().enabled = false;
            myCol.enabled = false;
        }
    }

    protected abstract void PickupBehavior();
}
