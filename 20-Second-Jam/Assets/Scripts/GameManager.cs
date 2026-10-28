using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState
    {
        MainMenu,
        Playing,
        Paused, // Idk if we even want this cause of the 20 second thing
        GameOver
    }

    public GameState CurrentState { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
        DontDestroyOnLoad(gameObject);
    }

    public void UpdateGameState(GameState newState)
    {
        //State = newState;

        //switch (newState)
        //{
        //    case GameState.MainMenu();
        //    break;
        //}
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
