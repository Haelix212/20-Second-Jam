using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static System.Reflection.Metadata.BlobBuilder;

public class DirectionalLock : PuzzleClass
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
    public float lockOutTimer;
    private int puzzleIndex = 0;


    [Header("Puzzle UI")]
    public GameObject directionalLockPrefab;

    private List<GameObject> puzzleImages = new List<GameObject>();
    private Vector3 spawnOffset;

    private bool puzzleActive = false;

    private KeyCode[] possibleDirections = new KeyCode[] { KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow };
    private List<KeyCode> puzzleAnswer = new List<KeyCode>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // StartPuzzle();
    }

    public override void StartPuzzle()
    {
        UpAction = InputSystem.actions.FindAction("Up");
        DownAction = InputSystem.actions.FindAction("Down");
        LeftAction = InputSystem.actions.FindAction("Left");
        RightAction = InputSystem.actions.FindAction("Right");

        spawnOffset = new Vector3(directionalLockPrefab.GetComponent<RectTransform>().rect.width, 0, 0);

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
        ResetPuzzle();
        for (int i = 0; i < puzzleLength; i++)
        {
            KeyCode randomDirection = possibleDirections[UnityEngine.Random.Range(0, possibleDirections.Length-1)];   
            puzzleAnswer.Add(randomDirection);
            UpdatePuzzleVisuals(randomDirection, i);
        }
        AdjustPanelWidth();
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
                puzzleImages[puzzleIndex].GetComponent<RawImage>().color = Color.green;
                puzzleIndex++;

                if (puzzleIndex == puzzleAnswer.Count)
                {
                    puzzleCount++;

                    if (CheckWinCondition())
                    {
                        print("Directional Lock completed");
                        PuzzleManager.Instance.SetPuzzleCompleted("Directional Lock");
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
                puzzleImages[puzzleIndex].GetComponent<RawImage>().color = Color.red;
                puzzleIndex = 0;
                puzzleActive = false;
                StartCoroutine(WaitAndGeneratePuzzle());
            }
        }
    }

    //Waits for 1 seconds before generating a new puzzle. Provides a lockout period for player.
    private IEnumerator WaitAndGeneratePuzzle()
    {
        yield return new WaitForSeconds(lockOutTimer);
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
        // Creates the prefab for the puzzle and sets its location
        GameObject newImage = Instantiate(directionalLockPrefab, this.transform);
        puzzleImages.Add(newImage);

        RectTransform rectTransform = puzzleImages[i].GetComponent<RectTransform>();

        // First Spawnoffset is the spawn postion then adds a the additional offsets needed for i.
        rectTransform.anchoredPosition = spawnOffset + spawnOffset * i;

        // Using key input to determine which way image should be rotated
        // Arrows are down by default so no case for DownArrow
        switch (input)
        {
            case KeyCode.UpArrow:
                puzzleImages[i].transform.Rotate(0, 0, 180);
                break;
            case KeyCode.LeftArrow:
                puzzleImages[i].transform.Rotate(0, 0, 270);
                break;
            case KeyCode.RightArrow:
                puzzleImages[i].transform.Rotate(0, 0, 90);
                break;
        }
    }

    //Destroys the previous puzzle prefabs and clears the lists of all of their contents.
    private void ResetPuzzle()
    {
        foreach (GameObject image in puzzleImages)
        {
            Destroy(image);
        }
        puzzleImages.Clear();
        puzzleAnswer.Clear();
    }

    private void AdjustPanelWidth()
    {
        RectTransform rectTransform = this.GetComponent<RectTransform>();
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, spawnOffset.x * (puzzleLength+1));
    }
}
