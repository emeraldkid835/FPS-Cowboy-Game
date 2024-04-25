using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
#if UNITY_EDITOR
using XNodeEditor;
#endif
using System;

[CreateAssetMenu]
[RequireNodeAttribute(typeof(Start_Generic),typeof(End_Generic))]
public class Dialog_Tree : NodeGraph 
{
	//This bad boy should make a node graph when using the create menu in editor.
	//Incase you can't read, it should be used for dialog trees. No functionality to currently enforce that.

	public Dialog_Base_Node current;
}
#if UNITY_EDITOR
[CustomNodeGraphEditor(typeof(Dialog_Tree))]
public class DialogEditor : NodeGraphEditor
{
    public override string GetNodeMenuName(Type type)
    {
        
        if (typeof(Dialog_Base_Node).IsAssignableFrom(type) || typeof(Generic_Node).IsAssignableFrom(type))
        {
            return base.GetNodeMenuName(type);
        }
        else
        {
            return null;
        }
    }
}
#endif