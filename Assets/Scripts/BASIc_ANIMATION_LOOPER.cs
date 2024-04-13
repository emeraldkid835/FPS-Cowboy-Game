using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animation))]
public class BASIc_ANIMATION_LOOPER : MonoBehaviour
{
    private Animation me;
    // Start is called before the first frame update
    void Start()
    {
        if(me == null)
        {
            me = GetComponent<Animation>();
        }
        me.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if(me.isPlaying == false)
        {
            me.Play();
        }
    }
}
