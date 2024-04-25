using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class End_Generic : Generic_Node {
	//Child of the Generic_Node, can be used to exit any node tree/graph. Only has an entrance, because of course.
	[Input] public bool entry;
}