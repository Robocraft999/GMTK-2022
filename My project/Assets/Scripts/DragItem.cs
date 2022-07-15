using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public DragSlot currentSlot;
    private Canvas canvas;
    private GraphicRaycaster graphicRaycaster;

    public void OnBeginDrag(PointerEventData data)
    {
        transform.localPosition += new Vector3(data.delta.x, data.delta.y) / transform.lossyScale.x;

        transform.SetParent(canvas.transform, true);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData data)
    {
        transform.localPosition += new Vector3(data.delta.x, data.delta.y) / transform.lossyScale.x;
    }

    public void OnEndDrag(PointerEventData data)
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
                    if (currentSlot) currentSlot.currentItem = null;
                    currentSlot = slot;
                    currentSlot.currentItem = this;
                }
                break;
            }
        }
        transform.SetParent(currentSlot.transform);
        transform.localPosition = Vector3.zero;
    }
    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        graphicRaycaster = canvas.GetComponent<GraphicRaycaster>();
    }

    void Update()
    {
        
    }
}
