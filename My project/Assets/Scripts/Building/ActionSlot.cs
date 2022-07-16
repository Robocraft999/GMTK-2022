using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSlot : DragSlot
{
    public int slotId;
    //public new ActionItem CurrentItem { get; set; }

    public override string ToString()
    {
        return "ActionSlot[" + slotId + ", " + CurrentItem + "]";
    }
}
