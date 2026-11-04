using UnityEngine;

public class Puzzle1 : MonoBehaviour, IDragBehavior
{

    public void OnStartDragAction()
    {
    }

    public void OnSuccessDropAction(GameObject slot)
    {
        if (slot.name == "Slot1")
        {
            Debug.Log("Correctly Dropped on Slot1");
            // Set puzzle as completed with the PuzzleManager
            PuzzleManager.Instance.SetPuzzleCompleted("Puzzle1");
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
