using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCulling : MonoBehaviour
{
    [SerializeField] public float cullingDistance = 300f;
    private Transform playerCamera;

    private void Start()
    {
        playerCamera = Camera.main.transform;
    }

    private void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            float distanceToCamera = Vector3.Distance(enemy.transform.position, playerCamera.position);

            if(distanceToCamera > cullingDistance)
            {
                SkinnedMeshRenderer meshRenderer = enemy.GetComponentInChildren<SkinnedMeshRenderer>();

                if (meshRenderer != null)
                {
                    meshRenderer.enabled = false;
                }
            }
            else
            {
                SkinnedMeshRenderer meshRenderer = enemy.GetComponentInChildren<SkinnedMeshRenderer>();
                if (meshRenderer != null)
                {
                    meshRenderer.enabled = true;
                }
            }
        }
    }
}
