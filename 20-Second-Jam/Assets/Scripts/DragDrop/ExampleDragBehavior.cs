using UnityEngine;

public class ExampleDragBehavior : MonoBehaviour, IDragBehavior
{

    public void OnStartDragAction()
    {
        Debug.Log("Example Started Dragging");
    }

    public void OnSuccessDropAction()
    {
        Debug.Log("Example Successfully Dropped");    
    }


    public void OnFailedDropAction()
    {
        Debug.Log("Example Failed to Drop");
    }

    public void OnEndDragAction(GameObject slot)
    {
        Debug.Log("Example Ended Dragging");
        if (slot != null)
        {
            Debug.Log("Dropped on slot: " + slot.name);
        }
    }
}
