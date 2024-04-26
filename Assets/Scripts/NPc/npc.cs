using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npc : MonoBehaviour, IInteract
{
    [SerializeField] DialogKnower dialogcanv;
    [SerializeField] Dialog_Tree meDialog;
    // Start is called before the first frame update
    void Start()
    {
        if(dialogcanv == null)
        {
            Debug.Log("Trying to grab the convo knower!");
            dialogcanv = GameObject.Find("Dialogcanv").GetComponent<DialogKnower>();
        }
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
       
      
        dialogcanv.InitiateDialog(meDialog);
    }
}
