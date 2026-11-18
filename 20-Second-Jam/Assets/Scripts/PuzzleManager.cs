using UnityEngine;
using System.Collections.Generic;
public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance { get; private set; }

    [SerializeField] private List<GameObject> allPuzzles; // List of all puzzles in the game
    [SerializeField] private Dictionary<string, GameObject> puzzleDictionary; // Dictionary to map puzzle names to their game objects
    [SerializeField] private int numberOfPuzzlesToActivate = 2; // Number of puzzles to activate at the start of the game
    [SerializeField] private List<GameObject> activePuzzles; // List of currently active puzzles
    [SerializeField] public PuzzlesCompleteEvent puzzlesCompleteEvent; // Event that triggers when all puzzles are complete
    [SerializeField] private GameObject progressBar;
    private int completedPuzzles = 0;
    private List<Light> progressLights = new List<Light>();
    private List<Renderer> progressMeshes = new List<Renderer>();


    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        // Initialize the list of active puzzles
        activePuzzles = new List<GameObject>();
        // Randomly select puzzles to activate
        for (int i = 0; i < numberOfPuzzlesToActivate; i++)
        {
            int randomIndex = Random.Range(0, allPuzzles.Count);
            if (activePuzzles.Contains(allPuzzles[randomIndex]))
            {
                i--;
                continue;
            }
            GameObject puzzleToActivate = allPuzzles[randomIndex];
            activePuzzles.Add(puzzleToActivate);
            puzzleToActivate.SetActive(true);
        }
        InitializeProgressLights();
    }
    void InitializeProgressLights()
    {
        foreach (Transform child in progressBar.transform)
        {
            Light pointLight = child.GetComponentInChildren<Light>();
            Renderer mesh = child.GetComponentInChildren<Renderer>();

            if (pointLight != null && mesh != null)
            {
                progressLights.Add(pointLight);
                progressMeshes.Add(mesh);

                pointLight.color = Color.red;
            }
        }
    }
    void ActivatePuzzle(GameObject puzzle)
    {
        if (!activePuzzles.Contains(puzzle))
        {
            activePuzzles.Add(puzzle);
            puzzle.SetActive(true);
        }
    }
    void UpdateProgressLights()
    {
        for (int i = 0; i < progressLights.Count; i++)
        {
            progressLights[i].color =
                i < completedPuzzles ? Color.green : Color.red;
        }
    }

    void CheckWinCondition()
    {
        if (activePuzzles.Count <= 0)
        {
            Debug.Log("All puzzles completed");
            puzzlesCompleteEvent.TriggerEvent();
        }
    }


    public void SetPuzzleCompleted(string puzzleName)
    {
        if (puzzleDictionary.TryGetValue(puzzleName, out GameObject puzzle))
        {
            if (activePuzzles.Remove(puzzle))
            {
                completedPuzzles++;

                puzzle.SetActive(false);

                UpdateProgressLights();

                CheckWinCondition();
            }
        }
    }
}
