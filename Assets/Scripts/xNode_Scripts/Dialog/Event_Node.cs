using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class Event_Node : Dialog_Base_Node {

    public override string GetDialogueType { get { return "Event"; } }
    public Game_Event eventToDo;

    [Input] public bool entry;
    [Output] public bool exit;
}