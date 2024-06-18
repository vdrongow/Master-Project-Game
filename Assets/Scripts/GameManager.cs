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
    
    private int _sceneIndex;

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
    
    private void Start()
    {
        _sceneIndex = 0;
    }
    
    public void StartLevel(ESortingAlgorithm sortingAlgorithm, ESortType sortType, int arraySize)
    {
        Session = new Session(Learner, sortingAlgorithm, sortType, arraySize);
        
        LoadNextScene();
    }
    
    public void LoadNextScene()
    {
        var currentScene = SceneManager.GetActiveScene();
        if(currentScene.name == gameSettings.sceneNames[gameSettings.sceneNames.Length - 1])
        {
            _sceneIndex = 0;
        }
        else
        {
            _sceneIndex++;
        }
        if(_sceneIndex >= gameSettings.sceneNames.Length)
        {
            _sceneIndex = 0;
        }
        
        var sceneName = gameSettings.sceneNames[_sceneIndex];
        SceneManager.LoadScene(sceneName);
    }
}