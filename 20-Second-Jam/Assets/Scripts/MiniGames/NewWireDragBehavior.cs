using UnityEngine;

public class NewWireDragBehavior : MonoBehaviour, IDragBehavior
{
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
    }

    public void OnSuccessDropAction(GameObject slot)
    {
        if (slot.tag == this.tag)
        {
            Debug.Log("Correctly Dropped");
            // Set puzzle as completed with the PuzzleManager
            this.transform.parent.GetComponent<NewWires>().WireMatched();
            //PuzzleManager.Instance.SetPuzzleCompleted("Puzzle3");
        }
        else
        {
            Debug.Log("Incorrectly Dropped on " + slot.tag);
            this.GetComponent<DragDrop>().ResetPosition();
        }
    }


    public void OnFailedDropAction()
    {
    }

    public void OnEndDragAction(GameObject slot)
    {
    }


}
