using UnityEngine;

public interface IDragBehavior
{
    void OnStartDragAction();
    void OnSuccessDropAction(GameObject slot);
    void OnFailedDropAction();
    void OnEndDragAction(GameObject slot);
}
