using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public class npc : MonoBehaviour, IInteract
{
    DialogKnower dialogcanv;
    [SerializeField] Dialog_Tree[] meDialog;
    
    [SerializeField] private uint dialogIndex; //becomes pointless if randomDialogs is true
    [SerializeField] public bool randomDialogs;
  
    void Start()
    { 
        dialogcanv = DialogKnower.instance; //should be convenient
    }

    
    public bool validToReinteract()
    {
        //don't reinit dialog if we already got one, also hide the ui for interacting
        if (dialogcanv.gameObject.GetComponent<Canvas>().enabled == true)
        {
            return false;
        }
        else { return true; }
    }
    public void Interaction()
    {
        //do dialog stuff
        if (randomDialogs == true) 
        {
            int temp = Mathf.RoundToInt(Random.Range(0, meDialog.Length - 1));
            Debug.Log("Rolled index value: " + temp);
            dialogcanv.InitiateDialog(meDialog[temp]);
        }
        else
        {
            dialogcanv.InitiateDialog(meDialog[dialogIndex]);
        }
    }
}
