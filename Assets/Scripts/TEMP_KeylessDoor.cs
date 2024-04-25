using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animation))]
[RequireComponent(typeof(Collider))]
public class TEMP_KeylessDoor : MonoBehaviour, IInteract
{
 
    [SerializeField] private float timeToclose = 5f;
    [SerializeField] private AudioSource openSound;
    [SerializeField] private AudioSource closeSound;
    [SerializeField] private Transform soundPosition;
    private Collider mycol;
    private float curTime;
    private Animation myAnim;
    private bool isOpen;
    private bool inTrigger;
   
    void Start()
    {
        //get everything set up.
        myAnim = this.GetComponent<Animation>();
        mycol = this.GetComponent<Collider>();
        mycol.isTrigger = true;
        isOpen = false;
        curTime = 0f;
    }

   
    void Update()
    {
        if (myAnim != null)//check if there are any animations at all, do nothing if not
        {
            
            if (isOpen == true && curTime < timeToclose && inTrigger == false) //timer logic
            {
                curTime += Time.deltaTime;
         
                //Debug.Log("Door is open, time spent open: " + curTime);
            }
            else if (isOpen == true && curTime >= timeToclose && inTrigger == false) //once timer is met, close the door
            {
                //Debug.Log("Should be rewinding!");
                curTime = 0f;
                myAnim[myAnim.clip.name].time = myAnim.clip.length;
                myAnim[myAnim.clip.name].speed = -1f;
                myAnim.Play();
               
                if(audiomanager.instance != null && closeSound != null)
                {
                    audiomanager.instance.PlaySFX3D(closeSound.clip, soundPosition.position);
                }
                isOpen = false;
                
            }
        }
    }
    public bool validToReinteract()
    {
        if(isOpen == true)
        {
            return false;
        }
        else { return true; }
    }
    public void Interaction()
    {
        if (myAnim != null)
        {
            //open the door
            if (isOpen == false)
            {
                myAnim[myAnim.clip.name].time = 0f;
                myAnim[myAnim.clip.name].speed = 1f;
                myAnim.Play();
                if (openSound != null && audiomanager.instance != null)
                {
                    audiomanager.instance.PlaySFX3D(openSound.clip, soundPosition.position);
                }
                
                isOpen = true;
            }
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
   
            inTrigger = true; //important to make sure door doesn't close on the player (unintentionally)
            curTime = 0f;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
   
            inTrigger = false;
        }
    }
}
