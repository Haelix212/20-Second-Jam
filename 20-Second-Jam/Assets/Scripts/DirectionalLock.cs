using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DirectionalLock : MonoBehaviour
{

    private int puzzleCount = 0;
    public int winCondition;

    private InputAction UpAction;
    private InputAction DownAction;
    private InputAction LeftAction;
    private InputAction RightAction;

    [Header("Puzzle Variables")]
    public int puzzleLength;
    public int puzzleLengthIncrement;
    private int puzzleIndex = 0;

    public RawImage[] puzzleImages;
    public Texture[] puzzleMaterials;

    private bool puzzleActive = false;


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
        //Locks player from recording input while new puzzle is being generated.
        if(puzzleActive)
        {
            //Checks for input on any arrow key then checks the input againt the current puzzle.
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
    }

    //Uses the current puzzle length to generate a random sequence for our puzzle. 
    private void GeneratePuzzle()
    {
        ClearPuzzle();
        puzzleAnswer.Clear();
        for (int i = 0; i < puzzleLength; i++)
        {
            KeyCode randomDirection = possibleDirections[UnityEngine.Random.Range(0, possibleDirections.Length-1)];   
            puzzleAnswer.Add(randomDirection);
            UpdatePuzzleVisuals(randomDirection,i);
        }

        puzzleActive = true;
        //print("Code Sequence: " + string.Join(", ", puzzleAnswer));
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
                puzzleImages[puzzleIndex].color = Color.green;
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
                        puzzleActive = false;
                        StartCoroutine(WaitAndGeneratePuzzle());
                    }
                }
            }
            //player puts wrong input, reset the index and generate a new puzzle.
            else
            {
                puzzleImages[puzzleIndex].color = Color.red;
                puzzleIndex = 0;
                puzzleActive = false;
                StartCoroutine(WaitAndGeneratePuzzle());
            }
        }
    }

    //Waits for 1 seconds before generating a new puzzle. Provides a lockout period for player.
    private IEnumerator WaitAndGeneratePuzzle()
    {
        yield return new WaitForSeconds(1);
        GeneratePuzzle();
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

    private void UpdatePuzzleVisuals(KeyCode input, int i)
    {
        // Uses a keycode input to assign the which texture to assign. 1 = Up, 2 = Down, 3 = Left, 4 = Right.
        // Also use an integer argument for the index.
        if (input == KeyCode.UpArrow)
        {
            puzzleImages[i].texture = puzzleMaterials[0];
        }
        else if (input == KeyCode.DownArrow)
        {
            puzzleImages[i].texture = puzzleMaterials[1];
        }
        else if (input == KeyCode.LeftArrow)
        {
            puzzleImages[i].texture = puzzleMaterials[2];
        }
        else if (input == KeyCode.RightArrow)
        {
            puzzleImages[i].texture = puzzleMaterials[3];
        } 
    }

    //Clears the textures and colors from the puzzle.
    private void ClearPuzzle()
    {
        for (int i = 0; i < puzzleImages.Length; i++)
        {
            puzzleImages[i].texture = null;
            puzzleImages[i].color = Color.white;
        }
    }
}
