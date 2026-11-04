using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static System.Reflection.Metadata.BlobBuilder;

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


    [Header("Puzzle UI")]
    public GameObject directionalLockPrefab;
    public Vector3 spawnPosition;
    public Vector3 spawnOffset;
    public Texture[] puzzleMaterials;

    private List<GameObject> puzzleImages = new List<GameObject>();

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
        ResetPuzzle();
        for (int i = 0; i < puzzleLength; i++)
        {
            KeyCode randomDirection = possibleDirections[UnityEngine.Random.Range(0, possibleDirections.Length-1)];   
            puzzleAnswer.Add(randomDirection);
            UpdatePuzzleVisuals(randomDirection, i);
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
                puzzleImages[puzzleIndex].GetComponent<RawImage>().color = Color.green;
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
        // Creates the prefab for the puzzle and sets its location
        GameObject newImage = Instantiate(directionalLockPrefab, this.transform);
        puzzleImages.Add(newImage);

        RectTransform rectTransform = puzzleImages[i].GetComponent<RectTransform>();

        rectTransform.anchoredPosition = spawnPosition + spawnOffset * i;

        //Using key input to determine which image should be applied
        switch (input)
        {
            case KeyCode.UpArrow:
                puzzleImages[i].GetComponent<RawImage>().texture = puzzleMaterials[0];
                break;
            case KeyCode.DownArrow:
                puzzleImages[i].GetComponent<RawImage>().texture = puzzleMaterials[1];
                break;
            case KeyCode.LeftArrow:
                puzzleImages[i].GetComponent<RawImage>().texture = puzzleMaterials[2];
                break;
            case KeyCode.RightArrow:
                puzzleImages[i].GetComponent<RawImage>().texture = puzzleMaterials[3];
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
}
