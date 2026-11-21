using UnityEngine;

public class PuzzleClass : MonoBehaviour
{
    // This literally only exists so the PuzzleManager can call StartPuzzle for each attached puzzle
    public virtual void StartPuzzle()
    {
        Debug.Log("You should never see this");
    }
}
