using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class Start_Generic : Generic_Node{
	//Child of the Generic_Node, can be used to enter any node tree/graph. Only has an exit, because of course.
	[Output(connectionType = ConnectionType.Override)] public bool exit;
}