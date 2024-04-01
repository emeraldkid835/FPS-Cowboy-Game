using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // prefab of the enemy to spawn
    public GameObject SpawnPosition;

    private void Awake()
    {
        SpawnPosition = GameObject.Find("SpawnPosition");

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        Instantiate(enemyPrefab, SpawnPosition.transform.position, Quaternion.identity);
    }
}
