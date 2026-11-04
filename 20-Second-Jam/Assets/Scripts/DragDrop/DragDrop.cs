using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;
    private IDragBehavior specialBehavior;
    private Vector2 originalPosition;
    private Vector2 lastPosition;
    public GameObject slot;
   
    // return to last and return to original should never both be true, right now just don't click both plz
    [SerializeField] public bool ReturnToLastPositionOnFail;
    [SerializeField] public bool ReturnToOriginalPositionOnFail;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
        specialBehavior = GetComponent<IDragBehavior>();
        originalPosition = rectTransform.anchoredPosition;
        lastPosition = originalPosition;
        slot = null;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // May not need
        Debug.Log(this.name);
        //if (eventData == null) Debug.Log("null eventData");
        Debug.Log("Clicking on " + eventData.selectedObject.name); //we're getting null eventData for some reason
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Dragging " + eventData.selectedObject.name);

        // Right now is making transparent, but for wires we want to make a wire
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        specialBehavior?.OnStartDragAction();

    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("dragging " + eventData.selectedObject.name);

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        slot = eventData.pointerEnter?.GetComponent<ItemSlot>()?.gameObject;
        specialBehavior?.OnEndDragAction(slot);

        if (eventData.pointerEnter == null || eventData.pointerEnter.GetComponent<ItemSlot>() == null)
        {
            OnDropFail();
        }
        else
        {
            OnDropSuccess(eventData);
        }

    }

    public void ResetPosition()
    {
        rectTransform.anchoredPosition = originalPosition;
    }

    public void ReturnToLastPosition()
    {
        rectTransform.anchoredPosition = lastPosition;
    }

    public void OnDropFail()
    {
        specialBehavior?.OnFailedDropAction();
        if (ReturnToOriginalPositionOnFail)
        {
            ResetPosition();
        }

        if (ReturnToLastPositionOnFail)
        {
            ReturnToLastPosition();
        }
    }

    public void OnDropSuccess(PointerEventData eventData)
    {
        slot = eventData.pointerEnter.GetComponent<ItemSlot>()?.gameObject;
        lastPosition = rectTransform.anchoredPosition;
        specialBehavior?.OnSuccessDropAction(slot);
    }

    public GameObject GetDraggedObject()
    {
        return this.gameObject;
    }
}