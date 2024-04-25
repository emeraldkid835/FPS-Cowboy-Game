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
            dialogcanv = GameObject.Find("Dialogcanv").GetComponent<DialogKnower>();
        }if (dialogcanv.gameObject.activeSelf == true)
        {
            dialogcanv.gameObject.SetActive(false);
        }
    }

    
    public bool validToReinteract()
    {
        if(dialogcanv.gameObject.activeSelf == true)
        {
            return false;
        }
        else { return true; }
    }
    public void Interaction()
    {
        //do dialog stuff
        dialogcanv.curDialog = meDialog;
        dialogcanv.gameObject.SetActive(true);
        dialogcanv.InitiateDialog();
    }
}
