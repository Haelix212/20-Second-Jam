using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private Canvas timerCanvas;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Canvas menuCanvas;
    [Header("Game Controls")]
    [SerializeField] private float timeRemaining = 20f;
    private float originalTime;
    public bool isGameActive { get; private set; } = false;
    [SerializeField] public GameOverEvent gameOverEvent;
    

    private Transform menuTransform;    // Keeping serialized fields to a minimum
    private Transform settingsTransform;
    private Transform creditsTransform;
    private Transform activeMenu;       // Used to keep back button code simple
    private Transform videoSettings;
    private Transform soundSettings;
    private bool introPlayed = false;
    
    private TextMeshProUGUI timeText;
    private GameObject failPanel;

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

        originalTime = timeRemaining;

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
        if (timeRemaining <= 0)
        {
            GameOver();
        }
        UpdateUI();

        //Debug.Log(Mathf.Round(timeRemaining) + " seconds remaining");
        
    }
    public void StartGame()
    {
        // Load the game scene, rn is my test scene
        if (SceneManager.GetActiveScene().name != "GameScene")
        {
            //introPlayed = true; // Was using this for debugging
            if (!introPlayed)
            {
                introPlayed = true;
                SceneManager.LoadScene("Intro");
                return;
            }
            else
            {
                SceneManager.LoadScene("GameScene");
            }
        }
        // Reset everything including my imaginary puzzle manager that will exist eventually
        // PuzzleManager.Instance.ResetPuzzlesCompleted();

        // If we have a scripted animation before playing it goes here

        timeRemaining += 0.5f;
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
        gameOverEvent.TriggerEvent();

        if (timeRemaining <= 0)
        {
            // Disable raycasts for all the world canvases since they were interfering with the game over canvas
            GameObject[] roots = SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (GameObject item in roots) {
                RaycasterWorld caster = item.GetComponent<RaycasterWorld>();
                if (caster != null) caster.enabled = false;
            }

            failPanel = Instantiate(gameOverPanel);
            failPanel.transform.Find("PlayAgainButton").GetComponent<Button>().onClick.AddListener(RestartGame);
            failPanel.transform.Find("QuitButton").GetComponent<Button>().onClick.AddListener(ReturnMainMenu);
            failPanel.SetActive(true);
        }
        else
        {
            // Win message
            Debug.Log("You win");
            // Some sort of jump away animation plays, might need to spin off a separate scene similar to the intro
        }
    }

    private void UpdateUI()
    {
        if (!isGameActive) return;

        if (timeText == null) return;
        timeText.text = $"00:{timeRemaining:00}";

        if (Mathf.Round(timeRemaining) % 2 == 0) timeText.color = Color.red;
        else timeText.color = Color.yellow;
    }
    public void RestartGame()
    {
        SceneManager.LoadScene("GameScene");
        timeRemaining = originalTime; // reset clock using copied value
        StartGame();
    }

    public void ReturnMainMenu()
    {
        // Load the menu scene
        if (SceneManager.GetActiveScene().name != "MainMenuScene")
        {
            SceneManager.LoadScene("MainMenuScene");
            Destroy(this.gameObject);
        }
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
