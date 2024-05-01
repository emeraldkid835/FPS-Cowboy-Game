using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] private Transform HomeSpawnPoint;
    [SerializeField] private Transform RanchSpawnPoint;
    [SerializeField] private Transform FactorySpawnPoint;
    [SerializeField] private Transform BossSpawnPoint;

    [SerializeField] GameObject Player;
    [SerializeField] WhereToSpawn boolscript;

    private void Awake()
    {
        HomeSpawnPoint = GameObject.Find("HomeSpawnPoint").GetComponent<Transform>();
        RanchSpawnPoint = GameObject.Find("RanchSpawnPoint").GetComponent<Transform>();
        FactorySpawnPoint = GameObject.Find("FactorySpawnPoint").GetComponent<Transform>();
        BossSpawnPoint = GameObject.Find("BossSpawnPoint").GetComponent<Transform>();

        Player = GameObject.Find("GoodPlayer");
        boolscript = GameObject.Find("GoodPlayer").GetComponent<WhereToSpawn>();
    }

    private void Start()
    {
        boolscript.spawnAtHome = PlayerPrefs.GetInt("SpawnAtHome", 0) == 1;
        boolscript.spawnAtRanch = PlayerPrefs.GetInt("SpawnAtRanch", 0) == 1;
        boolscript.spawnAtFactory = PlayerPrefs.GetInt("SpawnAtFactory", 0) == 1;
        boolscript.spawnAtBoss = PlayerPrefs.GetInt("SpawnAtBoss", 0) == 1;
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        if (boolscript.spawnAtHome == true)
        {
            Player.transform.position = HomeSpawnPoint.position;
        }
        if (boolscript.spawnAtRanch == true)
        {
            Player.transform.position = RanchSpawnPoint.position;
        }
        if (boolscript.spawnAtFactory == true)
        {
            Player.transform.position = FactorySpawnPoint.position;
        }
        if (boolscript.spawnAtBoss == true)
        {
            Player.transform.position = BossSpawnPoint.position;
        }
    }


}
