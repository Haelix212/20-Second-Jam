using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WireBehaviorDropBehavior : MonoBehaviour, IDragBehavior
{

    public void OnStartDragAction()
    {

        Debug.Log("Example Started Dragging");
    }

    public void OnSuccessDropAction()
    {
        //Check if the wire is connected to the correct slot
        //If right, increment score and check win con, snap wire to slot, and disable dragging
        //else return wire to original position

        GetComponent<DragDrop>().ReturnToLastPosition();
        GetComponent<DragDrop>().enabled = false;
        Debug.Log("Example Successfully Dropped");
    }

    public void OnFailedDropAction()
    {
        //return to original position
        //GetComponent<DragDrop>().ResetPosition();
        Debug.Log("Example Failed to Drop");
    }

    public void OnEndDragAction(GameObject slot)
    {
        Debug.Log("Example Ended Dragging");
    }

}
