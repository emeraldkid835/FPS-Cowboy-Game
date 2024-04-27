using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.AnimatedValues;
#endif
using UnityEngine;
using XNode;

public class NPC_Dialogue : Dialog_Base_Node
{
    [Input(typeConstraint = TypeConstraint.Strict)] //only let exits go to entrances, and entrances to exits! Only the same var type can be connected .
    public bool entry;

    [Output(dynamicPortList = true, connectionType = ConnectionType.Override)]
    public int exit;    //Reassigning output deletes the previous connection, instead of leaving it from ConnectionType.Override.
                        //Easier access to multiple ports from dynamicPortList.

    //Child node of dialog node, specifically for NPCs. It also holds the ports to all possible player responses.

    public override string GetDialogueType { get { return "NPC"; } }
}