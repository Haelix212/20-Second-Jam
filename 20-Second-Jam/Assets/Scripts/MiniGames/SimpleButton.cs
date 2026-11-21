using UnityEngine;
using UnityEngine.UI;

public class SimpleButton : PuzzleClass
{
    [Header("Set this to a unique int when hooking up to PuzzleManager.cs")]
    [SerializeField] int SimpleButtonNumber = -1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // StartPuzzle();
    }

    public override void StartPuzzle()
    {
        this.GetComponent<Button>().interactable = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonClicked()
    {
        PuzzleManager.Instance.SetPuzzleCompleted("SimpleButton" + SimpleButtonNumber);
    }
}
