using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animation))]
[RequireComponent(typeof(Collider))]
public class TEMP_KeylessDoor : MonoBehaviour
{
    private bool canOpen;
    [SerializeField] private float timeToclose = 5f;
    private Collider mycol;
    private float curTime;
    private Animation myAnim;
    private bool isOpen;
    // Start is called before the first frame update
    void Start()
    {
        myAnim = this.GetComponent<Animation>();
        mycol = this.GetComponent<Collider>();
        mycol.isTrigger = true;
        canOpen = false;
        isOpen = false;
        curTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (myAnim != null)
        {
            if (canOpen == true && Input.GetKeyDown(KeyCode.E))
            {
                myAnim[myAnim.clip.name].time = 0f;
                myAnim[myAnim.clip.name].speed = 1f;
                myAnim.Play();
                
                canOpen = false;
                isOpen = true;
            }
            if (isOpen == true && curTime < timeToclose)
            {
                curTime += Time.deltaTime;
                Debug.Log("Door is open, time spent open: " + curTime);
            }
            else if (isOpen == true && curTime >= timeToclose)
            {
                Debug.Log("Should be rewinding!");
                curTime = 0f;
                myAnim[myAnim.clip.name].time = myAnim.clip.length;
                myAnim[myAnim.clip.name].speed = -1f;
                myAnim.Play();
               
               
                isOpen = false;
                canOpen = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && isOpen == false)
        {
            canOpen = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            canOpen = false;
        }
    }
}
