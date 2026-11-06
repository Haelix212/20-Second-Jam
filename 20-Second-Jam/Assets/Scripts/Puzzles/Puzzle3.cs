using UnityEngine;

public class Puzzle3 : MonoBehaviour, IDragBehavior
{

    public void OnStartDragAction()
    {
    }

    public void OnSuccessDropAction(GameObject slot)
    {
        if (slot.name == "Slot3")
        {
            Debug.Log("Correctly Dropped on Slot3");
            // Set puzzle as completed with the PuzzleManager
            PuzzleManager.Instance.SetPuzzleCompleted("Puzzle3");
        }
        else
        {
            Debug.Log("Incorrectly Dropped on " + slot.name);
        }
    }


    public void OnFailedDropAction()
    {
    }

    public void OnEndDragAction(GameObject slot)
    {
    }
}