using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WireBehaviorDropBehavior : MonoBehaviour, IDragBehavior
{

    private GameObject slot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

    public void OnEndDragAction()
    {
        Debug.Log("Example Ended Dragging");
    }

}
