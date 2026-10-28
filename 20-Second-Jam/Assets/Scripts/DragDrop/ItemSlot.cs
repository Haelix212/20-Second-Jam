using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        //we can use this to trigger puzzle things
        GameObject draggedObject = eventData.pointerDrag;

        if (draggedObject != null)
        {
            // Snap the dragged object to this slot. We need to adjust logic for smth like wires
            RectTransform draggedRectTransform = draggedObject.GetComponent<RectTransform>();
            draggedRectTransform.anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
        }
    }
}