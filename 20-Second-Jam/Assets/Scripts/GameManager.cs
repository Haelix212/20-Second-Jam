using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private Canvas timerCanvas;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Canvas menuCanvas;
    private float timeRemaining = 20f;
     private bool isGameActive = false;

    private Transform menuTransform;    // Keeping serialized fields to a minimum
    private Transform settingsTransform;
    private Transform creditsTransform;
    private Transform activeMenu;       // Used to keep back button code simple
    private Transform videoSettings;
    private Transform soundSettings;
    
    private TextMeshProUGUI timeText;
    
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
        DontDestroyOnLoad(timerCanvas.gameObject);

        menuTransform = menuCanvas.transform.Find("MenuParent");
        settingsTransform = menuCanvas.transform.Find("SettingsParent");
        creditsTransform = menuCanvas.transform.Find("CreditsParent");

        videoSettings = settingsTransform.Find("VideoParent");
        soundSettings = settingsTransform.Find("SoundParent");
    }


    void Start()
    {
        
    }
    private void Update()
    {
        if (!isGameActive) return;
        
        timeRemaining -= Time.deltaTime;
        UpdateUI();

        //Debug.Log(Mathf.Round(timeRemaining) + " seconds remaining");
        
    }
    public void StartGame()
    {
        // Load the game scene, rn is my test scene
        if (SceneManager.GetActiveScene().name != "GameScene")
        {
            SceneManager.LoadScene("GameScene");
        }
        // Reset everything including my imaginary puzzle manager that will exist eventually
        // PuzzleManager.Instance.ResetPuzzlesCompleted();

        // If we have a scripted animation before playing it goes here

        timeRemaining = 20.5f;
        isGameActive = true;
        timeText = timerCanvas.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        
        
        // Hide gameover panel
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        UpdateUI();
        timeText.gameObject.SetActive(true);
    }
    public void GameOver()
    {
        isGameActive = false;
        if (timeRemaining <= 0)
        {
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);
            }
        }
        else
        {
            // Win message
        }
        
    }
    private void UpdateUI()
    {
        if (timeText == null) return;
        timeText.text = $"00:{timeRemaining:00}";
        if (Mathf.Round(timeRemaining) % 2 == 0) timeText.color = Color.red;
        else timeText.color = Color.yellow;
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

    public void ChangeSettingsPage()
    {
        if (soundSettings.gameObject.activeSelf)
        {
            soundSettings.gameObject.SetActive(false);
            videoSettings.gameObject.SetActive(true);
        }
        else
        {
            videoSettings.gameObject.SetActive(false);
            soundSettings.gameObject.SetActive(true);
        }
    }
}
