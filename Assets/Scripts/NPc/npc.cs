using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class npc : MonoBehaviour, IInteract
{
    DialogKnower dialogcanv;
    [SerializeField] Dialog_Tree[] meDialog;
    [SerializeField] private string contextTex = "Speak";
    [SerializeField] private int dialogIndex = 0; //becomes pointless if randomDialogs is true
    [SerializeField] private bool randomDialogs;
    [SerializeField] private string meName;

    public void SwapIndex(int newIndex)
    {
        dialogIndex = newIndex;
        PlayerPrefs.SetInt(meName, dialogIndex.ConvertTo<int>());
        PlayerPrefs.Save();
    }

    void Start() //IMPORTANT, NEVER cHANGE NPc NAMES!
    { 
        if(meName == null)
        {
            meName = this.gameObject.name;
        }
        if(meName != this.gameObject.name)
        {
            Debug.Log("This gameobject: " + gameObject.name + " is now bricked, player preferences are probably fucked, damn you. Wipe the prefs, under the edit menu.");
            Destroy(this.gameObject);
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode(); //only if editor, duh!
#endif
        }
        
        dialogcanv = DialogKnower.instance; //should be convenient
        
        if (randomDialogs == false)
        {
            dialogIndex = PlayerPrefs.GetInt(meName).ConvertTo<int>();
        }
    }

    public string contextText()
    {
        return contextTex;
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
            float temper = Random.Range(0, meDialog.Length); //SHOULD BE LENGTH -1, BUT THAT DOESN'T WORK???????????
            Debug.Log("Rolled value: " + temper);
            int temp = Mathf.RoundToInt(temper);
            Debug.Log("Rounded value: " + temp);
            dialogcanv.InitiateDialog(meDialog[temp]);
        }
        else
        {
            dialogcanv.InitiateDialog(meDialog[dialogIndex]);
        }
    }
}
