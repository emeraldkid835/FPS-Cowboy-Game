using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhereToSpawn : MonoBehaviour
{
    [SerializeField] public bool spawnAtHome;
    [SerializeField] public bool spawnAtRanch;
    [SerializeField] public bool spawnAtFactory;
    [SerializeField] public bool spawnAtBoss;



    public void SetSpawnToHome()
    {
        spawnAtHome = true;
        PlayerPrefs.SetInt("SpawnAtHome", spawnAtHome ? 1 : 0);
        spawnAtRanch = false;
        PlayerPrefs.SetInt("SpawnAtRanch", spawnAtRanch ? 1 : 0);
        spawnAtFactory = false;
        PlayerPrefs.SetInt("SpawnAtFactory", spawnAtFactory ? 1 : 0);
        spawnAtBoss = false;
        PlayerPrefs.SetInt("SpawnAtBoss", spawnAtBoss ? 1 : 0);
    }

    public void SetSpawnToRanch()
    {
        spawnAtHome = false;
        PlayerPrefs.SetInt("SpawnAtHome", spawnAtHome ? 1 : 0);
        spawnAtRanch = true;
        PlayerPrefs.SetInt("SpawnAtRanch", spawnAtRanch ? 1 : 0);
        spawnAtFactory = false;
        PlayerPrefs.SetInt("SpawnAtFactory", spawnAtFactory ? 1 : 0);
        spawnAtBoss = false;
        PlayerPrefs.SetInt("SpawnAtBoss", spawnAtBoss ? 1 : 0);
    }

    public void SetSpawnToFactory()
    {
        spawnAtHome = false;
        PlayerPrefs.SetInt("SpawnAtHome", spawnAtHome ? 1 : 0);
        spawnAtRanch = false;
        PlayerPrefs.SetInt("SpawnAtRanch", spawnAtRanch ? 1 : 0);
        spawnAtFactory = true;
        PlayerPrefs.SetInt("SpawnAtFactory", spawnAtFactory ? 1 : 0);
        spawnAtBoss = false;
        PlayerPrefs.SetInt("SpawnAtBoss", spawnAtBoss ? 1 : 0);
    }

    public void SetSpawnToBoss()
    {
        spawnAtHome = false;
        PlayerPrefs.SetInt("SpawnAtHome", spawnAtHome ? 1 : 0);
        spawnAtRanch = false;
        PlayerPrefs.SetInt("SpawnAtRanch", spawnAtRanch ? 1 : 0);
        spawnAtFactory = false;
        PlayerPrefs.SetInt("SpawnAtFactory", spawnAtFactory ? 1 : 0);
        spawnAtBoss = true;
        PlayerPrefs.SetInt("SpawnAtBoss", spawnAtBoss ? 1 : 0);
    }
}
