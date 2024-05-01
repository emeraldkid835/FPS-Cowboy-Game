using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class Player_Dialogue : Dialog_Base_Node {

	[Input(typeConstraint = TypeConstraint.Strict)] //Only connects to same var type.
	public int entry;

	[Output(connectionType = ConnectionType.Override)] //Overrides old connection if you reassign, instead of having two connections.
	public bool exit;

    //Child node of dialog node, specifically for Players. It will only hold one in, and one out port.
    //Currently no way to restrict certain dialog options based on conditions. (ie. no item necessary, or not enough skill level required)
    //(Supposedly ^^^ can be done with scriptable objects.)

    public override string GetDialogueType { get { return "Player"; } }
}