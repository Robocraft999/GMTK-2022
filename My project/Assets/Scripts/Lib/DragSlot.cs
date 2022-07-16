using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragSlot : MonoBehaviour {

    public DragItem CurrentItem { get; set; }
    public bool SlotFilled => CurrentItem;
}
