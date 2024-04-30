using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Event_On_Interact : MonoBehaviour, IInteract
{
    [SerializeField] private string contextTex = "Interact";
    [SerializeField] private bool validReinteract = true;
    [SerializeField] private UnityEvent me;
    public void Interaction()
    {
        me.Invoke();
    }
    public bool validToReinteract()
    {
        return validReinteract;
    }
    public string contextText()
    {
        return contextTex;
    }
}
