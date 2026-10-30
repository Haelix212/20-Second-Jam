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

    public void OnEndDragAction()
    {
        Debug.Log("Example Ended Dragging");
    }
}
