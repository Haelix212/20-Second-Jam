using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerUpHandler, IUpdateSelectedHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;
    private IDragBehavior specialBehavior;
    private Vector2 originalPosition;
    private Vector2 lastPosition;
    private BoxCollider canvasCollider;
    public GameObject slot;
    public float maxRaycastDistance = 5f;
    public float wireProximityCheck = 0.2f;
    private GameObject currentObject;

    // return to last and return to original should never both be true, right now just don't click both plz
    [SerializeField] public bool ReturnToLastPositionOnFail;
    [SerializeField] public bool ReturnToOriginalPositionOnFail;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
        canvasCollider = canvas.GetComponent<BoxCollider>();
        specialBehavior = GetComponent<IDragBehavior>();
        originalPosition = rectTransform.anchoredPosition;
        lastPosition = originalPosition;
        slot = null;
        currentObject = this.gameObject;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (canvasCollider == null) return;

        lastPosition = this.transform.position;
        canvasGroup.alpha = 0.6f;

        EventSystem.current.SetSelectedGameObject(currentObject);
        canvasGroup.blocksRaycasts = false;

    }

    public void OnUpdateSelected(BaseEventData eventData)
    {
        if (canvasCollider == null) return;
        
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        if (canvasCollider.Raycast(ray, out RaycastHit hit, maxRaycastDistance))
        {
            this.transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (canvasCollider == null) return;

        EventSystem.current.SetSelectedGameObject(null);
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        ItemSlot[] slots = canvas.transform.GetComponentsInChildren<ItemSlot>();
        foreach (ItemSlot item in slots)
        {
            if (Vector3.Distance(item.transform.position, this.transform.position) < wireProximityCheck) {
                slot = item.gameObject;
                this.transform.position = slot.transform.position;
                //specialBehavior?.OnEndDragAction(slot);
                OnDropSuccess(slot);
            }
            else
            {
                OnDropFail();
            }
        }
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
        Debug.Log("dragging " + eventData.selectedObject.name);

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        specialBehavior?.OnEndDragAction(slot);

        if (slot == null)
        {
            OnDropFail();
        }
        else
        {
            OnDropSuccess(slot);
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

    public void OnDropSuccess(GameObject slot)
    {
        lastPosition = rectTransform.anchoredPosition;
        specialBehavior?.OnSuccessDropAction(slot);
    }

    public GameObject GetDraggedObject()
    {
        return currentObject;
    }
}