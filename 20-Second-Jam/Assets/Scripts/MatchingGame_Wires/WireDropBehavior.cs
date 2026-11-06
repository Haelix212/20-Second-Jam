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

        //Debug.Log("Example Started Dragging");
    }

    public void OnSuccessDropAction(GameObject slot)
    {
        //Check if the wire is connected to the correct slot
        //If right, increment score and check win con, snap wire to slot, and disable dragging
        //else return wire to original position
        if (slot != null)
        {
            Color slotColor = slot.GetComponent<Image>().color;
            if (wireColor == slotColor)
            {
                Debug.Log("Color Matched!");
                GetComponent<DragDrop>().enabled = false;
            }
            else
            {
                Debug.Log("Color Did Not Match");
                GetComponent<DragDrop>().ReturnToLastPosition();
            }
        }
    }

    public void OnFailedDropAction()
    {
        //return to original position
        //GetComponent<DragDrop>().ResetPosition();
        //Debug.Log("Example Failed to Drop");
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
            }
            else
            {
                Debug.Log("Color Did Not Match");
                GetComponent<DragDrop>().ReturnToLastPosition();
            }
         }
       
        Debug.Log("Example Ended Dragging");
    }

}
