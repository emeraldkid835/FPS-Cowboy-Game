using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AmmoUpgradeTypes {  SMALLAMMOUPGRADE, LARGEAMMOUPGRADE }    
public class AmmoUpgrade : MonoBehaviour
{
    public AmmoUpgradeTypes ammoupgradeTypes;
    public int myValue;
    private GunClass equippedGun;

    void Start()
    {
        equippedGun = InputManager.instance.equippedGun;
        if (equippedGun == null)
        {
            Debug.Log("No weapon is equipped");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (ammoupgradeTypes)
            {
                case AmmoUpgradeTypes.SMALLAMMOUPGRADE:
                    equippedGun.AmmoUpgrade(myValue);
                    equippedGun.AddAmmo(myValue);
                    break;
                case AmmoUpgradeTypes.LARGEAMMOUPGRADE:
                    equippedGun.AmmoUpgrade(myValue);
                    equippedGun.AddAmmo(myValue);
                    break;
            }

            Destroy(gameObject);
        }
    }
}
