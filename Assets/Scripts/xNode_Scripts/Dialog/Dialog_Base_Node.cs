using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
#if UNITY_EDITOR
using XNodeEditor;
#endif
using System;

public abstract class Dialog_Base_Node : Node 
{
	[TextArea] // <- this bad boy makes the juicy string readable! yummy indeed!
	public string dialogueText; 
	public abstract string GetDialogueType { get; }
	//this node cannot be made, it is the parent for NPC and Player Dialogue nodes.
	 
	//If all Dialogue nodes need some new functionality, put it here. 

	//All nodes will also have an abstract public string that will determine information
	//for other scripts communication with the child scripts.
}