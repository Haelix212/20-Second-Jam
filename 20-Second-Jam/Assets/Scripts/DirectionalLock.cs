using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Diagnostics.Tracing;

public class DirectionalLock : MonoBehaviour
{

    private int puzzleCount = 0;
    public int winCondition;

    private InputAction UpAction;
    private InputAction DownAction;
    private InputAction LeftAction;
    private InputAction RightAction;

    public int puzzleLength;
    public int puzzleLengthIncrement;
    private int puzzleIndex = 0;


    private KeyCode[] possibleDirections = new KeyCode[] { KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow };
    private List<KeyCode> puzzleAnswer = new List<KeyCode>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpAction = InputSystem.actions.FindAction("Up");
        DownAction = InputSystem.actions.FindAction("Down");
        LeftAction = InputSystem.actions.FindAction("Left");
        RightAction = InputSystem.actions.FindAction("Right");

        GeneratePuzzle();
    }

    // Update is called once per frame
    void Update()
    {
        if (UpAction.triggered)
        {
            CheckPuzzle(KeyCode.UpArrow);
        }
        else if (DownAction.triggered)
        {
            CheckPuzzle(KeyCode.DownArrow);
        }
        else if (LeftAction.triggered)
        {
            CheckPuzzle(KeyCode.LeftArrow);
        }
        else if (RightAction.triggered)
        {
            CheckPuzzle(KeyCode.RightArrow);
        }
    }

    private void GeneratePuzzle()
    {
        puzzleAnswer.Clear();
        for (int i = 0; i < puzzleLength; i++)
        {
            KeyCode randomDirection = possibleDirections[Random.Range(0, possibleDirections.Length-1)];   
            puzzleAnswer.Add(randomDirection);
        }
        print("Code Sequence: " + string.Join(", ", puzzleAnswer));
    }

    //Checks player input
    private void CheckPuzzle (KeyCode input)
    {
        //if puzzleIndex is within the bounds of the answer listm go to next check
        if (puzzleIndex < puzzleAnswer.Count)
        {
            // if the input matches the current index of the answer, increment the index.
            if (input == puzzleAnswer[puzzleIndex])
            {
                puzzleIndex++;

                if (puzzleIndex == puzzleAnswer.Count)
                {
                    puzzleCount++;

                    if (CheckWinCondition())
                    {
                        print("Directional Lock completed");
                    }
                    else
                    {
                        puzzleIndex = 0;
                        puzzleLength += puzzleLengthIncrement;
                        GeneratePuzzle();
                    }
                }

            }
            //player puts wrong input, reset the index and generate a new puzzle.
            else
            {
                puzzleIndex = 0;
                GeneratePuzzle();
            }
        }
    }

    private bool CheckWinCondition()
    {
        if (puzzleCount >= winCondition)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
