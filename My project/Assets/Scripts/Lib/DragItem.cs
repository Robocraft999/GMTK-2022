using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public DragSlot currentSlot;
    protected Canvas canvas;
    protected GraphicRaycaster graphicRaycaster;

    public virtual void OnBeginDrag(PointerEventData data)
    {
        transform.SetParent(canvas.transform, false);
        transform.SetAsLastSibling();

        transform.position = Input.mousePosition;
    }

    public void OnDrag(PointerEventData data)
    {
        //transform.localPosition += new Vector3(data.delta.x, data.delta.y) / transform.lossyScale.x;
        transform.position = Input.mousePosition;
    }

    public virtual void OnEndDrag(PointerEventData data)
    {
        var results = new List<RaycastResult>();
        graphicRaycaster.Raycast(data, results);
        foreach (var hit in results)
        {
            var slot = hit.gameObject.GetComponent<DragSlot>();
            if (slot)
            {
                if (!slot.SlotFilled)
                {
                    currentSlot.CurrentItem = null;
                    currentSlot = slot;
                    currentSlot.CurrentItem = this;
                }
                break;
            }
        }
        transform.SetParent(currentSlot.transform);
        transform.SetAsLastSibling();
        transform.localPosition = Vector3.zero;
    }
    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        graphicRaycaster = canvas.GetComponent<GraphicRaycaster>();
    }
}
