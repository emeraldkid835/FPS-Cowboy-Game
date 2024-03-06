using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageObserver
{
    void OnDamageTaken(float currentHealth);
}
