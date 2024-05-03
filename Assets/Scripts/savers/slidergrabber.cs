using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class slidergrabber : MonoBehaviour
{
    [SerializeField] Slider slidX;
    [SerializeField] Slider slidY;
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetFloat("sensX") != 0)
        {
            slidX.value = PlayerPrefs.GetFloat("sensX");
        }
        if(PlayerPrefs.GetFloat("sensY") != 0)
        {
            slidY.value = PlayerPrefs.GetFloat("sensY");
        }
    }

   
}
