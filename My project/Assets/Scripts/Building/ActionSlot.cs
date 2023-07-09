using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSlot : DragSlot
{
    public int slotId;
    //public new ActionItem CurrentItem { get; set; }
    public List<ActionItem> Items { get; set; } = new List<ActionItem> ();
    public int MaxItems;
    public new bool SlotFilled => Items.Count == MaxItems;

    public override string ToString()
    {
        return "ActionSlot[" + slotId + ", {" + string.Join(";", Items) + "}]";
    }
}
