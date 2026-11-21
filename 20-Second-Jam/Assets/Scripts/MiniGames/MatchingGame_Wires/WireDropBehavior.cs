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
        print("On start drag action");
    }

    public void OnSuccessDropAction(GameObject slot)
    {

    }

    public void OnFailedDropAction()
    {

    }

    public void OnEndDragAction(GameObject slot)
    {
        print("On end drag action");

        if (slot != null)
        {
            Color slotColor = slot.GetComponent<Image>().color;
            if (wireColor == slotColor)
            {
                print("Color matched");
                GetComponent<DragDrop>().enabled = false;
                this.transform.parent.GetComponent<MatchingGame>().WireMatched();
            }
            else
            {
                print("wrong color");
                GetComponent<DragDrop>().ReturnToLastPosition();
            }
        }
    }

}
