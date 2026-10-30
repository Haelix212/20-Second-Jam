using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;
    private Vector2 originalPosition;
    private Vector2 lastPosition;
    // private string itemType; // "Wire", "Battery"
    // private string itemName;
    // private string requiredItemType;
    // return to last and return to original should never both be true, right now just don't click both plz
    [SerializeField] public bool ReturnToLastPositionOnFail;
    [SerializeField] public bool ReturnToOriginalPositionOnFail;
    // [SerializeField] public bool AllowIncorrectTypeDrop;
    // [SerializeField] public Item heldItem;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
        originalPosition = rectTransform.anchoredPosition;
        lastPosition = originalPosition;
        // itemType = heldItem.itemType;
        // itemName = heldItem.itemName;
        // requiredItemType = "Any"; 
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // May not need
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Right now is making transparent, but for wires we want to make a wire
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;

    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        if (eventData.pointerEnter == null || eventData.pointerEnter.GetComponent<ItemSlot>() == null)
        {
            OnDropFail();
        }
        else
        {
            OnDropSuccess();
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
        if (ReturnToOriginalPositionOnFail)
        {
            ResetPosition();
        }

        if (ReturnToLastPositionOnFail)
        {
            ReturnToLastPosition();
        }
    }

    public void OnDropSuccess()
    {
        lastPosition = rectTransform.anchoredPosition;
        // Check if puzzle was completed should be put here
    }
}