using Adlete;
using Configs;
using Enums;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class GameManager : MonoBehaviour
{
    public static GameManager Singleton { get; private set; } = null!;
    
    [HideInInspector]
    public GameState gameState = null!;
    
    [Header("Configs")]
    public GameSettings gameSettings = null!;
    public ArraySettings arraySettings = null!;
    
    [Header("Game State")]
    public bool isGameRunning;
    public bool isGamePaused;
    public int mistakeCount;

    public Learner Learner;
    public Session Session;

    private void Awake()
    {
        if(Singleton == null)
        {
            Singleton = this;
            DontDestroyOnLoad(this);
        }
        else if(Singleton != this)
        {
            Destroy(gameObject);
        }
        
        gameState = GetComponent<GameState>();
        
       Session = new Session(Learner, ESortingAlgorithm.BubbleSort, ESortType.Unsorted, 5);
    }

    public void StartLevel(ESortingAlgorithm sortingAlgorithm, ESortType sortType, int arraySize)
    {
        Session = new Session(Learner, sortingAlgorithm, sortType, arraySize);
        
        LoadScene(Constants.LEVEL_SCENE);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}