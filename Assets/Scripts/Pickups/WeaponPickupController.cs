using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickupController : MonoBehaviour
{
    public WeaponPickup weaponpickup;
    public Rigidbody rb;
    public BoxCollider coll;
    public Transform player, gunContainer, fpsCam;

    public float pickUpRange;
    public float dropForwardForce, dropUpwardForce;

    public bool equipped;
    public static bool slotFull;

    private void Update()
    {
        // Check if the player is in range
        Vector3 distanceToPlayer = player.position - transform.position;

    }
}
