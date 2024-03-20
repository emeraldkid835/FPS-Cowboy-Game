using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingedBootsPickup : BASE_Pickup
{
    [SerializeField] private GameObject _player;

    protected override void PickupBehavior()
    {
        Debug.Log("I was a picked up boy!");
        _player.GetComponent<PlayerMovement>().jumpAmountIncrease(1);
        
    }
}
