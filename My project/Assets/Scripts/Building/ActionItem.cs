using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionItem : DragItem
{
    public ActionType Type { get; set; }
    public new ActionSlot currentSlot;

    public override void OnBeginDrag(PointerEventData data)
    {
        GetComponentInChildren<LayoutElement>().ignoreLayout = true;
        base.OnBeginDrag(data);
    }

    public override void OnEndDrag(PointerEventData data)
    {
        var results = new List<RaycastResult>();
        graphicRaycaster.Raycast(data, results);
        foreach (var hit in results)
        {
            var slot = hit.gameObject.GetComponentInChildren<ActionSlot>();
            if (slot)
            {
                if (!slot.SlotFilled)
                {
                    currentSlot.Items.Remove(this);
                    currentSlot = slot;
                    currentSlot.Items.Add(this);
                }
                break;
            }
        }
        transform.SetParent(currentSlot.transform);
        transform.SetAsLastSibling();
        transform.localPosition = Vector3.zero;
        GetComponentInChildren<LayoutElement>().ignoreLayout = false;
    }

    public override string ToString()
    {
        return "ActionItem[" + Type.Name + "]";
    }
}
