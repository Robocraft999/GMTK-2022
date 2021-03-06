using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionItem : DragItem
{
    public ActionType Type { get; set; }

    public override string ToString()
    {
        return "ActionItem[" + Type.Name + "]";
    }
}
