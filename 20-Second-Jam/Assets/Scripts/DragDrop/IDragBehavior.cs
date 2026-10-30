using UnityEngine;

public interface IDragBehavior
{
    void OnStartDragAction();
    void OnSuccessDropAction();
    void OnFailedDropAction();
    void OnEndDragAction();
}
