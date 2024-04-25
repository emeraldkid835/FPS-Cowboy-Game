using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XNode;
[RequireComponent(typeof(Canvas))]
public class DialogKnower : MonoBehaviour
{
    public Dialog_Tree curDialog;
    public Text SpokenText;
    public Transform ResponsePanel;
    public GameObject buttonPrefab;
    [SerializeField] private PlayerPause pauser;
    private Canvas me;
    // Start is called before the first frame update
    private void Start()
    {
        if(pauser == null)
        {
            pauser = GameObject.Find("GoodPlayer").GetComponent<PlayerPause>(); 
        }
        this.gameObject.SetActive(false);
    }
    public void InitiateDialog()
    {
        if (curDialog != null)
        {
            this.gameObject.SetActive(true);
            foreach (Node item in curDialog.nodes)
            {
                if (item is Start_Generic)
                {
                    curDialog.current = item.GetPort("exit").Connection.node as Dialog_Base_Node;
                    break;
                }
            }
   
          
            pauser.PanicPause();
            ParseNode();
        }
        else
        {
            Debug.Log("Assign the dialog tree, you bumble-headed buffoon!");
        }
    }

    void ParseNode()
    {
        if (curDialog.current == null || SpokenText == null)
        {
            return;
        }

        switch (curDialog.current.GetDialogueType)
        {   
            //specific bug for NPc dialog (mostly because it would be a pain in the ass to change), you can only put prog quest, add quest,
            //and event nodes after player dialog, since npc doesn't automatically go through exit due to it's behavior, too bad!
            case "NPC":

                SpokenText.text = curDialog.current.dialogueText;
                spawnPlayerDialog();
                
                Debug.Log("NPc dialog passed");
                break;

            case "Player":

                clearPlayerDialog();
                NextNode("exit");
                Debug.Log("Player dialog passed");
                break;

            
            case "Event":

                Debug.Log("Event node hit?");
                Event_Node temp = curDialog.current as Event_Node;
                if (temp.eventToDo != null)
                {
                    temp.eventToDo.Raise();
                }
                else
                {
                    Debug.Log("No event assigned to event node!");
                }
                NextNode("exit");

                break;

        }


    }
    private void spawnPlayerDialog()
    {
        foreach (NodePort port in curDialog.current.Ports)
        {
            if (port.IsInput || port.Connection == null)
            {
                continue;
            }

            if (port.Connection.node is Player_Dialogue)
            {
                Player_Dialogue pd = port.Connection.node as Player_Dialogue;
                Button b = Instantiate(buttonPrefab, ResponsePanel).GetComponent<Button>();
                b.onClick.AddListener(() => NextNode(port.fieldName));
                b.GetComponentInChildren<Text>().text = pd.dialogueText;
            }
        }
    }

    public void NextNode(string fieldName)
    {
        foreach (NodePort port in curDialog.current.Ports)
        {
            if (port.IsInput || port.Connection == null)
            {
                continue;
            }

            if (port.fieldName == fieldName)
            {
                curDialog.current = port.Connection.node as Dialog_Base_Node;
                break;
            }
        }

        ParseNode();
    }

    public void clearPlayerDialog()
    {
        Transform[] children = ResponsePanel.GetComponentsInChildren<Transform>();

        for (int i = children.Length - 1; i >= 0; i--)
        {
            if (children[i] != ResponsePanel)
            {
                Destroy(children[i].gameObject);
            }
        }
    }

    public void ExitDialog()
    {
        clearPlayerDialog();
        pauser.Resume();
        me.enabled = false;
        this.gameObject.SetActive(false);
    }
    
}
