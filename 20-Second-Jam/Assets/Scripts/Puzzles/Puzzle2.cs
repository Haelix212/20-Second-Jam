using UnityEngine;

public class Puzzle2 : MonoBehaviour, IDragBehavior
{

    public void OnStartDragAction()
    {
    }

    public void OnSuccessDropAction(GameObject slot)
    {
        if (slot.name == "Slot2")
        {
            Debug.Log("Correctly Dropped on Slot2");
            // Set puzzle as completed with the PuzzleManager
            PuzzleManager.Instance.SetPuzzleCompleted("Puzzle2");
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
