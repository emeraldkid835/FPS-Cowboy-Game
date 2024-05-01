using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RANTHSPAGHETTI : MonoBehaviour
{
    [SerializeField] private GameObject allOpps;
    [SerializeField] private npc jibbler;
    // Start is called before the first frame update
    void Start()
    {
      if(PlayerPrefs.GetInt("GunUnlocked_2") == 1)
        {
            jibbler.SwapIndex(2);
            allOpps.SetActive(false);
        }   
    }

    
}
