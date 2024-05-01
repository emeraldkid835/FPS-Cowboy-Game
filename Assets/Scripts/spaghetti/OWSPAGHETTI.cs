using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OWSPAGHETTI : MonoBehaviour
{
    [SerializeField] private GameObject allEnemies;
    [SerializeField] private npc mayor;
    [SerializeField] ParticleSystem piss;
    [SerializeField] private GameObject vfx;
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetInt("PlayerJump") > 1 && PlayerPrefs.GetInt("GunUnlocked_2") == 1)
        {
            mayor.SwapIndex(2);
            Destroy(allEnemies);
          
        }
        if(PlayerPrefs.GetInt("GunUnlocked_2") == 1)
        {
            piss.Stop();
            if(vfx != null)
            {
                vfx.SetActive(false);
            }
        }
    }

}
