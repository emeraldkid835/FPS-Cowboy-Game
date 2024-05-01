using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPosition : MonoBehaviour
{
    
    public Transform playerfromRanchPosition;
    public Transform playerfromFactoryPosition;
    public Transform playerfromBossPosition;

    public GameObject GoodPlayer;

    private void Awake()
    {
        GoodPlayer = GameObject.FindGameObjectWithTag("Player");
    }


    public void SpawnPlayerOutsideRanch()
    {
        GoodPlayer.transform.position = playerfromRanchPosition.transform.position;
    }

    public void SpawnPlayerOutsideFactoryPosition()
    {
        GoodPlayer.transform.position = playerfromFactoryPosition.transform.position;
    }

    public void SpawnPlayerOutsideBossPosition()
    {
        GoodPlayer.transform.position = playerfromBossPosition.transform.position;
    }


}
