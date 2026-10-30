using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Canvas menuCanvas;
    private float timeRemaining = 20f;
     private bool isGameActive = false;

    private Transform menuTransform;    // Keeping serialized fields to a minimum
    private Transform settingsTransform;
    private Transform creditsTransform;
    private Transform activeMenu;       // Used to keep back button code simple
    public bool IsGameActive => isGameActive;



    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
        DontDestroyOnLoad(gameObject);

        menuTransform = menuCanvas.transform.Find("MenuParent");
        settingsTransform = menuCanvas.transform.Find("SettingsParent");
        creditsTransform = menuCanvas.transform.Find("CreditsParent");
    }


    void Start()
    {
        
    }
    private void Update()
    {
        if (!isGameActive) return;
        
        timeRemaining -= Time.deltaTime;
        UpdateUI();
    }
    public void StartGame()
    {
        // Load the game scene, rn is my test scene
        if (SceneManager.GetActiveScene().name != "Movement")
        {
            SceneManager.LoadScene("Movement");
        }
        // Reset everything including my imaginary puzzle manager that will exist eventually
        // PuzzleManager.Instance.ResetPuzzlesCompleted();
        timeRemaining = 20f;
        isGameActive = true;
        
        // Hide gameover panel
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        UpdateUI();
    }
    public void GameOver()
    {
        isGameActive = false;
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }
    private void UpdateUI()
    {
        if (timeText == null) return;
        timeText.text = $"TIME TILL MELTDOWN:{timeRemaining:00}";
    }
    public void RestartGame()
    {
        StartGame();
    }

    public void OpenSettings()
    {
        menuTransform.gameObject.SetActive(false);
        settingsTransform.gameObject.SetActive(true);

        activeMenu = settingsTransform; // Saving this for GoBack() to use
    }

    public void OpenCredits()
    {
        menuTransform.gameObject.SetActive(false);
        creditsTransform.gameObject.SetActive(true);

        activeMenu = creditsTransform; // Saving this for GoBack() to use
    }
    
    public void GoBack()
    {
        activeMenu.gameObject.SetActive(false);
        menuTransform.gameObject.SetActive(true);
    }
}
