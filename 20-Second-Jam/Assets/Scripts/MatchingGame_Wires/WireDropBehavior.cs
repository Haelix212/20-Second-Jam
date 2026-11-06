using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WireBehaviorDropBehavior : MonoBehaviour, IDragBehavior
{
    Color wireColor;

    void Start()
    {
        wireColor = GetComponent<Image>().color;
    }

    public void OnStartDragAction()
    {

    }

    public void OnSuccessDropAction(GameObject slot)
    {

    }

    public void OnFailedDropAction()
    {

    }

    public void OnEndDragAction(GameObject slot)
    {
        if (slot != null)
        {
            Color slotColor = slot.GetComponent<Image>().color;
            if (wireColor == slotColor)
            {
                Debug.Log("Color Matched!");
                GetComponent<DragDrop>().enabled = false;
                this.transform.parent.GetComponent<MatchingGame>().WireMatched();
            }
            else
            {
                Debug.Log("Color Did Not Match");
                GetComponent<DragDrop>().ReturnToLastPosition();
            }
        }
    }

}
