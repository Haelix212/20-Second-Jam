using UnityEngine;

public class ExampleDragBehavior : MonoBehaviour, IDragBehavior
{

    public void OnStartDragAction()
    {
        Debug.Log("Example Started Dragging");
    }

    public void OnSuccessDropAction(GameObject slot)
    {
        Debug.Log("Example Successfully Dropped");    
    }


    public void OnFailedDropAction()
    {
        Debug.Log("Example Failed to Drop");
    }

    public void OnEndDragAction(GameObject slot)
    {
    }
}
