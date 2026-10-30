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
        specialBehavior?.OnStartDragAction();

    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        specialBehavior?.OnEndDragAction();

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

    public void OnDropSuccess()
    {
        lastPosition = rectTransform.anchoredPosition;
        specialBehavior?.OnSuccessDropAction();
        // Check if puzzle was completed should be put here
    }
}