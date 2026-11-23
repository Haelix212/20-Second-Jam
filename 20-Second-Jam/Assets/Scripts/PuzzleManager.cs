using UnityEngine;
using System.Collections.Generic;
// using System.ComponentModel.DataAnnotations;
public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance { get; private set; }

    [SerializeField] private List<GameObject> allPuzzles; // List of all puzzles in the game
    [SerializeField] private List<GameObject> easyPuzzles;
    [SerializeField] private List<GameObject> hardPuzzles;
    [SerializeField] private Dictionary<string, GameObject> puzzleDictionary; // Dictionary to map puzzle names to their game objects
    [SerializeField] private int numberOfPuzzlesToActivate = 2; // Number of puzzles to activate at the start of the game
    [SerializeField] private List<GameObject> activePuzzles; // List of currently active puzzles
    [SerializeField] public PuzzlesCompleteEvent puzzlesCompleteEvent; // Event that triggers when all puzzles are complete
    [SerializeField] private GameObject progressBar;
    [SerializeField] private Material red;
    [SerializeField] private Material green;
    private int completedPuzzles = 0;
    private int hardCount;
    private int easyCount;
    private List<Renderer> progressLights = new List<Renderer>();
    List<GameObject> puzzlesToActivate = new List<GameObject>();


    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        hardCount = numberOfPuzzlesToActivate /3;
        easyCount = numberOfPuzzlesToActivate - hardCount;
        // Randomly select puzzles to activate
        GetRandomPuzzles(hardPuzzles, hardCount);
        GetRandomPuzzles(easyPuzzles, easyCount);
        foreach (GameObject puzzle in puzzlesToActivate)
        {
            activePuzzles.Add(puzzle);
            puzzle.SetActive(true);
            puzzle.transform.Find("PuzzleLight").GetComponentInChildren<Animation>().Play();
            puzzle.GetComponentInChildren<PuzzleClass>().StartPuzzle();
            
        }
        
        InitializeProgressLights();
    }

    private void GetRandomPuzzles(List<GameObject> source, int puzzleCount)
    {
        for (int i = 0; i < puzzleCount; i++)
            {
                int randomIndex = Random.Range(0, source.Count);
                if (puzzlesToActivate.Contains(source[randomIndex]))
                {
                    i--;
                    continue;
                }
                puzzlesToActivate.Add(source[randomIndex]);
            }
    }
    void InitializeProgressLights()
    {
        foreach (Transform child in progressBar.transform)
        {
            if (activePuzzles.Count == progressLights.Count)
            {
                break;
            }
            child.gameObject.SetActive(true);
            // Light pointLight = child.GetComponentInChildren<Light>();
            Renderer[] renderers = child.GetComponentsInChildren<Renderer>();
            Renderer mesh = renderers.Length > 1 ? renderers[1] : null;


            if (mesh != null)
            {
                progressLights.Add(mesh);
                mesh.material = red;
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
            progressLights[i].material =
                i < completedPuzzles ? green : red;
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

                // puzzle.SetActive(false);

                Transform light = puzzle.transform.Find("PuzzleLight");
                light.GetComponentInChildren<Animation>().Stop();
                light.GetComponentInChildren<Light>().color = Color.green;
                light.GetComponentInChildren<Light>().intensity = 1f;

                UpdateProgressLights();

                CheckWinCondition();
            }
        }
    }
}
